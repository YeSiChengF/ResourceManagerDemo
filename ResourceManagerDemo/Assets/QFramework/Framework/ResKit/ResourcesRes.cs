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
        }

        public override bool LoadSync()
        {
            return Asset = Resources.Load(mAssetPath);
        }

        public override void LoadAsync(Action<Res> onLoaded)
        {
            var resRequest = Resources.LoadAsync(mAssetPath);

            resRequest.completed += operation =>
            {
                Asset = resRequest.asset;

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

