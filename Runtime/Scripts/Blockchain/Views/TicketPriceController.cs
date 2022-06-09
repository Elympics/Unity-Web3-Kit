using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;
using UniRx;

public class TicketPriceController : MonoBehaviour
{
	[SerializeField] public List<PriceToggle> toggles = new List<PriceToggle>();
	[SerializeField] public PriceToggle defaultPrice = null;

	[SerializeField] private BetSlider priceSlider = null;
	[SerializeField] private TextMeshProUGUI enterFeeLabel = null;
	[SerializeField] private TextMeshProUGUI payoutLabel = null;
	private SCController controller;

	[SerializeField] private bool autoInitAbiConnectionOnPriceChanged = true;

	public PriceToggle CurrentSelectedPriceToggle { get; private set; }

	public event Action<int> SetButton;

	[Inject]
	private void Inject(SCController controller)
	{
		this.controller = controller;
		controller.Model.BetValue.Subscribe(_ => { HandleBetPricing(); });
		controller.Model.PayoutBalance.Subscribe(_ => { HandleBetPricing(); });
		priceSlider.sliderBetChangeValue += SetupToggles;
	}

	private void Start()
	{
		if (defaultPrice == null)
			Debug.LogError("Initial bet isn't set!");

		InitBetValue(defaultPrice);
	}

	private void InitBetValue(PriceToggle betVariant)
	{
		CurrentSelectedPriceToggle = betVariant;
		StartCoroutine(controller.SetBetValue(betVariant.TicketPrice));
		enterFeeLabel.text = "...";
		payoutLabel.text = "...";
	}

	private void HandleBetPricing()
	{
		enterFeeLabel.text = controller.Model.BetValue.ToString();
		payoutLabel.text = controller.Model.PayoutBalance.ToString();
		enterFeeLabel.transform.DOShakeScale(0.1f, 0.2f);
		payoutLabel.transform.DOShakeScale(0.1f, 0.2f);
	}

	[ReferencedByUnity]
	public void SetupToggles(PriceToggle selectedToggle)
	{
		foreach (var toggle in toggles)
		{
			toggle.PriceToggleOff();
		}

		selectedToggle.PriceToggleOn();

		InitBetValue(selectedToggle);

		priceSlider.SetSlider(selectedToggle.SliderFillValue);

		SetButton?.Invoke(selectedToggle.TicketPrice);
	}

	[ReferencedByUnity]
	public void SetupToggles(float value)
	{
		PriceToggle selectedToggle = null;

		foreach (var toggle in toggles)
		{
			toggle.PriceToggleOff();

			if (Mathf.Approximately(value, toggle.SliderFillValue))
				selectedToggle = toggle;
		}
		selectedToggle.PriceToggleOn();

		InitBetValue(selectedToggle);

		SetButton?.Invoke(selectedToggle.TicketPrice);
	}

	public void Reset()
	{
		SetupToggles(defaultPrice);
	}

	private void OnDestroy()
	{
		controller.Model.BetValue.Dispose();
		controller.Model.PayoutBalance.Dispose();
	}
}
