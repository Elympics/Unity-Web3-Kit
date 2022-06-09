using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ElympicsRoomsAPI;
using DaftPopups;
using Elympics;
using System.Linq;

public class GameInvitationController : MonoBehaviour
{
	[Header("Popups data:")]
	[SerializeField] private ErrorPopupData roomDoesntExistData = null;
	[SerializeField] private ErrorPopupData roomClosedData = null;

	private PopupsManager popupsManager = null;
	private GameMetaData gameMetaData = null;

	[Inject]
	private void Inject(PopupsManager popupsManager, GameMetaData gameMetaData)
	{
		this.popupsManager = popupsManager;
		this.gameMetaData = gameMetaData;
	}

	private void Start()
	{
		if (ElympicsRoomsAPIController.Instance.IsReady)
		{
			CheckIfURLHasRoomKey();
		}
		else
		{
			ElympicsRoomsAPIController.Instance.IsReadyChanged += CheckRoomInvitation;
		}
	}

	private void CheckRoomInvitation(bool isReady)
	{
		if (isReady)
			CheckIfURLHasRoomKey();
	}

	private void CheckIfURLHasRoomKey()
	{
		if (gameMetaData.HasRoomId)
		{
			ProcessRoomKeyFromURL();

			gameMetaData.RemoveRoomIdFromURL();
		}
	}

	private void ProcessRoomKeyFromURL()
	{
		string roomKey = gameMetaData.RoomId;

		ElympicsRoomsAPIController.Instance.GetRoomInfo(roomKey, ProcessRoomIfExists);
	}

	private void ProcessRoomIfExists(ElympicsRoomsAPIResponses.Room room)
	{
		if (room != null)
		{
			if (room.players.Any(x => x.is_you))
			{
				OnRoomJoined(room);
			}
			else
			{
				//TODO: Display popup here or someting, that given room is full?
				if ((room.players != null && room.players.Length >= 2) || room.state == ElympicsRoomsAPIResponses.RoomState.closed)
				{
					popupsManager.ShowPopupInfo<ErrorPopup>(null, roomClosedData);
					Debug.Log("Room full / closed!");
					return;
				}

				JoinToGivenRoom(room);
			}
		}
		else
		{
			popupsManager.ShowPopupInfo<ErrorPopup>(null, roomDoesntExistData);
			Debug.Log("Given room in url doesn't exists!");
		}
	}

	private void JoinToGivenRoom(ElympicsRoomsAPIResponses.Room room)
	{
		ElympicsRoomsAPIController.Instance.JoinRoom(room.room_key, OnRoomJoined);
	}

	private void OnRoomJoined(ElympicsRoomsAPIResponses.Room room)
	{
		var challengeFriendPopup = popupsManager.ShowPopup<ChallengeFriendPopup>();
		challengeFriendPopup.InitializeAsGuest(room);
	}
}
