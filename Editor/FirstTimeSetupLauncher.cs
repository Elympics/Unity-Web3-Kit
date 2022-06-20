using UnityEditor;
using UnityEditor.Callbacks;

namespace Web3Kit
{
	public class FirstTimeSetupLauncher
	{
		private const string SETUP_EDITOR_PREFS_KEY = "Web3KitFirstTimeSetup";
		private const string SKIP_EDITOR_PREFS_KEY = "Web3KitSkipSetup";

		public static bool DidRunFirstTimeSetup() => EditorPrefs.GetBool(SETUP_EDITOR_PREFS_KEY);

		public static void MarkFirstTimeSetupRun(bool value) => EditorPrefs.SetBool(SETUP_EDITOR_PREFS_KEY, value);

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