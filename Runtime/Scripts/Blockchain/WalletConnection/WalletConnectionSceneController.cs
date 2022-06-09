using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using Zenject;

public class WalletConnectionSceneController : MonoBehaviour
{
	[SerializeField] private GameObject selectionWindow = null;
	[SerializeField] private GameObject connectingWindow = null;
	[SerializeField] private TextMeshProUGUI loadingText = null;

	[Header("Loading texts")]
	[SerializeField] private string connectingMetamaskText = "Logging...";
	[SerializeField] private string networkCheckingText = "Network checking...";
	[SerializeField] private string walletAddressText = "Wallet address checking...";
	[SerializeField] private string matchStartedText = "Account has approve token, game starting...";

	private SCController controller;
	private IOrbiesSmartContractAPI smartContractAPI;
	private ITokenAPI tokenAPI;
	private IWalletAPI walletApi;
	private IScenesLoader scenesLoader;
	private GameObject[] allWindows;

	private const int minimalTokenAllowedValue = 10;

	[Inject]
	public void Inject(IWalletAPI walletApi, IOrbiesSmartContractAPI smartContractAPI, ITokenAPI tokenAPI, IScenesLoader scenesLoader, SCController controller)
	{
		this.controller = controller;
		this.smartContractAPI = smartContractAPI;
		this.tokenAPI = tokenAPI;
		this.walletApi = walletApi;
		this.scenesLoader = scenesLoader;

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

	public void ConnectButton()
	{
		smartContractAPI.Init();
		tokenAPI.Init();
		walletApi.ReloadPageOnAccountChange();

		ChangeLoggingText(connectingMetamaskText);
		ChangeWindowTo(connectingWindow);

		StartCoroutine(ConnectionSequence());
	}


	public IEnumerator ConnectionSequence()
	{
		var connectionStages = new IStageMetamaskConnection[]
		{
			new ConnectionToMetamaskStage(controller, connectingMetamaskText),
			new CheckConnectionStage(controller,networkCheckingText),
			new WalletAddressStage(controller,walletAddressText),
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

		ChangeLoggingText(matchStartedText);
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
