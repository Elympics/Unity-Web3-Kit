using System.Collections;
using System.Runtime.InteropServices;

public class MetaMaskAPI : ApiConnector, IWalletAPI
{
	[DllImport("__Internal")]
	private static extern int MetaMaskConnect();

	[DllImport("__Internal")]
	private static extern int MetaMaskWalletId();

	[DllImport("__Internal")]
	private static extern void MetaMaskReloadPageOnAccountChange();

	// taken from https://docs.metamask.io/guide/ethereum-provider.html#errors
	protected override string GetErrorDescription(string error)
	{
		var errorCode = GetErrorCodeAsInt(error);
		switch (errorCode)
		{
			case 0:
				return null;
			case 4001:
				return "Rejected by user";
			case -32602:
				return "Parameters invalid";
			case -32603:
				return "Internal error";
			default:
				return "Unknown error";
		}
	}

	public void ReloadPageOnAccountChange() => MetaMaskReloadPageOnAccountChange();

	public IEnumerator Connect(ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, MetaMaskConnect());
	}

	public IEnumerator GetWalletAddress(ApiCallHandler apiCallHandler)
	{
		yield return RequestCoroutine(apiCallHandler, MetaMaskWalletId());
	}
}
