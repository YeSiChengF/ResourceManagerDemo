using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;

public class AssetBundleExample : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/AseetBundle/Build")]
    static void MenuItem()
    {
        if (! Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        //打包
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath,BuildAssetBundleOptions.None,BuildTarget.StandaloneOSX);
    }
    [MenuItem("Tools/AseetBundle/Run")]
    static void MenuItem2()
    {
        EditorApplication.isPlaying = true;
        new GameObject("test").AddComponent<AssetBundleExample>();
    }
#endif
    private AssetBundle _mBundle = null;
    void Start()
    {
        _mBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/prefabs/cube");
        GameObject go = _mBundle.LoadAsset<GameObject>("cube");
        Instantiate(go);
    }

    void OnDestroy()
    {
        _mBundle.Unload(true);
    }
}
