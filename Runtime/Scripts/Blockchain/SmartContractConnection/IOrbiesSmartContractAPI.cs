using System.Collections;

public interface IOrbiesSmartContractAPI
{
	void Init();
	IEnumerator CheckNetworkConnection(ApiCallHandler apiCallHandler);
	IEnumerator GetNickname(ApiCallHandler apiCallHandler);
	IEnumerator GetAllowedTokens(ApiCallHandler apiCallHandler);
	IEnumerator SetNickname(string nickname, ApiCallHandler apiCallHandler);
	IEnumerator ValidateBet(string betValue, ApiCallHandler apiCallHandler);
	IEnumerator EnterWithBet(string amount, ApiCallHandler apiCallHandler);
	IEnumerator GetPayout(string betAmount, ApiCallHandler apiCallHandler);
}


