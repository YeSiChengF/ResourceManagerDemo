using System.Linq;
using UnityEngine;

namespace QFramework
{
	public class AssetBundleManifestExample : MonoBehaviour
	{
#if UNITY_EDITOR
		[UnityEditor.MenuItem("QFramework/Example/25.AssetBundleManifestExample", false, 25)]
		static void MenuItem1()
		{
			var mainAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/StreamingAssets");

			var bundleManifest = mainAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

			bundleManifest.GetAllDependencies("gameobject")
				.ToList()
				.ForEach(dependency => { Debug.LogFormat("gameobject dependency:{0}", dependency); });

			bundleManifest.GetAllAssetBundles()
				.ToList()
				.ForEach(assetBundle => { Debug.Log(assetBundle); });


			bundleManifest.GetDirectDependencies("gameobject")
				.ToList()
				.ForEach(dependency => { Debug.LogFormat("gameobject dependency:{0}", dependency); });
			
			
			mainAssetBundle.Unload(true);
		}
#endif
	}
}