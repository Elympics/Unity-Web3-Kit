using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElympicsRoomsAPI;
using System.Runtime.InteropServices;

public class GameMetaData
{
	public string RoomId { get; private set; } = null;

	private GameURLDataParser gameURLDataParser = null;

	public bool HasRoomId => RoomId != null;

#if UNITY_WEBGL
	[DllImport("__Internal")]
	private static extern void ChangeURL(string newURL);
#endif

	public GameMetaData()
	{
		gameURLDataParser = new GameURLDataParser();

		if (gameURLDataParser.HasRoomIdInURL())
			RoomId = gameURLDataParser.GetRoomIdFromURL();
	}

	public string AddRoomIdToUrl(string roomId)
	{
		//Validate roomId?

		RoomId = roomId;
		
		return gameURLDataParser.AddRoomIdToURL(roomId);
	}

	public void RemoveRoomIdFromURL()
	{
#if UNITY_WEBGL
		string urlWithoutRoomId = gameURLDataParser.GetURLWithoutRoomId();

		ChangeURL(urlWithoutRoomId);
#endif
	}
}