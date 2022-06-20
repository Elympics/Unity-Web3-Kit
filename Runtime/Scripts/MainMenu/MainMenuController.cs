using UnityEngine;
using UnityEngine.UI;
using Elympics;
using Zenject;
using System.Linq;
using System.Collections.Generic;
using DaftPopups;
using TMPro;
using System.Text;
using System.Collections;
using UniRx;

public class MainMenuController : MonoBehaviour
{
	[SerializeField] private OrbsKingdomButton playButton = null;
	[SerializeField] private TextMeshProUGUI displayedPlayerName = null;
	[SerializeField] private ErrorPopupData authenticatorClientError = null;
	[SerializeField] private string matchmakingQueueBase = "Default";
	[SerializeField] private string matchmakingQueueFree = "Free";
	[SerializeField] private GameObject[] objectsToHideIfNotUsingBlockchain;

	private SCController controller;
	private ElympicsLobbyClient lobbyClient;
	private IScenesLoader scenesLoader;
	private PopupsManager popupsManager = null;
	private SmartContractConfig smartContractConfig;
	private GameLoadingScreenPopup popup;

	[Inject]
	private void Inject(IScenesLoader scenesLoader, PopupsManager popupsManager, SCController controller, SmartContractConfig smartContractConfig)
	{
		this.controller = controller;
		this.scenesLoader = scenesLoader;
		this.popupsManager = popupsManager;
		this.smartContractConfig = smartContractConfig;

		lobbyClient = FindObjectOfType<ElympicsLobbyClient>();
		lobbyClient.Authenticated += HandleAuthenticated;

		foreach (var element in objectsToHideIfNotUsingBlockchain)
			element.SetActive(smartContractConfig.useSmartContract);

		StartCoroutine(SetupNicknameView());
	}

	private IEnumerator SetupNicknameView()
	{
		var nicknameStage = new NicknameStage(controller);
		yield return nicknameStage.Start();
		if (nicknameStage.Success)
			HandlePlayerNameChanged(controller.Model.NickName.Value);
		else
			controller.UseGuestName();

		SubscribeNicknameView();
	}

	private void HandlePlayerNameChanged(string name)
	{
		displayedPlayerName.text = name;
	}

	private void SubscribeNicknameView()
	{
		controller.Model.NickName.Subscribe(newValue =>
		{
			HandlePlayerNameChanged(newValue);
			GameplayDataInput.Nickname = newValue;
		});
	}

	private void HandleAuthenticated(bool success, string userId, string jwtToken, string error)
	{
		if (!success)
		{
			authenticatorClientError.errorCode = error;
			popupsManager.ShowPopupInfo<ErrorPopup>(scenesLoader.LoadMainMenu, authenticatorClientError);
			Debug.LogError($"Authentication failed.\n {error}");
		}
	}

	[ReferencedByUnity]
	public void ShowNicknamePopup()
	{
		popupsManager.ShowPopup<NicknamePopup>().Show();
	}

	[ReferencedByUnity]
	public void PlayForFree()
	{
		GameplayDataInput.BetValue = 0;
		GameplayDataInput.PayoutValue = 0;
		LoadMultiplayerMode($"{matchmakingQueueBase}:{matchmakingQueueFree}");
	}

	public void LoadMultiplayerMode(string queueName = null, bool showPopup = true, string betResponse = null)
	{
		var byteArray = betResponse == null ? null : Encoding.ASCII.GetBytes(betResponse);

		string fullQueueName = !string.IsNullOrEmpty(queueName) ? queueName : null;

		lobbyClient.PlayOnline(null, byteArray, fullQueueName);

		if (showPopup)
			popupsManager.ShowPopup<GameLoadingScreenPopup>().SetLabelToMatchmaking().FadeIn();
	}

	public void LoadMultiplayerGame(string customQueueName = null)
	{
		StartCoroutine(MultiGame(customQueueName));
	}

	public void LoadMultiplayerGame()
	{
		StartCoroutine(MultiGame());
	}

	private IEnumerator MultiGame(string customQueueName = null)
	{
		popup = popupsManager.ShowPopup<GameLoadingScreenPopup>();
		popup.FadeIn();

		var bettingStages = new IStage[]
		{
			new TokenAllowedStage(controller, popup),
			new TokenBalanceStage(controller),
			new BetPlacedStage(controller)
		};

		yield return BettingStages(bettingStages);

		popup.SetLabelToMatchmaking();

		var queueName = string.IsNullOrEmpty(customQueueName) ? $"{matchmakingQueueBase}:{controller.Model.BetValue}" : customQueueName;

		LoadMultiplayerMode(queueName, false, controller.Model.BettingHash.Value);
	}

	private IEnumerator BettingStages(IStage[] bettingStages)
	{
		popup.SetLabelToMetamaskTermsAccept();
		foreach (var stage in bettingStages)
		{
			yield return stage.Start();
			if (!stage.Success)
			{
				popupsManager.ShowPopupInfo<ErrorPopup>(scenesLoader.LoadMainMenu, authenticatorClientError);
				yield break;
			}
		}
	}

	private void OnDestroy()
	{
		lobbyClient.Authenticated -= HandleAuthenticated;
	}
}
