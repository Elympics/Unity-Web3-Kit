using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using ElympicsRoomsAPI;

public class ChallengeFriendRoomGenerator : MonoBehaviour
{
	[SerializeField] private TMP_InputField createdRoomLinkDisplay = null; 

	private GameMetaData gameMetaData = null;

	private string linkToShare = null;

	private string createdRoomKey = null;
	public string CreatedRoomKey => createdRoomKey;

	[Inject]
	private void Inject(GameMetaData gameMetaData)
	{
		this.gameMetaData = gameMetaData;
	}

	private Action<ElympicsRoomsAPIResponses.Room> RoomCreatedCallback = null;

	public void CreateRoom(int bet, Action<ElympicsRoomsAPIResponses.Room> RoomCreated)
	{
		RoomCreatedCallback = RoomCreated;

		ElympicsRoomsAPIController.Instance.CreateChallengeFriendRoom(bet, OnRoomCreated);
	}

	private void OnRoomCreated(ElympicsRoomsAPIResponses.Room createdRoom)
	{
		createdRoomKey = createdRoom.room_key;

		CreateLinkForShare(createdRoom.room_key);
		UpdateLinkDisplay();
		RoomCreatedCallback(createdRoom);
	}

	private void UpdateLinkDisplay()
	{
		createdRoomLinkDisplay.text = linkToShare;
	}

	private void CreateLinkForShare(string roomId)
	{
		linkToShare = gameMetaData.AddRoomIdToUrl(roomId);
	}

	public void DisplayLinkToRoom(ElympicsRoomsAPIResponses.Room room)
	{
		createdRoomKey = room.room_key;

		CreateLinkForShare(room.room_key);
		UpdateLinkDisplay();
	}

	[ReferencedByUnity]
	public void CopyRoomLinkToClipboard()
	{
		GUIUtility.systemCopyBuffer = linkToShare;

#if UNITY_WEBGL
		WebGLCopyAndPasteAPI.CopyToClipboard(linkToShare);
#endif
	}
}
