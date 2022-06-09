using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DaftPopups;
using ElympicsRoomsAPI;
using UnityEngine.UI;

public class ChallengeFriendButton : MonoBehaviour
{
	[SerializeField] private Button displayPopupButton = null;

	private PopupsManager popupsManager = null;

	[Inject]
	private void Inject(PopupsManager popupsManager)
	{
		this.popupsManager = popupsManager;
	}

	private void SetButtonBehaviour()
	{
		Debug.Log("Set display popup button to: " + ElympicsRoomsAPIController.Instance.IsReady);

		displayPopupButton.interactable = ElympicsRoomsAPIController.Instance.IsReady;

		if (!ElympicsRoomsAPIController.Instance.IsReady)
			ElympicsRoomsAPIController.Instance.IsReadyChanged += (bool isReady) => displayPopupButton.interactable = isReady;
	}

	[ReferencedByUnity]
	public void DisplayChallengeFriendPopup()
	{
		var challengeFriendPopup = popupsManager.ShowPopup<ChallengeFriendPopup>();
		challengeFriendPopup.InitializeAsHost();
	}
}
