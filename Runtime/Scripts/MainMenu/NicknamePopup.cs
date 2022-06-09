using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using Zenject;
using DaftPopups;

public class NicknamePopup : Popup
{
	[SerializeField] private CanvasGroup focusCanvasGroup = null;
	[SerializeField] private float fadeTime = 0.1f;
	[SerializeField] private TMP_InputField nicknameInputField = null;
	[SerializeField] private WheelWaiter wheelWaiter = null;

	[SerializeField] private Button confirmNicknameButton = null;
	[SerializeField] private Button cancelNicknameButton = null;

	[Header("Keyboard controls:")]
	[SerializeField] private KeyCode confirmNicknameKey = KeyCode.KeypadEnter;
	[SerializeField] private KeyCode cancelNicknameKey = KeyCode.Escape;

	public bool allowCancel = true;
	private SCController controller;

	[Inject]
	private void Inject(SCController controller)
	{
		this.controller = controller;
	}

	public void FadeCanvasGroup(bool value) => focusCanvasGroup.DOFade(value ? 1 : 0, fadeTime).OnComplete(() =>
	{
		focusCanvasGroup.interactable = value;
		focusCanvasGroup.blocksRaycasts = value;

		nicknameInputField.interactable = value;
		if (value)
			nicknameInputField.ActivateInputField();
	});

	private void Update()
	{
		if (Input.GetKeyDown(confirmNicknameKey))
		{
			confirmNicknameButton.onClick?.Invoke();
		}
		else if (Input.GetKeyDown(cancelNicknameKey))
		{
			cancelNicknameButton.onClick?.Invoke();
		}
	}

	public void SetNicknameButton()
	{
		StartCoroutine(SetNickname());
	}

	public override void Show()
	{
		base.Show();
		nicknameInputField.ActivateInputField();
	}

	private IEnumerator SetNickname()
	{
		nicknameInputField.interactable = false;
		confirmNicknameButton.interactable = false;

		wheelWaiter.SpiningWheelOn();

		yield return controller.SetNickname(nicknameInputField.text, null);

		wheelWaiter.SpiningWheelOff();

		FadeCanvasGroup(false);
		this.Hide();
	}
}
