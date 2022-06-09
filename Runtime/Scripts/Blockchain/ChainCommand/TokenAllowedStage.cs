using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenAllowedStage : Stage, IStage
{
	private const int MIN_TOKEN_ALLOWED_VALUE = 10;
	private readonly GameLoadingScreenPopup popup;

	public TokenAllowedStage(SCController controller, GameLoadingScreenPopup popup) : base(controller)
	{
		this.popup = popup;
	}

	protected override IEnumerator StartRequestCoroutine()
	{
		popup.SetLabelToTokenApproval();
		yield return controller.TokenAllowedAmount(HandleError);
		UpdateContidionMet();
		Debug.Log($"Allowance: {Success}");
		if (!Success)
		{
			Debug.Log($"Approving...");
			yield return controller.TokenApprove(null);
			UpdateContidionMet();
		}
	}

	private void UpdateContidionMet()
	{
		IsConditionMet = controller.Model.TokenAllowed.Value > MIN_TOKEN_ALLOWED_VALUE;
	}
}
