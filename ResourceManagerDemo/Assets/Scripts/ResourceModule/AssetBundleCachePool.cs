using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AssetBundleCachePool
{
    public MonoBehaviour monoBehaviour;
    private static AssetBundleCachePool m_Instance;
    public static AssetBundleCachePool Instance
    {
        get { return m_Instance ?? (m_Instance = new AssetBundleCachePool()); }
    }
    private Dictionary<string, AssetBundleInfo> m_LoadAssetBundle = new Dictionary<string, AssetBundleInfo>();
    private Dictionary<string, Action<AssetBundle>> m_LoadingAssetBundle = new Dictionary<string, Action<AssetBundle>>();
    #region Load
    public AssetBundle LoadAssetsFromAB(string abName)
    {
        AssetBundleInfo assetBundleInfo = null;
        if (m_LoadAssetBundle.TryGetValue(abName, out assetBundleInfo))
        {   //检查是否加载过
            assetBundleInfo.ReferencedCount++;//这里可以使用封装好的引用计数，为了方便展示直接使用
        }
        else
        {
            string loadPath = PathUnit.DataPath + PathUnit.ABRootPath + abName;
            var ab = AssetBundle.LoadFromFile(loadPath);
            if (ab is null)
            {
                Debug.Log("AB包加载失败！"+ loadPath);
            }
            else
            {
                assetBundleInfo = new AssetBundleInfo(ab);
                m_LoadAssetBundle.Add(abName, assetBundleInfo);
            }
        }
        return assetBundleInfo.AssetBundle;
    }
    public void LoadAssetsFormAbAsync(string abName, Action<AssetBundle> loadingABAction)
    {
        monoBehaviour.StartCoroutine(OnLoadAssetsFormAbAsync(abName, loadingABAction));
    }
    IEnumerator OnLoadAssetsFormAbAsync(string abName, Action<AssetBundle> loadingABAction)
    {
        AssetBundleInfo assetBundleInfo = null;
        if (m_LoadAssetBundle.TryGetValue(abName,out assetBundleInfo))
        {
            Debug.Log("已加载过");
            AssetBundle assetBundle = assetBundleInfo.AssetBundle;
            //引用计数+1
            assetBundleInfo.ReferencedCount++;
            //触发回调
            if (loadingABAction != null)
            {
                loadingABAction.Invoke(assetBundle);
            }
            yield break;
        }
        else
        {
            if (m_LoadingAssetBundle.TryGetValue(abName,out Action<AssetBundle> temploadingABAction))
            {
                if (loadingABAction != null) { temploadingABAction += loadingABAction; }
                Debug.Log("正在被加载");
                yield break;
            }
            else
            {
                //这步需要执行，避免与其他逻辑冲突
                m_LoadingAssetBundle.Add(abName, loadingABAction);
                string loadPath = PathUnit.DataPath + PathUnit.ABRootPath + abName;
                AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(loadPath);
                AssetBundle resAB = assetBundleCreateRequest.assetBundle;
                if (resAB is null)
                {
                    Debug.LogError("AB包不能存在");
                }
                else
                {
                    assetBundleInfo = new AssetBundleInfo(resAB);
                    m_LoadAssetBundle.Add(abName, assetBundleInfo);
                }
                //加载完成移除加载中列表
                //避免啥异常状况，保险点！先判断是否存在
                if (m_LoadingAssetBundle.ContainsKey(abName))
                {
                    Action<AssetBundle> action = m_LoadingAssetBundle[abName];
                    m_LoadingAssetBundle.Remove(abName);
                    var callBackList =  action.GetInvocationList();
                    foreach (Action<AssetBundle> callBack in callBackList)
                    {
                        callBack.Invoke(resAB);
                    }
                }
            }
        }
    }
    #endregion
    #region UnLoad
    public void UnLoad()
    {

    }
    #endregion
}