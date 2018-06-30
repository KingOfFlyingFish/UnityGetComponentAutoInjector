#region Header

/* ============================================
 *	작성자 : KJH
   ============================================ */

#endregion Header

#if UNITY_EDITOR

namespace UnityEditor
{
    using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEditor.Callbacks;

	[CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
	[InitializeOnLoad]
	public class CAutoInjectionEditor : Editor
    {
		private const string _isPressedPlayButton = "isPressedPlayButton";
		private const string _isPlayWithInjected = "isPlayWithInjected";
		private const string _isPlayWithInjectedTime = "_isPlayWithInjectedTime";
		private const float _delayedInjectionTime = 1.5f;

		static CAutoInjectionEditor()
		{
			EditorApplication.update += OnUpdateEditor;
		}

		[MenuItem("CONTEXT/MonoBehaviour/Force auto inject this")]
        private static void ForceInject(MenuCommand cmd)
        {
			if (EditorApplication.isPlaying) return;

			Object obj = cmd.context;

			bool isSupported = true;
			try
			{
				MonoBehaviour mono = (obj as MonoBehaviour);
			}
			catch
			{
				Debug.LogError("Only monobehaviour type supported");
				isSupported = false;
			}

			if (isSupported)
				InjectFrom_NoneSerializedObject(obj, true);
		}

		private static void OnUpdateEditor()
		{
			bool isPressedPlayButton = EditorPrefs.GetBool(_isPressedPlayButton);

			if (EditorApplication.isPlaying == false && EditorApplication.isCompiling && EditorApplication.isPlayingOrWillChangePlaymode)
			{ 
				if (isPressedPlayButton == false)
				{
					EditorPrefs.SetBool(_isPressedPlayButton, true);
					CDebug.Log("<color=red><b>Editor compiling and change play mode is detected!\n",
					"After ", _delayedInjectionTime, " seconds, the play mode is stopped and the auto injection is completed and then play mode again.</b></color>");
				}
			}

			bool isPlayWithInjected = EditorPrefs.GetBool(_isPlayWithInjected);
			if (isPlayWithInjected)
			{
				float time = EditorPrefs.GetFloat(_isPlayWithInjectedTime);
				if (time < EditorApplication.timeSinceStartup)
				{
					InjectFor_CurrentScene(); 

					EditorPrefs.SetBool(_isPlayWithInjected, false);
					EditorApplication.isPlaying = true;
				}
			}
		}

		[DidReloadScripts]
		private static void OnReloadScripts()
		{
			if (EditorApplication.isCompiling && EditorApplication.isPlaying)
				EditorApplication.isPlaying = false; 

			bool isPressedPlayButton = EditorPrefs.GetBool(_isPressedPlayButton);
			if (isPressedPlayButton)
			{
				EditorApplication.isPlaying = false;

				EditorPrefs.SetBool(_isPressedPlayButton, false);
				EditorPrefs.SetBool(_isPlayWithInjected, true);
				EditorPrefs.SetFloat(_isPlayWithInjectedTime, (float)EditorApplication.timeSinceStartup + _delayedInjectionTime);
			}
			else
				InjectFor_CurrentScene();
		}

		public static void InjectFor_CurrentScene()
		{ 
			Scene currentScene = SceneManager.GetActiveScene();
			 
			GameObject[] gameObjects = currentScene.GetRootGameObjects();
			if (gameObjects == null) return;

			int len = gameObjects.Length;
			for (int i = 0; i < len; i++)
			{
				GameObject gameObject = gameObjects[i];
				if (gameObject == null) continue;

				MonoBehaviour[] monos = gameObject.GetComponentsInChildren<MonoBehaviour>(true);

				int lenComponents = monos.Length;
				for (int j = 0; j < lenComponents; j++)
				{
					MonoBehaviour mono = monos[j];
					if (mono == null) continue;

					InjectFrom_NoneSerializedObject(mono, false);
				}
			}
		}

		public static void InjectFrom_NoneSerializedObject(Object obj, bool forceInject)
		{
			InjectFrom_SerializedObject(new SerializedObject(obj), forceInject);
		} 

		public static void InjectFrom_SerializedObject(SerializedObject serializedObject, bool forceInject)
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode) return;

			serializedObject.Update();
				CAutoInjector.Inject(serializedObject, forceInject);
			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
        {
			InjectFrom_SerializedObject(serializedObject, false);
		}
    }
}

#endif