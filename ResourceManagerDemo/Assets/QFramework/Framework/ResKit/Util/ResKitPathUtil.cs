using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SocialPlatforms;

namespace QFramework.Util
{
    public class ResKitPathUtil
    {
        public static string FullPathForAssetBundles(string assetBundleName = "")
        {
            string platformName = GetPlatformName();
            return Application.streamingAssetsPath + "/AssetBundles/" + platformName + "/" + assetBundleName;
        }
        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return _GetPlatformName(EditorUserBuildSettings.activeBuildTarget);
#else
            return _GetPlatformName(Application.platform);
#endif
        }
#if UNITY_EDITOR
        private static string _GetPlatformName(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.StandaloneLinux64:
                    return "Linux";
                case BuildTarget.StandaloneOSX:
                    return "OSX";
                case BuildTarget.WebGL:
                    return "WebGL";
                default:
                    return String.Empty;
            }
        }
#endif
        private static string _GetPlatformName(RuntimePlatform runtimePlatform)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                default:
                    return String.Empty;
            }
        }
    }
}