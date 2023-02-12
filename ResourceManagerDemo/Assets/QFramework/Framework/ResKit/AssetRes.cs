using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace QFramework
{
    public class AssetRes : Res
    {
        private string mOwnerBundleName;
        public AssetRes(string assetName,string ownerBundleName)
        {
            Name = assetName; 
            mOwnerBundleName = ownerBundleName;
            ResState = ResState.Waiting;
        }
        private ResLoader mResLoader = new ResLoader();
        public override void LoadAsync()
        {
            ResState = ResState.Loading;
            mResLoader.LoadAsync<AssetBundle>(mOwnerBundleName, (ownerBundle) =>
            {
                AssetBundleRequest assetBundleRequest = ownerBundle.LoadAssetAsync(Name);
                assetBundleRequest.completed += operation =>
                {
                    Asset = assetBundleRequest.asset;
                    ResState = ResState.loaded;
                };
            });
        }

        public override bool LoadSync()
        {
            ResState = ResState.Loading;
            var assetBundle =  mResLoader.LoadSync<AssetBundle>(mOwnerBundleName);
            Asset =  assetBundle.LoadAsset(Name);
            ResState = ResState.loaded;
            return Asset;
            
        }

        protected override void OnReleaseRes()
        {
            if(Asset is GameObject)
            {

            }
            else
            {
                Resources.UnloadAsset(Asset);
                Asset = null;
            }
            mResLoader.ReleaseAll();
            mResLoader = null;

            ResMgr.Instance.SharedLoadedReses.Remove(this);
        }
    }
}
