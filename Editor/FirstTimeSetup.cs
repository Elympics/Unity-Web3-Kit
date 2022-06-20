using UnityEngine;
using UnityEditor;
using Elympics;

namespace Web3Kit
{
	public class FirstTimeSetup : EditorWindow
	{
		private const string DestinationPath = "Assets/Web3Kit";

		private ElympicsRoomAPIConfig roomApiConfig;
		private SmartContractConfig smartContractConfig;

		[MenuItem("Tools/Web3Kit/First time setup")]
		public static void Init()
		{
			var window = GetWindow<FirstTimeSetup>("Web3Kit Set Up");
			window.Show();
		}

		private void RunSetup()
		{
			bool success = true;
			var config = Resources.Load<FirstTimeSetupConfig>("FirstTimeSetupConfig");
			var path = AssetDatabase.GetAssetPath(config.FolderToCopy);
			if (!AssetDatabase.CopyAsset(path, DestinationPath))
			{
				Debug.LogError("[Web3Kit:FirstTimeSetup] Copying files failed!");
				success = false;
			}

			AssetDatabase.ImportAsset(DestinationPath);
			var error = AssetDatabase.RenameAsset(DestinationPath + "/_Resources", "Resources");
			if (!string.IsNullOrEmpty(error))
			{
				Debug.LogError(error);
				Debug.LogError("[Web3Kit:FirstTimeSetup] Renaming _Resources to Resources failed!");
				success = false;
			}

			ReassignReferences();

			FirstTimeSetupLauncher.MarkFirstTimeSetupRun(success);

			roomApiConfig = Resources.Load<ElympicsRoomAPIConfig>(ElympicsRoomAPIConfig.PATH_IN_RESOURCES);
			smartContractConfig = Resources.Load<SmartContractConfig>(SmartContractConfig.PATH_IN_RESOURCES);
		}

		[MenuItem("Tools/Web3Kit/Reassign references")]
		public static void ReassignReferences()
		{
			var projectInstaller = Resources.Load<ProjectInstaller>("ProjectContext");
			var newScenesLoader = AssetDatabase.LoadAssetAtPath<ScenesLoader>(DestinationPath + "/PopupsData/ScenesLoader.asset");
			projectInstaller.scenesLoader = newScenesLoader;
			EditorUtility.SetDirty(projectInstaller);
			AssetDatabase.SaveAssetIfDirty(projectInstaller);
		}

		private void OnGUI()
		{
			GUILayout.Label("Welcome to Elympics Web3Kit!", EditorStyles.largeLabel);

			if (!FirstTimeSetupLauncher.DidRunFirstTimeSetup())
			{
				GUILayout.Label("Run first time setup to copy and setup necessary files. This will only take a few seconds.", EditorStyles.wordWrappedLabel);
				if (GUILayout.Button("Start"))
					RunSetup();
				bool skip = GUILayout.Toggle(FirstTimeSetupLauncher.ShouldSkipFirstTimeSetup(), "Don't ask again");
				FirstTimeSetupLauncher.ToggleSkipFirstTimeSetup(skip);
			}
			else
			{
				GUILayout.Label("This setup script has copied files to " + DestinationPath + ". " +
					"Check contents of that folder and edit anything you like, but preserve the directory structure.", EditorStyles.wordWrappedLabel);
				GUILayout.Label("Let's run a quick setup, you can do this manually by editing scriptable objects in the Web3Kit folder. " +
					"If you don't know what to set for something, just leave it empty and come back later.", EditorStyles.wordWrappedLabel);

				roomApiConfig.Uri = EditorGUILayout.TextField("Elympics Room API uri", roomApiConfig.Uri);
				AssetDatabase.SaveAssetIfDirty(roomApiConfig);

				smartContractConfig.useSmartContract = EditorGUILayout.Toggle("Use blockchain integration?", smartContractConfig.useSmartContract);
				if (smartContractConfig.useSmartContract)
				{
					GUILayout.Label("New to web3? You can set up this section later and use your game in play for free mode.", EditorStyles.wordWrappedLabel);
					smartContractConfig.smartContractAddress = EditorGUILayout.TextField("Smart contract address", smartContractConfig.smartContractAddress);
					smartContractConfig.chainAddress = EditorGUILayout.TextField("Chain address", smartContractConfig.chainAddress);
				}
				AssetDatabase.SaveAssetIfDirty(smartContractConfig);

				GUILayout.Label("Next step: open your elympics config and create a new game by clicking the button below or using Tools/Elympics/Manage games in Elympics.", EditorStyles.wordWrappedLabel);
				if (GUILayout.Button("Manage games in Elympics"))
					OpenManageGamesInElympicsWindow();

				GUILayout.Label("Drag and drop TitleScreenController and MainMenuController from " + DestinationPath + "/Prefabs/ to your scenes " +
					"or start with " + DestinationPath + "/Scenes/. Remember to add them to build settings!", EditorStyles.wordWrappedLabel);
			}
		}

		// TODO: make OpenManageGamesInElympicsWindow public in Elympic SDK
		private static void OpenManageGamesInElympicsWindow()
		{
			var elympicsConfig = ElympicsConfig.Load();
			var serializedElympicsConfig = new SerializedObject(elympicsConfig);

			var elympicsApiEndpoint = serializedElympicsConfig.FindProperty("elympicsApiEndpoint");
			var elympicsLobbyEndpoint = serializedElympicsConfig.FindProperty("elympicsLobbyEndpoint");
			var elympicsGameServersEndpoint = serializedElympicsConfig.FindProperty("elympicsGameServersEndpoint");
			var currentGameIndex = serializedElympicsConfig.FindProperty("currentGame");
			var availableGames = serializedElympicsConfig.FindProperty("availableGames");

			ManageGamesInElympicsWindow.ShowWindow(serializedElympicsConfig, currentGameIndex, availableGames, elympicsApiEndpoint, elympicsLobbyEndpoint, elympicsGameServersEndpoint);
		}
	}
}