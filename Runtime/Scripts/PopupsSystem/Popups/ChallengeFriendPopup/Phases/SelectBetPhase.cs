using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBetPhase : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private WaitForOpponentPhase waitingForOpponentPhase = null;
	[SerializeField] private CanvasGroup canvasGroup = null;
	[SerializeField] private GameObject backgroundImages = null;

	public void StartPhase()
	{
		DisplayPhase(true);
	}

	[ReferencedByUnity]
	public void GoToNextPhase()
	{
		waitingForOpponentPhase.StartPhase(true);

		DisplayPhase(false);
	}

	public void DisplayPhase(bool value)
	{
		canvasGroup.alpha = value ? 1 : 0;
		canvasGroup.interactable = value;
		canvasGroup.blocksRaycasts = value;
		backgroundImages.SetActive(value);
	}
}
