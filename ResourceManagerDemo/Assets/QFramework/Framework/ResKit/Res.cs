using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    public class Res : SimpleRC
    {
        public Object Asset { get; private set; }

        public string Name { get; private set; }

        private string mAssetPath;

        public Res(string assetPath)
        {
            mAssetPath = assetPath;

            Name = assetPath;
        }

        public bool LoadSync()
        {
            return Asset = Resources.Load(mAssetPath);
        }

        public void LoadAsync(Action<Res> onLoaded)
        {
            var resRequest = Resources.LoadAsync(mAssetPath);

            resRequest.completed += operation =>
            {
                Asset = resRequest.asset;

                onLoaded(this);
               
            };
        }

        protected override void OnZeroRef()
        {
            if (Asset is GameObject)
            {
                Asset = null;
                
                Resources.UnloadUnusedAssets();
            }
            else
            {
                Resources.UnloadAsset(Asset);
            }

            ResMgr.Instance.SharedLoadedReses.Remove(this);
                
            Asset = null;
        }
    }
}