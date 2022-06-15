using UnityEngine;

namespace Web3Kit
{
	public class FirstTimeSetupConfig : ScriptableObject
	{
		[SerializeField] private Object folderToCopy;

		public Object FolderToCopy => folderToCopy;
	}
}