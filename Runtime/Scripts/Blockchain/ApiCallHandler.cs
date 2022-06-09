public class ApiCallHandler
{
	public delegate void SuccessCallback(string data);
	public delegate void ErrorCallback(string error);

	private readonly SuccessCallback successCallback;
	private readonly ErrorCallback errorCallback;

	public ApiCallHandler(SuccessCallback successCallback, ErrorCallback errorCallback)
	{
		this.successCallback = successCallback;
		this.errorCallback = errorCallback;
	}

	public void OnSuccess(string data) => successCallback?.Invoke(data);
	public void OnError(string error) => errorCallback?.Invoke(error);
}
