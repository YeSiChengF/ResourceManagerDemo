using UnityEngine;
using ET;

namespace QFramework
{
	public class LoadABAssetExample : MonoBehaviour
	{

#if UNITY_EDITOR
		[UnityEditor.MenuItem("QFramework/Example/30.LoadABAssetExample", false, 30)]
		static void MenuClicked()
		{
			UnityEditor.EditorApplication.isPlaying = true;

			new GameObject("LoadABAssetExample")
				.AddComponent<LoadABAssetExample>();
		}
#endif

		private ResLoader mResLoader = new ResLoader();
		
		// Use this for initialization
		void Start()
		{
			var squareTexture = mResLoader.LoadSync<Texture2D>("square", "Square");
			Debug.Log(squareTexture.name);
			//TODO：异步加载有bug，加载不出来。检查一下
			mResLoader.LoadAsync<GameObject>("gameobject","GameObject",
				gameObjPrefab => { Instantiate(gameObjPrefab); });
		}

		private void OnDestroy()
		{
			mResLoader.ReleaseAll();
			mResLoader = null;
		}
	}
}