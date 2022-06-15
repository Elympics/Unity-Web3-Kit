using UnityEngine;
using UnityEditor;
using System.IO;

namespace Web3Kit
{
	public class FirstTimeSetup
	{
		private const string DestinationPath = "Assets/Web3Kit";

		[MenuItem("Tools/Web3Kit/First time setup")]
		public static void Run()
		{
			var config = Resources.Load<FirstTimeSetupConfig>("FirstTimeSetupConfig");
			var path = AssetDatabase.GetAssetPath(config.FolderToCopy);
			if (!AssetDatabase.CopyAsset(path, DestinationPath))
				throw new IOException("[Web3Kit:FirstTimeSetup] Copying files failed!");

			AssetDatabase.ImportAsset(DestinationPath);
			var error = AssetDatabase.RenameAsset(DestinationPath + "/_Resources", "Resources");
			if (!string.IsNullOrEmpty(error))
			{
				Debug.LogError(error);
				throw new IOException("[Web3Kit:FirstTimeSetup] Renaming _Resources to Resources failed!");
			}
		}
	}
}