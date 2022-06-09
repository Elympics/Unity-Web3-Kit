using System.Collections;
using System.Globalization;
using UnityEngine;

public class UnityEditorSmartContract : IOrbiesSmartContractAPI
{
	public void Init()
	{
		Debug.Log("Pretending to initialize smart contract api");
	}

	public IEnumerator GetNickname(ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(0.2f);
		apiCallHandler.OnSuccess("dsdsds");
	}

	public IEnumerator SetNickname(string nickname, ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(5);
		apiCallHandler.OnSuccess(nickname);
	}

	public IEnumerator CheckNetworkConnection(ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(0.1f);
		apiCallHandler.OnSuccess(string.Empty);
	}

	public IEnumerator EnterWithBet(string amount, ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(5);
		apiCallHandler.OnSuccess($"{amount}");
	}

	public IEnumerator GetAllowedTokens(ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(5);
		apiCallHandler.OnSuccess(string.Empty);
	}

	public IEnumerator GetPayout(string betAmount, ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(0.5f);
		apiCallHandler.OnSuccess((int.Parse(betAmount) * 2 * 0.95f).ToString("0.00", CultureInfo.InvariantCulture));
	}

	public IEnumerator ValidateBet(string betValue, ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(0.5f);
		apiCallHandler.OnSuccess("true");
	}
}
