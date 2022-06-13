using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Image targetImage = null;

	[SerializeField] private Sprite soundOnLoud = null;
	[SerializeField] private Sprite soundOn = null;
	[SerializeField] private Sprite soundOff = null;
	[SerializeField] private AudioMixerGroup mixerGroup = null;
	[SerializeField] private Slider slider = null;

	[Header("Parameters")]
	[SerializeField] private int mindB = -80;
	[SerializeField] private int maxdB = 20;
	[SerializeField] private AnimationCurve volumeScaleCurve = null;

	private const string MASTER_VOLUME = "MasterVolume";

	private float dBValueRange = 0;
	private float lastValue = 50;

	private void Awake()
	{
		if (mixerGroup == null)
			Debug.LogError("[Web3Kit] Unassigned reference to audio mixer group in AudioController.\nCreate a new AudioMixer and assign the reference.");

		dBValueRange = Mathf.Abs(mindB) + Mathf.Abs(maxdB);

		UpdateAudioDB(50);
	}

	public void UpdateAudioDB(float value0100)
	{
		value0100 = Mathf.Round(value0100);

		float newVolume = mindB + (dBValueRange * volumeScaleCurve.Evaluate(value0100 / 100.0f));
		mixerGroup?.audioMixer.SetFloat(MASTER_VOLUME, newVolume);
		SetSoundSprite(value0100);

		slider.value = value0100;
	}

	private void SetSoundSprite(float value0100)
	{
		if (value0100 > 70f)
			targetImage.sprite = soundOnLoud;
		else if (value0100 < 1.0f)
			targetImage.sprite = soundOff;
		else
			targetImage.sprite = soundOn;
	}

	public void ToggleAudio()
	{
		float currentVolume = 0;
		mixerGroup?.audioMixer.GetFloat(MASTER_VOLUME, out currentVolume);

		if (currentVolume == mindB)
		{
			UpdateAudioDB(lastValue);
		}
		else
		{
			lastValue = slider.value;
			UpdateAudioDB(0);
		}
	}
}