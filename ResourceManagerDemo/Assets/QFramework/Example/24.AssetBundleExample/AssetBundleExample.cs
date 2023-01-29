using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QFramework
{
	public class AssetBundleExample : MonoBehaviour
	{

#if UNITY_EDITOR
		[UnityEditor.MenuItem("QFramework/Example/24.AssetBundleExample/Build AssetBundle", false, 24)]
		static void MenuItem1()
		{
			if (!Directory.Exists(Application.streamingAssetsPath))
			{
				Directory.CreateDirectory(Application.streamingAssetsPath);
			}

			UnityEditor.BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, UnityEditor.BuildAssetBundleOptions.None,
				UnityEditor.BuildTarget.StandaloneWindows);
		}
		
		[UnityEditor.MenuItem("QFramework/Example/24.AssetBundleExample/Run", false, 24)]
		static void MenuItem2()
		{
			UnityEditor.EditorApplication.isPlaying = true;

			new GameObject("AssetBundleExample").AddComponent<AssetBundleExample>();
		}
#endif

		private AssetBundle mBundle;
		
		// Use this for initialization
		void Start()
		{
			mBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/gameobject");

			var gameObj = mBundle.LoadAsset<GameObject>("GameObject");


			Instantiate(gameObj);

		}

		private void OnDestroy()
		{
			mBundle.Unload(true);
		}
	}
}