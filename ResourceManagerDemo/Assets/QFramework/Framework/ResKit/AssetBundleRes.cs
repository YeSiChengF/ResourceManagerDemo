using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    //AssetBundle的加载类
    public class AssetBundleRes : Res
    {
        //添加AssetBundleManifest的处理，方案1-初始化加载(后续好处理热更新)、方案2-第一次创建AssetBundle的时候加载
        //加载了依赖还需要处理依赖的加载，异步的回调会比较麻烦
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
            ResState = ResState.Waiting;
        }
        private ResLoader mResLoader = new ResLoader();

        public override bool LoadSync()
        {
            ResState = ResState.Loading;
            string[] dependencies =  Manifest.GetDirectDependencies(mAssetPath.Substring(Application.streamingAssetsPath.Length - 1));//streamingAssetsPath末尾有斜杠
            foreach (var dependencyBundleName in dependencies)
            {
                mResLoader.LoadSync<AssetBundle>(Application.streamingAssetsPath + "/" + dependencyBundleName);
            }
            AssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + mAssetPath);
            ResState = ResState.loaded;
            return AssetBundle;
        }

        public override void LoadAsync(Action<Res> onLoaded)
        {
            ResState = ResState.Loading;
            var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + mAssetPath);

            assetBundleCreateRequest.completed += operation =>
            {
                AssetBundle = assetBundleCreateRequest.assetBundle;
                ResState = ResState.loaded;
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
                mResLoader.ReleaseAll();
                mResLoader = null;
            }

            ResMgr.Instance.SharedLoadedReses.Remove(this);
        }

    }
}