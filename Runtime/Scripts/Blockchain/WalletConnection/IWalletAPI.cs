using System.Collections;

public interface IWalletAPI
{
	void ReloadPageOnAccountChange();
	IEnumerator Connect(ApiCallHandler apiCallHandler);
	IEnumerator GetWalletAddress(ApiCallHandler apiCallHandler);
}