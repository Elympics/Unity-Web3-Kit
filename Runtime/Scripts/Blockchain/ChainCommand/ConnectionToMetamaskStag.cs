using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionToMetamaskStage : Stage, IStageMetamaskConnection
{
	public string LoggingText { get; private set; }

	protected override IEnumerator StartRequestCoroutine()
	{
		yield return controller.ConnectToMetamask(HandleError);
		IsConditionMet = true;
		LoggingText = "${}";
		Debug.Log($"Connection: {Success}");
	}

	public ConnectionToMetamaskStage(SCController controller, string loggingText) : base(controller)
	{
		this.controller = controller;
		this.LoggingText = loggingText;
	}
}
