using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckConnectionStage : Stage, IStageMetamaskConnection
{
	public string LoggingText { get; private set; }

	protected override IEnumerator StartRequestCoroutine()
	{
		yield return controller.CheckConnection(HandleError);
		IsConditionMet = true;
		LoggingText = "${}";
		Debug.Log($"Checking Connection: {Success}");
	}

	public CheckConnectionStage(SCController controller, string loggingText) : base(controller)
	{
		this.controller = controller;
		this.LoggingText = loggingText;
	}
}
