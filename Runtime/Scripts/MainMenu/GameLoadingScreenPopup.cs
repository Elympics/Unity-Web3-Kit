using UnityEngine;
using DG.Tweening;
using TMPro;
using DaftPopups;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class GameLoadingScreenPopup : Popup
{
	[SerializeField] private string tokenApproval = "Waiting for transaction confirmation...\nThis can take a while, depending on the network...";
	[SerializeField] private string tokenApprovalFooter = "This can take a while, depending on the network...";

	[SerializeField] private string matchmakingLabelText = "Searching for an opponent...";
	[SerializeField] private string trainingLabelText = "Loading...";
	[SerializeField] private string betLabelText = "Waiting for transaction approve...";
	[SerializeField] private string metamaskTermsAccept = "Waiting for metamask terms accept...";
	[SerializeField] private TextMeshProUGUI loadingLabel = null;
	[SerializeField] private TextMeshProUGUI footerLabel = null;
	[SerializeField] private Transform wheelTransform = null;
	[SerializeField] private float spinningSpeed = 1;
	[SerializeField] private float fadeTime = 0.5f;
	[SerializeField] private CanvasGroup canvasGroup = null;
	[SerializeField] private MatchmakingResponses matchmakingResponses = null;

	public GameLoadingScreenPopup SetLabelToMetamaskTermsAccept() => SetLabelText(metamaskTermsAccept);
	public GameLoadingScreenPopup SetLabelToTransactionApprove() => SetLabelText(betLabelText);
	public GameLoadingScreenPopup SetLabelToMatchmaking() => SetLabelText(matchmakingLabelText);
	public GameLoadingScreenPopup SetLabelToTokenApproval() => SetLabelText(tokenApproval, tokenApprovalFooter);
	public GameLoadingScreenPopup SetLabelToTraining() => SetLabelText(trainingLabelText);

	private GameLoadingScreenPopup SetLabelText(string text, string footerText = null)
	{
		loadingLabel.text = text;
		if (!string.IsNullOrEmpty(footerText))
			footerLabel.text = footerText;
		return this;
	}

	public void FadeIn(TweenCallback callback = null)
	{
		wheelTransform.DORotate(wheelTransform.transform.eulerAngles - Vector3.forward * 10, spinningSpeed).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
		if (canvasGroup == null)
			canvasGroup = GetComponent<CanvasGroup>();
		var tween = canvasGroup
			.DOFade(1, fadeTime)
			.SetEase(Ease.Linear);
		if (callback != null)
			tween.OnComplete(callback);
		canvasGroup.blocksRaycasts = true;

		matchmakingResponses.ClearMatchmakingPhaseDisplay();
	}
}
