using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;

public class AssetBundleExample : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/AseetBundle���")]
    static void MenuItem()
    {
        if (! Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        //���
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath,BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows);
    }
#endif
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
