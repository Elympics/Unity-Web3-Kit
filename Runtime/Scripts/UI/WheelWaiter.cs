using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WheelWaiter : MonoBehaviour
{
	[SerializeField] private Image orbieWheelImage = null;
	[SerializeField] private float wheelSpinningTime = 1;
	[SerializeField] private float alfaMaxWheelColor = 0.25f;
	public void SpiningWheelOn()
	{
		orbieWheelImage.DOFade(alfaMaxWheelColor, wheelSpinningTime);
		orbieWheelImage.transform.DORotate(orbieWheelImage.transform.eulerAngles - Vector3.forward * 10, wheelSpinningTime).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
	}

	public void SpiningWheelOff()
	{
		DOTween.KillAll();
		orbieWheelImage.DOFade(0, wheelSpinningTime);
	}
}
