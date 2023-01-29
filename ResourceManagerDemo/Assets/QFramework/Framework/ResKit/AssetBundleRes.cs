using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    public class AssetBundleRes : SimpleRC
    {
        public AssetBundle Asset { get; private set; }

        public string Name { get; private set; }

        private string mAssetPath;

        public AssetBundleRes(string assetPath)
        {
            mAssetPath = assetPath;

            Name = assetPath;
        }

        public bool LoadSync()
        {
            return Asset = AssetBundle.LoadFromFile(mAssetPath);
        }

        public void LoadAsync(Action<AssetBundleRes> onLoaded)
        {
            var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(mAssetPath);

            assetBundleCreateRequest.completed += operation =>
            {
                Asset = assetBundleCreateRequest.assetBundle;

                onLoaded(this);
               
            };
        }

        protected override void OnZeroRef()
        {
            var assetBundle = Asset;
            assetBundle.Unload(true);
        }
    }
}