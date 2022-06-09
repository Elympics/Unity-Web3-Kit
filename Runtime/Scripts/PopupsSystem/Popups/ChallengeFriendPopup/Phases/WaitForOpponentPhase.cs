using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using ElympicsRoomsAPI;
using UnityEngine.UI;

public class WaitForOpponentPhase : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private ChallengeFriendRoomGenerator challengeFriendRoomGenerator = null;
	[SerializeField] private ChallengeFriendPlayersDisplayController playersDisplayController = null;
	[SerializeField] private OrbsKingdomButton startMatchButton = null;
	[SerializeField] private TextMeshProUGUI startMatchButtonText = null;

	[Header("Settings:")]
	[SerializeField] private float playersRefreshTime = 1.0f;
	[SerializeField] private string startMatchButtonPrefixText = "PLAY";

	[Header("View references:")]
	[SerializeField] private TextMeshProUGUI betValueText = null;
	[SerializeField] private TextMeshProUGUI prizeValueText = null;
	[SerializeField] private CanvasGroup canvasGroup = null;

	public bool IsActive { get; private set; } = false;

	private ElympicsRoomsAPIResponses.Room room = null;
	private SCController controller;
	private TicketPriceController priceController;
	private MainMenuController mainMenuController;

	[Inject]
	private void Inject(TicketPriceController priceController, MainMenuController mainMenuController, SCController controller)
	{
		this.controller = controller;
		this.priceController = priceController;
		this.mainMenuController = mainMenuController;
	}

	public void StartPhase(bool isCreator, ElympicsRoomsAPIResponses.Room room = null)
	{

		if (room != null)
		{
			this.room = room;
			UpdateRoomView(room);

			challengeFriendRoomGenerator.DisplayLinkToRoom(room);

			controller.SetBetValue(room.bet.amount);
			controller.GetPayout(room.bet.amount, null);
		}

		if (isCreator)
		{
			challengeFriendRoomGenerator.CreateRoom(controller.Model.BetValue.Value, OnRoomCreated);
		}
		else
		{
			InvokeRepeating(nameof(RefreshPlayersInRoom), 0.0f, playersRefreshTime);
		}

		IsActive = true;
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	private void OnRoomCreated(ElympicsRoomsAPIResponses.Room room)
	{
		this.room = room;

		controller.SetBetValue(room.bet.amount);

		InvokeRepeating(nameof(RefreshPlayersInRoom), 0.0f, playersRefreshTime);
	}

	private void RefreshPlayersInRoom()
	{
		ElympicsRoomsAPIController.Instance.GetRoomInfo(room.room_key, UpdateRoomView);
	}

	private void UpdateRoomView(ElympicsRoomsAPIResponses.Room room)
	{
		if (room != null)
		{
			betValueText.text = room.bet.amount.ToString();
			prizeValueText.text = controller.Model.PayoutBalance.ToString();
			startMatchButtonText.text = $"{startMatchButtonPrefixText} ({room.bet.amount})";

			if (room.players != null && room.players.Length > 1)
			{
				// Enable match start, etc...
				startMatchButton.Interactable = true;
			}
			else
			{
				startMatchButton.Interactable = false;
			}

			playersDisplayController.UpdateRoomView(room);
		}
	}

	public void LeaveRoom()
	{
		priceController.SetupToggles(priceController.defaultPrice);
		ElympicsRoomsAPIController.Instance.LeaveRoom(room.room_key);
	}

	[ReferencedByUnity]
	public void StartGame()
	{
		string queueName = FixMatchmakerQueueNameFromAPI(room.matchmaker_queue_name);

		Debug.Log("Room queue name: " + queueName);

		//Call matchmaking directly:
		mainMenuController.LoadMultiplayerGame(queueName);

		Debug.Log($"Trying to set queue name: {queueName}");
	}

	//TODO: Totally bad thing, but first letter of default has to be upper case
	private string FixMatchmakerQueueNameFromAPI(string matchmakerQueueName)
	{
		return matchmakerQueueName[0].ToString().ToUpper() + matchmakerQueueName.Substring(1);
	}
}
