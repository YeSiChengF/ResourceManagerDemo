using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    public class AssetBundleRes : Res
    {
        //���AssetBundleManifest�Ĵ�������1-��ʼ������(�����ô����ȸ���)������2-��һ�δ���AssetBundle��ʱ�����
        //��������������Ҫ���������ļ��أ��첽�Ļص���Ƚ��鷳
        static AssetBundleManifest mManifest;
        public static AssetBundleManifest Manifest
        {
            get {
                if (mManifest == null)
                {
                    var mainBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/StreamingAssets");
                    mManifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
                return mManifest;
            }
        }
        private string mAssetPath;

        public AssetBundle AssetBundle { 
            get { return Asset as AssetBundle; }
            private set { Asset = value; }
        }

        public AssetBundleRes(string assetPath)
        {
            mAssetPath = assetPath;

            Name = assetPath;
        }
        private ResLoader mResLoader = new ResLoader();

        public override bool LoadSync()
        {
            string[] dependencies =  Manifest.GetDirectDependencies(mAssetPath.Substring(Application.streamingAssetsPath.Length - 1));//streamingAssetsPathĩβ��б��
            foreach (var dependencyBundleName in dependencies)
            {
                mResLoader.LoadSync<AssetBundle>(dependencyBundleName);
            }
            return AssetBundle = AssetBundle.LoadFromFile(mAssetPath);
        }

        public override void LoadAsync(Action<Res> onLoaded)
        {
            var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(mAssetPath);

            assetBundleCreateRequest.completed += operation =>
            {
                AssetBundle = assetBundleCreateRequest.assetBundle;

                onLoaded(this);
               
            };
        }

        protected override void OnReleaseRes()
        {
            AssetBundle assetBundle = AssetBundle;

            if (assetBundle!= null)
            {
                assetBundle.Unload(true);
                Asset = null;
            }

            ResMgr.Instance.SharedLoadedReses.Remove(this);
        }

    }
}