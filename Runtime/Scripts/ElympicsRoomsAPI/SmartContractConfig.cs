using UnityEngine;

[CreateAssetMenu(menuName = "Web3Kit/SmartContractConfig")]
public class SmartContractConfig : ScriptableObject
{
	public const string PATH_IN_RESOURCES = "Elympics/SmartContractConfig";

	public bool useSmartContract;
	public string smartContractAddress;
	public string chainAddress;
}
