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
	using System.Collections.Generic;

	[CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
	[InitializeOnLoad]
	public class CAutoInjectionEditor : Editor 
    {
		private const string _isPressedPlayButton = "isPressedPlayButton";
		private const string _isPlayWithInjected = "isPlayWithInjected";

		private static HashSet<int> _hashCodes = new HashSet<int>();

		static CAutoInjectionEditor()
		{
			EditorApplication.update += OnUpdateEditor;
			EditorApplication.playModeStateChanged += OnChangeStateEditor;
		}

		private static void OnChangeStateEditor(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingPlayMode)
				InjectFor_CurrentScene();
		}

		[MenuItem("CONTEXT/MonoBehaviour/Force auto inject this")]
        private static void ForceInject(MenuCommand cmd)
        {
			InjectFrom_NoneSerializedObject(cmd.context, true);
		}

		[DidReloadScripts]
		private static void OnReloadScripts()
		{
			InjectFor_CurrentScene();
		}

		private static void OnUpdateEditor()
		{
			if (EditorPrefs.GetBool(_isPlayWithInjected))
			{
				InjectFor_CurrentScene();

				EditorPrefs.SetBool(_isPlayWithInjected, false);
				EditorApplication.isPlaying = true;
			}

			if (EditorApplication.isCompiling)
			{
				if (EditorApplication.isPlayingOrWillChangePlaymode && EditorPrefs.GetBool(_isPressedPlayButton) == false)
				{
					EditorPrefs.SetBool(_isPressedPlayButton, true);

					CDebug.Log("<color=red><b>Editor compiling and change play mode is detected!\n",
					"After few seconds, the play mode is stopped and the auto injection is completed and then play mode again.</b></color>");
				}
			}
			else
			{
				if (EditorPrefs.GetBool(_isPressedPlayButton))
				{
					EditorApplication.isPlaying = false;

					EditorPrefs.SetBool(_isPressedPlayButton, false);
					EditorPrefs.SetBool(_isPlayWithInjected, true);
				}
			}
		}

		public static void InjectFor_CurrentScene()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode) return;

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
			if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode) return;

			Object target = serializedObject.targetObject;
			if (target == null) return;

			if (PrefabUtility.GetPrefabType(target) == PrefabType.Prefab) return;

			int hashCode = target.GetHashCode();
			if (forceInject == false && _hashCodes.Contains(hashCode)) return;

			_hashCodes.Add(hashCode);

			serializedObject.Update();
				CAutoInjector.Inject(serializedObject, target, forceInject);
			serializedObject.ApplyModifiedProperties();
		}

		private void OnEnable()
        {
			InjectFrom_SerializedObject(serializedObject, false);
		}
    }
}

#endif