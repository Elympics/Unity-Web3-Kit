using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrbsKingdomButton : MonoBehaviour
{
	[Header("Main references:")]
	[SerializeField] private Button button = null;
	[SerializeField] private RectTransform buttonRootGraphic = null;
	[SerializeField] private Image buttonImage = null;
	[SerializeField] private TextMeshProUGUI buttonText = null;
	[SerializeField] private TextMeshProUGUI buttonInactiveDescription = null;
	[SerializeField] private PointerEventEmitter pointerEventEmmiter = null;

	[Header("Button inactive references:")]
	[SerializeField] private TMP_FontAsset buttonFontWhileInactive = null;
	[SerializeField] private Sprite buttonInactiveSprite = null;

	[Header("Button positions:")]
	[SerializeField] private Vector2 buttonPressedPositionModification = Vector2.zero;

	[Header("Colors:")]
	[SerializeField] private Color buttonPressedColor = Color.gray;
	[SerializeField] private Color fontColorWhenButtonInactive = Color.gray;

	[Header("While inactive settings:")]
	[SerializeField] private bool hasInactiveReasonDescription = false;
	[SerializeField] private string inactiveReasonDescription = null;

	private Vector2 buttonDefaultPosition = Vector2.zero;
	private Sprite buttonDefaultSprite = null;
	private TMP_FontAsset buttonDefaultFont = null;

	private Graphic[] graphicComponentsInButton = null;

	public bool Interactable
	{
		get
		{
			return button.interactable;
		}
		set
		{
			ChangeButtonInteractable(value);
		}
	}

	private void Awake()
	{
		buttonDefaultPosition = buttonRootGraphic.anchoredPosition;
		buttonDefaultSprite = buttonImage.sprite;
		buttonDefaultFont = buttonText.font;

		buttonInactiveDescription.text = inactiveReasonDescription;

		graphicComponentsInButton = buttonRootGraphic.GetComponentsInChildren<Graphic>();

		pointerEventEmmiter.PointerDown += ProcessPointerDown;
		pointerEventEmmiter.PointerUp += ProcessPointerUp;

		if (hasInactiveReasonDescription)
		{
			pointerEventEmmiter.PointerEnter += () => SetDescriptionView(true);
			pointerEventEmmiter.PointerExit += () => SetDescriptionView(false);
		}
	}

	private void ProcessPointerDown()
	{
		if (button.IsInteractable())
		{
			ChangeButtonPositionToPressed();
			ChangeColor(buttonPressedColor);
		}
	}

	private void ProcessPointerUp()
	{
		if (button.IsInteractable())
		{
			ChangeButtonPositionToDefault();
			ChangeColor(Color.white);
		}
	}

	private void ChangeButtonPositionToPressed()
	{
		buttonRootGraphic.anchoredPosition = buttonDefaultPosition + buttonPressedPositionModification;
	}

	private void ChangeButtonPositionToDefault()
	{
		buttonRootGraphic.anchoredPosition = buttonDefaultPosition;	
	}

	private void ChangeButtonInteractable(bool isInteractable)
	{
		button.interactable = isInteractable;
		ChangeColor(Color.white);

		if (isInteractable)
		{
			ChangeButtonPositionToDefault();
			buttonImage.sprite = buttonDefaultSprite;
			buttonText.font = buttonDefaultFont;
		}
		else
		{
			ChangeButtonPositionToPressed();
			buttonImage.sprite = buttonInactiveSprite;
			buttonText.font = buttonFontWhileInactive;
			ChangeButtonTextColor(fontColorWhenButtonInactive);
		}
	}

	private void SetDescriptionView(bool show)
	{
		show &= !button.interactable;

		buttonInactiveDescription.gameObject.SetActive(show);
	}

	private void ChangeColor(Color color)
	{
		foreach (Graphic graphicComponent in graphicComponentsInButton)
		{
			graphicComponent.color = color;
		}
	}

	private void ChangeButtonTextColor(Color color)
	{
		buttonText.color = color;
	}
}
