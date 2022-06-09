using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameURLDataParser
{
	private const int roomKeyLength = 8;

	public bool HasRoomIdInURL()
	{
		return Application.absoluteURL.Contains("?roomId");
	}

	public string GetRoomIdFromURL()
	{
		int roomKeyStartingPoint = Application.absoluteURL.IndexOf("?roomId") + roomKeyLength;

		return Application.absoluteURL.Substring(roomKeyStartingPoint, roomKeyLength);
	}

	public string AddRoomIdToURL(string roomId)
	{
		string urlWithRoomId = null;

		if (HasRoomIdInURL())
		{
			urlWithRoomId = Application.absoluteURL.Substring(0, Application.absoluteURL.IndexOf("?roomId")) + "?roomId=" + roomId;
		}
		else
		{
			urlWithRoomId = Application.absoluteURL + "?roomId=" + roomId;
		}

		return urlWithRoomId;
	}

	public string GetURLWithoutRoomId()
	{
		if (HasRoomIdInURL())
			return Application.absoluteURL.Substring(0, Application.absoluteURL.IndexOf("?roomId"));
		else
			return Application.absoluteURL;
	}
}
