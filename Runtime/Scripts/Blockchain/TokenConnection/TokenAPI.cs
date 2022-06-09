using System.Collections;
using System.Runtime.InteropServices;

public class TokenAPI : ApiConnector, ITokenAPI
{
	[DllImport("__Internal")]
	private static extern void TokenABIInit();

	[DllImport("__Internal")]
	private static extern int TokenABIApprove();

	[DllImport("__Internal")]
	private static extern int TokenABIIsAllowed(string walletAddress);

	[DllImport("__Internal")]
	private static extern int TokenABIBalance(string walletAddress);

	public void Init() => TokenABIInit();

	public IEnumerator TokenABITokenAproval(ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, TokenABIApprove());
	}

	public IEnumerator TokenABIIsAllowed(string walletAddress, ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, TokenABIIsAllowed(walletAddress));
	}

	public IEnumerator TokenBalance(string walletAddress, ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, TokenABIBalance(walletAddress));
	}
}