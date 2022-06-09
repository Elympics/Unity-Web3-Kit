using System.Collections;
using System.Runtime.InteropServices;

public class EthereumABIIntegration : ApiConnector, IOrbiesSmartContractAPI
{
	[DllImport("__Internal")]
	private static extern void ABIInit();

	[DllImport("__Internal")]
	private static extern int ABICheckNetworkConnection();

	[DllImport("__Internal")]
	private static extern int ABIGetNickname();

	[DllImport("__Internal")]
	private static extern int ABISetNickname(string nickname);

	[DllImport("__Internal")]
	private static extern int ABIEnterWithBet(string amount);

	[DllImport("__Internal")]
	private static extern int ABIBetValidate(string amount);

	[DllImport("__Internal")]
	private static extern int ABIGetPayout(string betAmount);

	[DllImport("__Internal")]
	private static extern int ABIGetAllowedTokens();

	public void Init() => ABIInit();

	public IEnumerator ValidateBet(string betValue, ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, ABIBetValidate(betValue));
	}

	public IEnumerator CheckNetworkConnection(ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, ABICheckNetworkConnection());
	}

	public IEnumerator GetNickname(ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, ABIGetNickname());
	}

	public IEnumerator GetAllowedTokens(ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, ABIGetAllowedTokens());
	}

	public IEnumerator SetNickname(string nickname, ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, ABISetNickname(nickname));
	}

	public IEnumerator EnterWithBet(string amount, ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, ABIEnterWithBet(amount));
	}

	public IEnumerator GetPayout(string betAmount, ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, ABIGetPayout(betAmount));
	}
}
