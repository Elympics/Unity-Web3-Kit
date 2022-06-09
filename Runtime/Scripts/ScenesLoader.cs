using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ScenesLoader", menuName = "Speculos/ScenesLoader")]
public class ScenesLoader : ScriptableObject, IScenesLoader
{
	// can't use SceneAsset because it's only visible in UnityEditor
	[SerializeField] private int mainMenuSceneBuildIndex = 1;

	public void LoadMainMenu()
	{
		SceneManager.LoadScene(mainMenuSceneBuildIndex);
	}
}
