using System.Collections;

public interface ITokenAPI
{
	void Init();
	IEnumerator TokenABITokenAproval(ApiCallHandler apiCallHandler);
	IEnumerator TokenABIIsAllowed(string walletAddress, ApiCallHandler apiCallHandler);
	IEnumerator TokenBalance(string walletAddress, ApiCallHandler apiCallHandler);
}