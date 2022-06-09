using System.Collections;
using UnityEngine;

public abstract class Stage
{
	public bool Success => IsConditionMet && !IsError;

	public SCController controller;
	public bool IsError;
	public bool IsConditionMet;

	public IEnumerator Start()
	{
		IsConditionMet = false;
		IsError = false;
		yield return StartRequestCoroutine();
	}
	protected abstract IEnumerator StartRequestCoroutine();

	public Stage(SCController controller) => this.controller = controller;

	protected virtual void HandleError(string error) => IsError = true;
}