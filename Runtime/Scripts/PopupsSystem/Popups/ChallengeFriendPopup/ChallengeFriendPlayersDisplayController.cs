using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ElympicsRoomsAPI;

public class ChallengeFriendPlayersDisplayController : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private ChallengeFriendPlayerView[] playersView = new ChallengeFriendPlayerView[2];

	public void PrepareHostView()
	{
		ElympicsRoomsAPIController.Instance.GetCurrentPlayer(DisplayHostOnly);
	}

	private void DisplayHostOnly(ElympicsRoomsAPIResponses.ElympicsPlayer host)
	{
		playersView[0].DisplayPlayerNickname(host.nickname, true);
	}

	public void UpdateRoomView(ElympicsRoomsAPIResponses.Room room)
	{
		for (int i = 0; i < playersView.Length; i++)
		{
			if (i < room.players.Length)
				playersView[i].DisplayPlayerNickname(room.players[i].nickname, room.players[i].is_you);
			else
				playersView[i].DisplayPlayerNickname(null, false);
		}
	}
}
