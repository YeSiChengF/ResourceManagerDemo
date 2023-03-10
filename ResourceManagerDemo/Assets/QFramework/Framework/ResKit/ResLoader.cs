using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace QFramework
{
    public class ResLoader
    {
        private List<Res> mResRecord = new List<Res>();

        public T LoadSync<T>(string assetBundleName,string assetName) where T : Object
        {
            return DoLoadSync<T>(assetName, assetBundleName);
        }

        public T LoadSync<T>(string assetName) where T : Object
        {
            return DoLoadSync<T>(assetName);
        }
        private T DoLoadSync<T>(string assetName,string assetBundleName = null) where T : Object
        {
            var res = GetResFromRecord(assetName);
            if (res != null)
            {
                if (res.ResState == ResState.Loading)
                {
                    throw new Exception(String.Format("不要异步加载资源{0}时，进行{1}的同步加载", res, assetName));
                }
                //ResState.loaded情况
                return res.Asset as T;
            }
            // 真正加载资源
            res = CreateRes(assetName, assetBundleName);
            res.LoadSync();
            return res.Asset as T;
        }

        public void LoadAsync<T>(string assetName, Action<T> onLoaded) where T : Object
        {
            DoLoadAsync<T>(assetName,null, onLoaded);
        }
        public void LoadAsync<T>(string assetBundleName,string assetName, Action<T> onLoaded) where T : Object
        {
            DoLoadAsync<T>(assetName, assetBundleName, onLoaded);
        }
        private void DoLoadAsync<T>(string assetName, string assetBundleName, Action<T> onLoaded) where T : Object
        {
            // 查询当前的 资源记录
            var res = GetResFromRecord(assetName);
            Action<Res> onResLoaded = null;
            onResLoaded = loadedRes =>
            {
                onLoaded(loadedRes.Asset as T);
                res.UnRegiesterOnLoadedEvent(onResLoaded);
            };
            if (res != null)
            {
                if (res.ResState == ResState.Loading)
                {
                    //需要等待
                    res.RegiesterOnLoadedEvent(onResLoaded);
                    return;
                }
                //ResState.loaded情况
                onLoaded(res.Asset as T);
                return;
            }
            // 真正加载资源
            res = CreateRes(assetName);
            res.RegiesterOnLoadedEvent(onResLoaded);
            res.LoadAsync();
        }


        public void ReleaseAll()
        {
            mResRecord.ForEach(loadedAsset => loadedAsset.Release());
            mResRecord.Clear();
        }
        
        
        #region private	

        private Res GetRes(string assetName)
        {
            // 查询当前的 资源记录
            var res = GetResFromRecord(assetName);
            return res;
        }
        private Res GetOrCreateRes(string assetName)
        {
            // 查询当前的 资源记录
            var res = GetResFromRecord(assetName);

            if (res != null)
            {
                return res;
            }

            // 查询全局资源池
            res = GetFromResMgr(assetName);

            if (res != null)
            {
                AddRes2Record(res);

                return res;
            }

            return res;
        }
        
        //这里有两个方案，方案一通过url解析来判断(也被称为路由机制)，方案二通过泛型来创建
        private Res CreateRes(string assetName,string ownerBundleName = null)
        {
            Res res = null;
            if (ownerBundleName != null)
            {
                res = new AssetRes(assetName, ownerBundleName);
            }
            else
            {
                if (assetName.StartsWith("resources://"))
                {
                    //Resoureces时加前缀因为不太常用。性能比较好
                    res = new ResourcesRes(assetName);
                }
                else
                {
                    res = new AssetBundleRes(assetName);
                }
            }
           
            ResMgr.Instance.SharedLoadedReses.Add(res);
            AddRes2Record(res);
            return res;
        }

        private Res GetResFromRecord(string assetName)
        {
            return mResRecord.Find(loadedAsset => loadedAsset.Name == assetName);
        }

        private Res GetFromResMgr(string assetName)
        {
            return ResMgr.Instance.SharedLoadedReses.Find(loadedAsset => loadedAsset.Name == assetName);
        }

        private void AddRes2Record(Res resFromResMgr)
        {
            mResRecord.Add(resFromResMgr);
                
            resFromResMgr.Retain();
        }
        
        #endregion
    }
}