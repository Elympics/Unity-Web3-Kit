using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameStage : Stage, IStage
{
	public NicknameStage(SCController controller) : base(controller)
	{
	}

	protected override IEnumerator StartRequestCoroutine()
	{
		yield return controller.GetNickname(HandleError);
		IsConditionMet = !string.IsNullOrEmpty(controller.Model.NickName.Value);
	}
}
