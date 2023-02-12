using System;
using System.IO;
using QFramework.Util;
using UnityEngine;
using UnityEditor;

namespace QFramework
{
    public class AssetBundleExporter : MonoBehaviour
    {
        //打包至当前平台
        [MenuItem("Tools/ResKit/Build AssetBundles")]
        static void BuildAssetBundles()
        {
            string outPutPath = ResKitPathUtil.FullPathForAssetBundles();
            if (!Directory.Exists(outPutPath))
            {
                Directory.CreateDirectory(outPutPath);
            }

            BuildPipeline.BuildAssetBundles(outPutPath, BuildAssetBundleOptions.ChunkBasedCompression,
                EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
        }
        
    }
}

