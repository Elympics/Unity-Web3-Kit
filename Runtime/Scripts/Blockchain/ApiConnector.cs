using System.Collections;
using System.Runtime.InteropServices;

public abstract class ApiConnector
{
	private const string ERROR_PREFIX = "ERROR:";
	public const int UNKNOWN_ERROR_CODE = 1234567;

	[DllImport("__Internal")]
	protected static extern string GetResponseData(int ticket);

	[DllImport("__Internal")]
	protected static extern bool IsWaitingForResponse(int ticket);

	protected IEnumerator RequestCoroutine(ApiCallHandler apiCallHandler, int ticket)
	{
		while (IsWaitingForResponse(ticket))
			yield return null;

		var response = GetResponseData(ticket);
		if (IsError(response))
		{
			apiCallHandler?.OnError(GetErrorDescription(response));
		}
		else if (apiCallHandler != null)
		{
			UnityEngine.Debug.Log("Response from coroutine: " + response);
			apiCallHandler?.OnSuccess(response);
		}
	}

	protected virtual string GetErrorDescription(string response) => response;
	protected bool IsError(string response) => response.StartsWith(ERROR_PREFIX);
	protected string GetErrorCode(string error) => error.Substring(ERROR_PREFIX.Length);

	protected int GetErrorCodeAsInt(string error)
	{
		var codeString = GetErrorCode(error);
		if (int.TryParse(error, out int code))
			return code;
		return UNKNOWN_ERROR_CODE;
	}
}
