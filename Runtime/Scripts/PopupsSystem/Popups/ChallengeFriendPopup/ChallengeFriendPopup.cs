using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaftPopups;
using ElympicsRoomsAPI;
using Zenject;

public class ChallengeFriendPopup : Popup
{
	[Header("References:")]
	[SerializeField] private SelectBetPhase selectBetPhase = null;
	[SerializeField] private WaitForOpponentPhase waitForOpponentPhase = null;

	private TicketPriceController mainTicketPriceController = null;

	[Inject]
	private void Inject(TicketPriceController mainTicketPriceController)
	{
		this.mainTicketPriceController = mainTicketPriceController;
	}

	public void InitializeAsHost()
	{
		selectBetPhase.StartPhase();
	}

	public void InitializeAsGuest(ElympicsRoomsAPIResponses.Room room)
	{
		selectBetPhase.DisplayPhase(false);

		waitForOpponentPhase.StartPhase(false, room);
	}

	public override void Hide()
	{
		base.Hide();

		mainTicketPriceController.Reset();

		if (waitForOpponentPhase.IsActive)
			waitForOpponentPhase.LeaveRoom();
	}
}
