using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletAddressStage : Stage, IStageMetamaskConnection
{
	public WalletAddressStage(SCController controller, string loggingText) : base(controller)
	{
		this.controller = controller;
		this.LoggingText = loggingText;
	}
	public string LoggingText { get; private set; }

	protected override IEnumerator StartRequestCoroutine()
	{
		yield return controller.GetWalletAddress(HandleError);
		IsConditionMet = true;
		LoggingText = "${}";
		Debug.Log($"Get Wallet Address: {Success}");
	}
}
