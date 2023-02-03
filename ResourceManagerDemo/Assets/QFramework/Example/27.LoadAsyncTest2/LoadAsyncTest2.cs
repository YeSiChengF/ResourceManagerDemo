using System.Linq;
using UnityEngine;

namespace QFramework
{
	public class LoadAsyncTest2 : MonoBehaviour
	{
#if UNITY_EDITOR
		[UnityEditor.MenuItem("QFramework/Example/27.LoadAsyncTest2", false, 27)]
		static void MenuItem()
		{
			UnityEditor.EditorApplication.isPlaying = true;

			new GameObject("LoadAsyncTest1").AddComponent<LoadAsyncTest2>();

		}
#endif

		ResLoader mResLoader = new ResLoader();

		private void Start()
		{
			mResLoader.LoadAsync<Texture2D>("resources://BigTexture", bigTexture => { Debug.Log(bigTexture.name); });
		}
	}
}