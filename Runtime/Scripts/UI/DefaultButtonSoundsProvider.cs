using UnityEngine;

public class DefaultButtonSoundsProvider : IDefaultButtonSoundsProvider
{
	private const string DEFAULT_SOUNDS_DIRECTORY = "DefaultSounds/";

	public AudioClip InteractableSound => Resources.Load<AudioClip>(DEFAULT_SOUNDS_DIRECTORY + "InteractableClick");
	public AudioClip NonInteractableSound => Resources.Load<AudioClip>(DEFAULT_SOUNDS_DIRECTORY + "NonInteractableClick");
}
