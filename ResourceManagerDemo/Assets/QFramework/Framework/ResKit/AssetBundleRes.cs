using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    public class AssetBundleRes : Res
    {
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

        public override bool LoadSync()
        {
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