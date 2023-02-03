using UnityEngine;

namespace QFramework
{
	public class LoadDependencyAsyncExample : MonoBehaviour
	{

#if UNITY_EDITOR
		[UnityEditor.MenuItem("QFramework/Example/29.LoadDependencyAsyncExample", false, 29)]
		static void MenuItem()
		{
			UnityEditor.EditorApplication.isPlaying = true;

			new GameObject("LoadDependencyAsyncExample").AddComponent<LoadDependencyAsyncExample>();

		}
#endif
		
		ResLoader mResLoader = new ResLoader();
		// Use this for initialization
		void Start()
		{
			mResLoader.LoadAsync<AssetBundle>("gameobject", bundle =>
			{
				var gameObjPrefab = bundle.LoadAsset<GameObject>("GameObject");


				Instantiate(gameObjPrefab);
			});
		}

		private void OnDestroy()
		{
			mResLoader.ReleaseAll();
			mResLoader = null;
		}
	}
}