using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEditorTokenConnect : ITokenAPI
{
	public IEnumerator TokenABIIsAllowed(string walletAddress, ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(5);
		Debug.Log("Wallet Address: " + walletAddress);
		apiCallHandler.OnSuccess("15");
	}

	public IEnumerator TokenABITokenAproval(ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(5);
		Debug.Log("Approve: Success");
		apiCallHandler.OnSuccess("100");

	}

	public void Init()
	{
		Debug.Log("Pretending to initialize token api");
	}

	public IEnumerator TokenBalance(string walletAddress, ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(1);
		apiCallHandler.OnSuccess("11");
	}
}
