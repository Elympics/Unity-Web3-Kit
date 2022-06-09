using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PriceToggle : MonoBehaviour
{
	[Header("Ticket Settings")]
	[SerializeField] private int ticketPrice = 0;
	[SerializeField][Range(0.0f, 1.0f)] private float sliderFillValue = 0.0f;

	[Header("Scene References")]
	[SerializeField] private Image targetImage = null;
	[SerializeField] private float offToggleScale = 0.8f;
	[SerializeField] private TextMeshProUGUI prizeView = null;

	public float SliderFillValue => sliderFillValue;
	public int TicketPrice => ticketPrice;

	public void PriceToggleOn()
	{
		ChangeBackgroundAlpha(1.0f);
		this.transform.DOScale(Vector3.one, 0.3f);
	}

	public void PriceToggleOff()
	{
		ChangeBackgroundAlpha(0.0f);
		this.transform.DOScale(Vector3.one * offToggleScale, 0.3f);
	}

	private void ChangeBackgroundAlpha(float value)
	{
		Color backgroundColor = targetImage.color;
		backgroundColor.a = value;
		targetImage.color = backgroundColor;
	}
}
