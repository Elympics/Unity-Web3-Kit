using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BetSlider : MonoBehaviour, IPointerUpHandler
{
	[SerializeField] float sliderDoTweenTime = 0.1f;
	private Slider priceSlider = null;

	public event Action<float> sliderBetChangeValue;

	private void Awake()
	{
		priceSlider = GetComponent<Slider>();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		var setValue = 0f;
		if (priceSlider.value > 0.25f && priceSlider.value < 0.75f)
			setValue = 0.5f;
		else if (priceSlider.value < 0.25f)
			setValue = 0;
		else
			setValue = 1;

		SetSliderByTime(setValue);
		sliderBetChangeValue.Invoke(setValue);
	}

	internal void SetSlider(float sliderFillValue)
	{
		SetSliderByTime(sliderFillValue);
	}

	private void SetSliderByTime(float value) => DOTween.To(() => priceSlider.value, x => priceSlider.value = x, value, sliderDoTweenTime);
}
