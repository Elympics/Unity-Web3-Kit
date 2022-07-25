using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using Zenject;

public class TitleScreenController : MonoBehaviour
{
	[SerializeField] private GameObject selectionWindow = null;
	[SerializeField] private GameObject connectingWindow = null;
	[SerializeField] private TextMeshProUGUI loadingText = null;
	[SerializeField] private GameObject metaMaskButton = null;
	[SerializeField] private GameObject playAsGuestButton = null;

	[Header("Loading texts")]
	[SerializeField] private string connectingProviderText = "Logging in...";
	[SerializeField] private string checkingNetworkText = "Checking network connection...";
	[SerializeField] private string fetchingWalletAddressText = "Fetching wallet address...";
	[SerializeField] private string startingGameText = "Starting game...";

	private SCController controller;
	private IOrbiesSmartContractAPI smartContractAPI;
	private ITokenAPI tokenAPI;
	private IWalletAPI walletApi;
	private IScenesLoader scenesLoader;
	private SmartContractConfig smartContractConfig;
	private GameObject[] allWindows;

	[Inject]
	public void Inject(IWalletAPI walletApi, IOrbiesSmartContractAPI smartContractAPI, ITokenAPI tokenAPI, IScenesLoader scenesLoader, SCController controller, SmartContractConfig smartContractConfig)
	{
		this.controller = controller;
		this.smartContractAPI = smartContractAPI;
		this.tokenAPI = tokenAPI;
		this.walletApi = walletApi;
		this.scenesLoader = scenesLoader;
		this.smartContractConfig = smartContractConfig;

		metaMaskButton.SetActive(smartContractConfig.useSmartContract);
		playAsGuestButton.SetActive(!smartContractConfig.useSmartContract);

		allWindows = new GameObject[] { selectionWindow, connectingWindow };
	}

	private void Start()
	{
#if ORBS_KINGDOM
		Debug.Log("Wallet connection skipped");
		StartGame();
#else
		ChangeWindowTo(selectionWindow);
#endif
	}

	[ReferencedByUnity]
	public void ConnectButton()
	{
		smartContractAPI.Init();
		tokenAPI.Init();
		walletApi.ReloadPageOnAccountChange();

		ChangeLoggingText(connectingProviderText);
		ChangeWindowTo(connectingWindow);

		StartCoroutine(ConnectionSequence());
	}

	[ReferencedByUnity]
	public void PlayAsGuest()
	{
		StartGame();
	}

	public IEnumerator ConnectionSequence()
	{
		var connectionStages = new IStageMetamaskConnection[]
		{
			new ConnectionToMetamaskStage(controller, connectingProviderText),
			new CheckConnectionStage(controller, checkingNetworkText),
			new WalletAddressStage(controller, fetchingWalletAddressText),
		};

		foreach (var stage in connectionStages)
		{
			ChangeLoggingText(stage.LoggingText);

			yield return stage.Start();

			if (!stage.Success)
			{
				HandleConnectionError("Error");
				yield break;
			}
		}

		ChangeLoggingText(startingGameText);
		StartGame();
	}

	private void HandleConnectionError(string error)
	{
		Debug.LogError(error);
		ChangeWindowTo(selectionWindow);
	}

	private void ChangeWindowTo(GameObject selectedWindow)
	{
		foreach (var window in allWindows)
			window.SetActive(window == selectedWindow);
	}

	private void StartGame()
	{
		scenesLoader.LoadMainMenu();
	}

	private void ChangeLoggingText(string text) => loadingText.text = text;
}
