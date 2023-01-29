using UnityEngine;

namespace QFramework
{
	public class UIXXXPanel : MonoBehaviour
	{

#if UNITY_EDITOR
		[UnityEditor.MenuItem("QFramework/Example/21.UIXXXPanel", false, 21)]
		static void MenuItem()
		{
			UnityEditor.EditorApplication.isPlaying = true;

			new GameObject("UIXXXPanel")
				.AddComponent<UIXXXPanel>()
				.gameObject.AddComponent<UIYYYPanel>();
		}
#endif

		ResLoader mResLoader = new ResLoader();

		private void Start()
		{
			var coinClip = mResLoader.LoadSync<AudioClip>("coin");

			var homeClip = mResLoader.LoadSync<AudioClip>("home");

			var bgClip = mResLoader.LoadSync<AudioClip>("coin");
			///

			OtherFunction();
		}


		private void OtherFunction()
		{
			var bgClip = mResLoader.LoadSync<AudioClip>("coin");
		}

		private void OnDestroy()
		{
			mResLoader.ReleaseAll();
		}
	}

	public class UIYYYPanel : MonoBehaviour
	{
		ResLoader mResLoader = new ResLoader();

		private void Start()
		{
			var coinClip = mResLoader.LoadSync<AudioClip>("coin");

			var homeClip = mResLoader.LoadSync<AudioClip>("home");

			var bgClip = mResLoader.LoadSync<AudioClip>("coin");
			///

			OtherFunction();
		}


		private void OtherFunction()
		{
			var bgClip = mResLoader.LoadSync<AudioClip>("coin");
		}

		private void OnDestroy()
		{
			mResLoader.ReleaseAll();
		}
	}
}