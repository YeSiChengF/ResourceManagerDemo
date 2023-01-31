using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace QFramework
{
	public class ResLoader
	{	
		public T LoadSync<T>(string assetName) where T : Object
		{
			var res = GetOrCreateRes(assetName);

			if (res != null)
			{
				return res.Asset as T;
			}
			
			// 真正加载资源
			res = CreateRes(assetName);

			res.LoadSync();
			
			return res.Asset as T;
		}

		public void LoadAsync<T>(string assetName, Action<T> onLoaded) where T : Object
		{
			// 查询当前的 资源记录
			var res = GetOrCreateRes(assetName);

			if (res != null)
			{
				onLoaded(res.Asset as T);
				
				return;
			}

			// 真正加载资源
			res = CreateRes(assetName);

			res.LoadAsync(loadedRes =>
			{
				onLoaded(loadedRes.Asset as T);
			});
		}


		public void ReleaseAll()
		{
			mResRecord.ForEach(loadedAsset => loadedAsset.Release());

			mResRecord.Clear();
		}
		
		
		#region private	
		private List<Res> mResRecord = new List<Res>();

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
		private Res CreateRes(string assetName)
		{
			Res res = null;

            if (assetName.StartsWith("resources://"))
			{
				//Resoureces时加前缀因为不太常用。性能比较好
				res = new ResourcesRes(assetName);
            }
            else
			{
				res = new AssetBundleRes(assetName);
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