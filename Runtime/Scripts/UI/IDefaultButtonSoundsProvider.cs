using UnityEngine;

public interface IDefaultButtonSoundsProvider
{
	AudioClip InteractableSound { get; }
	AudioClip NonInteractableSound { get; }
}