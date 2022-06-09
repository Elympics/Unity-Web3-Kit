using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetPlacedStage : Stage, IStage
{
	private int bet;
	protected override IEnumerator StartRequestCoroutine()
	{
		yield return controller.EnterWithBet(bet, HandleError);
		IsConditionMet = !string.IsNullOrEmpty(controller.Model.BettingHash.Value);
		Debug.Log($"Bet: {Success}");
	}

	public BetPlacedStage(SCController controller) : base(controller)
	{
		this.controller = controller;
		this.bet = controller.Model.BetValue.Value;
	}
}
