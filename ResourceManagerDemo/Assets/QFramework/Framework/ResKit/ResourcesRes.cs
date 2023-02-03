using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace QFramework
{
    public class ResourcesRes :Res
    {
        private string mAssetPath;
        public ResourcesRes(string assetPath)
        {
            mAssetPath = assetPath.Substring("resources://".Length);

            Name = assetPath;

            ResState = ResState.Waiting;
        }

        public override bool LoadSync()
        {
            ResState = ResState.Loading;
            Asset = Resources.Load(mAssetPath);
            ResState = ResState.loaded;
            return Asset;
        }

        public override void LoadAsync(Action<Res> onLoaded)
        {
            ResState = ResState.Loading;
            var resRequest = Resources.LoadAsync(mAssetPath);

            resRequest.completed += operation =>
            {
                Asset = resRequest.asset;
                ResState = ResState.loaded;
                onLoaded(this);
            };
        }

        protected override void OnReleaseRes()
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

