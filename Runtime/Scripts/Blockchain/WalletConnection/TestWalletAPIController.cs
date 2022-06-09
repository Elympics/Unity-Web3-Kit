using UnityEngine;
using TMPro;
using Zenject;

public class TestWalletAPIController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI walletId = null;

	private IWalletAPI walletAPI;
	private ApiCallHandler walletAddressHandler;

	[Inject]
	public void Inject(IWalletAPI walletAPI)
	{
		this.walletAPI = walletAPI;
		walletAddressHandler = new ApiCallHandler(DisplayWalletId, HandleError);
	}

	public void GetWalletId()
	{
		StartCoroutine(walletAPI.GetWalletAddress(walletAddressHandler));
	}

	private void DisplayWalletId(string walletId)
	{
		this.walletId.text = walletId;
	}

	private void HandleError(string errorMessage)
	{
		Debug.LogError(errorMessage);
	}
}
