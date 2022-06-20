using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Web3Kit
{
	public class FirstTimeSetupLauncher
	{
		private const string SKIP_EDITOR_PREFS_KEY = "Web3KitSkipSetup";

		public static bool DidRunFirstTimeSetup() => AssetDatabase.LoadAssetAtPath(FirstTimeSetup.DestinationPath, typeof(Object)) != null;

		public static bool ShouldSkipFirstTimeSetup() => EditorPrefs.GetBool(SKIP_EDITOR_PREFS_KEY);

		public static void ToggleSkipFirstTimeSetup(bool value) => EditorPrefs.SetBool(SKIP_EDITOR_PREFS_KEY, value);

		[DidReloadScripts]
		private static void Launch()
		{
			if (!ShouldSkipFirstTimeSetup() && !DidRunFirstTimeSetup())
				FirstTimeSetup.Init();
		}
	}
}