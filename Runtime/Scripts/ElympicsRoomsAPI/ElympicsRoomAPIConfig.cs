using UnityEngine;

[CreateAssetMenu(fileName = "ElympicsRoomAPIConfig", menuName = "Web3Kit/ElympicsRoomAPIConfig")]
public class ElympicsRoomAPIConfig : ScriptableObject
{
	public const string PATH_IN_RESOURCES = "Elympics/ElympicsRoomAPIConfig";

	public string Uri = "";
}
