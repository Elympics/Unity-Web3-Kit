using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenBalanceStage : Stage, IStage
{
	private const int MIN_TOKEN_BALANCE = 10;

	public TokenBalanceStage(SCController controller) : base(controller)
	{
	}

	protected override IEnumerator StartRequestCoroutine()
	{
		yield return controller.GetTokenBalance(HandleError);
		IsConditionMet = controller.Model.TokenBalance.Value > MIN_TOKEN_BALANCE;
		Debug.Log($"Balance: {Success}");
	}
}
