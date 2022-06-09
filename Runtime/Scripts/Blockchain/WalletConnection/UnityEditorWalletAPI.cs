using System.Collections;
using UnityEngine;

public class UnityEditorWalletAPI : IWalletAPI
{
	public void ReloadPageOnAccountChange() { }

	public IEnumerator Connect(ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(1);
		apiCallHandler.OnSuccess(string.Empty);
	}

	public IEnumerator GetWalletAddress(ApiCallHandler apiCallHandler)
	{
		yield return new WaitForSeconds(1);
		// return a fake address but with same amount of bytes as a real address for testing
		apiCallHandler.OnSuccess("0x0000000000000000000000000000000000000000");
	}
}
