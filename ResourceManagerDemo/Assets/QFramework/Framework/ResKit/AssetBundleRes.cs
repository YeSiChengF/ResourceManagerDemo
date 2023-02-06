using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QFramework
{
    //AssetBundle的加载类
    public class AssetBundleRes : Res
    {
        //添加AssetBundleManifest的处理，方案1-初始化加载(后续好处理热更新)、方案2-第一次加载AssetBundle的时候加载
        //加载了依赖还需要处理依赖的加载，异步的回调会比较麻烦
        static AssetBundleManifest mManifest;
        public static AssetBundleManifest Manifest
        {
            //这里用了方案二的懒加载
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
            //加载AssetBundle前必须要先加载依赖文件
            //比如A包里的PrefabA文件依赖B包里的TexB，PrefabA从包里加载前B包必须加载完毕。

            //streamingAssetsPath末尾有斜杠所以+1
            //AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/"
            string[] dependencies =  Manifest.GetDirectDependencies(mAssetPath.Substring(Application.streamingAssetsPath.Length +1));
            foreach (var dependencyBundleName in dependencies)
            {
                mResLoader.LoadSync<AssetBundle>(Application.streamingAssetsPath + "/" + dependencyBundleName);
            }
            AssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + mAssetPath);
            ResState = ResState.loaded;
            return AssetBundle;
        }
        private void LoadDependencyBundlesAsync(Action onAllLoaded)
        {
            //string[] dependencies = Manifest.GetDirectDependencies(mAssetPath.Substring(Application.streamingAssetsPath.Length + 1));//streamingAssetsPath末尾有斜杠
            string[] dependencies = Manifest.GetDirectDependencies(mAssetPath);
            int loadedCount = 0;
            if (dependencies.Length == 0)
            {
                onAllLoaded();
            }
            foreach (var dependencyBundleName in dependencies)
            {
                mResLoader.LoadAsync<AssetBundle>(dependencyBundleName,
                    dependBundle =>
                    {
                        //通过计数判断是否加载完成
                        loadedCount++;
                        if (loadedCount == dependencies.Length)
                        {
                            //完成，走下面的逻辑
                            onAllLoaded();
                        }
                    });
            }
        }

        public override void LoadAsync()
        {
            ResState = ResState.Loading;

            LoadDependencyBundlesAsync(() =>
            {
                //依赖文件异步加载完成后走这里。
                var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + mAssetPath);
                assetBundleCreateRequest.completed += operation =>
                {
                    AssetBundle = assetBundleCreateRequest.assetBundle;
                    ResState = ResState.loaded;
                };
            });
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