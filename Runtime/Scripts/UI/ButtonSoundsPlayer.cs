using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class ButtonSoundsPlayer : MonoBehaviour, IPointerDownHandler
{ 
	public bool useCustomSounds;
	
	[SerializeField] private AudioClip interactableSound; 
	[SerializeField] private AudioClip nonInteractableSound;
	
	private Button button;
	private AudioSource audioSource;

	[Inject]
	public void Inject(IDefaultButtonSoundsProvider defaultButtonSoundsProvider)
	{
		button = GetComponent<Button>();
		audioSource = GetComponent<AudioSource>();
		if (!useCustomSounds)
			SetDefaultSounds(defaultButtonSoundsProvider);
	}

	protected virtual void SetDefaultSounds(IDefaultButtonSoundsProvider defaultButtonSoundsProvider)
	{
		interactableSound = defaultButtonSoundsProvider.InteractableSound;
		nonInteractableSound = defaultButtonSoundsProvider.NonInteractableSound;
	}

	private void PlaySounds()
	{
		if (button.interactable)
			audioSource.clip = interactableSound;
		else
			audioSource.clip = nonInteractableSound;

		audioSource.Play();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		PlaySounds();
	}
}
