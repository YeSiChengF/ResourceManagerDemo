using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
	public class ResMgr : MonoSingleton<ResMgr>
	{
		public List<Res> SharedLoadedReses = new List<Res>();

#if UNITY_EDITOR
		private void OnGUI()
		{
			if (Input.GetKey(KeyCode.F1))
			{
				GUILayout.BeginVertical("box");

				SharedLoadedReses.ForEach(loadedRes =>
				{
					GUILayout.Label(string.Format("Name:{0} RefCount:{1}", loadedRes.Name, loadedRes.RefCount));
				});

				GUILayout.EndVertical();
			}
		}
#endif
	}
}