using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeFriendPlayerView : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private TextMeshProUGUI playersNickname = null;
	[SerializeField] private Image background = null;
	[SerializeField] private Sprite playerJoinedBackground = null;
	[SerializeField] private Sprite emptySlotBackground = null;

	[Header("Settings:")]
	[SerializeField] private string currentPlayerPostfix = " (you)";
	[SerializeField] private string undefinedPlayerNickname = "Your Opponent";
	[SerializeField] private Color currentPlayerColor = Color.white;
	[SerializeField] private Color opponentColor = Color.white;

	private void Awake()
	{
		playersNickname.text = undefinedPlayerNickname;
	}

	public void DisplayPlayerNickname(string nickname, bool isCurrentPlayer)
	{
		if (nickname != null)
		{
			string nicknameToDisplay = nickname;

			if (isCurrentPlayer)
				nicknameToDisplay += currentPlayerPostfix;

			playersNickname.text = nicknameToDisplay;

			background.sprite = playerJoinedBackground;
			background.color = isCurrentPlayer ? currentPlayerColor : opponentColor;
		}
		else
		{
			playersNickname.text = "";
			background.sprite = emptySlotBackground;
			background.color = Color.white;
		}
	}
}
