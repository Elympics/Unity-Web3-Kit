using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;
using MatchTcpClients.Synchronizer;
using System.Threading;
using TMPro;
using DG.Tweening;
using Zenject;
using DaftPopups;

public class MatchmakingResponses : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private GameLoadingScreenPopup gameLoadingScreen = null;

	[Header("Matchmaking process")]
	[SerializeField] private TextMeshProUGUI matchmakingPhaseText = null;

	[Header("Popup scriptables:")]
	[SerializeField] private ErrorPopupData opponentNotFound = null;

	private PopupsManager popupsManager = null;
	private ElympicsLobbyClient elympicsLobbyClient = null;

	private event Action ErrorPopupClosed = null;
	private bool errorDisplayed = false;

	[Inject]
	private void Inject(PopupsManager popupsManager, ElympicsLobbyClient elympicsLobbyClient)
	{
		this.popupsManager = popupsManager;
		this.elympicsLobbyClient = elympicsLobbyClient;
	}

	private void Awake()
	{
		ErrorPopupClosed += gameLoadingScreen.Hide;
		ErrorPopupClosed += ClearErrorPopupFlag;

		SetupEvents();
	}

	private void SetupEvents()
	{
		elympicsLobbyClient.Matchmaker.MatchmakingError += (string message) => DisplayErrorPopup();
		elympicsLobbyClient.Matchmaker.WaitingForMatchError += ((string gameId, string gameVersion, string message) error) => DisplayErrorPopup();

		elympicsLobbyClient.Matchmaker.MatchmakingStarted += () => UpdateMatchmakingPhaseText("If no match is found, the money will be returned within 15 minutes.");
		elympicsLobbyClient.Matchmaker.WaitingForMatchStarted += ((string gameId, string gameVersion) a) => UpdateMatchmakingPhaseText("Match found! Assigning a server nearby...");
		elympicsLobbyClient.Matchmaker.WaitingForMatchStateInitializingStartedWithMatchId += (string message) => UpdateMatchmakingPhaseText("Everything is set! Starting the match...");
		elympicsLobbyClient.Matchmaker.WaitingForMatchStateRunningStartedWithMatchId += (string message) => UpdateMatchmakingPhaseText("Everything is set! Starting the match...");
		elympicsLobbyClient.Matchmaker.WaitingForMatchStateRunningError += ((string gameId, string gameVersion) a) => DisplayErrorPopup();
	}

	private void DisplayErrorPopup()
	{
		if (errorDisplayed)
			return;

		popupsManager.ShowPopupInfo<ErrorPopup>(ErrorPopupClosed, opponentNotFound);
		errorDisplayed = true;
	}

	private void ClearErrorPopupFlag()
	{
		errorDisplayed = false;
	}

	public void ClearMatchmakingPhaseDisplay()
	{
		matchmakingPhaseText.text = "";
	}

	private void UpdateMatchmakingPhaseText(string message)
	{
		matchmakingPhaseText.text = message;
	}

	//TODO: Remove this hack after feature implemented in elympics sdk
	private string ProcessErrorEventMessage(string message)
	{
		string errorMessage = message;

		string headerForErrorMessage = "ErrorMessage\":\"";
		int indexWhereMessageBegins = message.IndexOf(headerForErrorMessage);

		if (indexWhereMessageBegins != -1)
		{
			errorMessage = message.Substring(indexWhereMessageBegins + headerForErrorMessage.Length);

			int indexWhereMessageEnds = errorMessage.IndexOf("\"");

			errorMessage = errorMessage.Substring(0, indexWhereMessageEnds);
		}

		return errorMessage;
	}
}
