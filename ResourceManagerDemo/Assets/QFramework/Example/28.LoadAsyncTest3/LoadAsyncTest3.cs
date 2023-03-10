using UnityEngine;

namespace QFramework
{
	public class LoadAsyncTest3 : MonoBehaviour
	{
#if UNITY_EDITOR
		[UnityEditor.MenuItem("QFramework/Example/28.LoadAsyncTest3", false, 28)]
		static void MenuItem()
		{
			UnityEditor.EditorApplication.isPlaying = true;

			new GameObject("LoadAsyncTest3").AddComponent<LoadAsyncTest3>();

		}
#endif

		ResLoader mResLoader = new ResLoader();

		private void Start()
		{
			mResLoader.LoadAsync<Texture2D>("resources://BigTexture", bigTexture => { Debug.Log(bigTexture.name); });
			mResLoader.LoadAsync<Texture2D>("resources://BigTexture", bigTexture => { Debug.Log(bigTexture.name); });
		}
	}
}