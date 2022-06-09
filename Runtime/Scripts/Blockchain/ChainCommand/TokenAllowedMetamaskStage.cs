using System.Collections;
using UnityEngine;

public class TokenAllowedMetamaskStage : Stage, IStageMetamaskConnection
{
	public TokenAllowedMetamaskStage(SCController controller, string loggingText) : base(controller)
	{
		this.controller = controller;
		this.LoggingText = loggingText;
	}

	public string LoggingText { get; private set; }
	private const int MIN_TOKEN_ALLOWED_VALUE = 10;

	protected override IEnumerator StartRequestCoroutine()
	{
		LoggingText = "${}";
		yield return controller.TokenAllowedAmount(HandleError);
		IsConditionMet = controller.Model.TokenAllowed.Value > MIN_TOKEN_ALLOWED_VALUE;
		Debug.Log($"Allowance: {Success}");
		if (!Success)
		{
			Debug.Log($"Approving...");
			yield return controller.TokenApprove(null);
			IsConditionMet = true;
		}
	}
}