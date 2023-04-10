using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
	public TMP_Text sliderValueText;
	public string playerPrefString;
	public string sliderType;
	public float deafaultSliderValue;
	public AudioClip previewAudioClip;

	private AudioSource audioSource;
	private bool awake;

	public AudioClip previewAudioClip;

	private AudioSource audioSource;
	private bool awake;

	void Start()
	{
		var value = PlayerPrefs.GetFloat(playerPrefString, deafaultSliderValue);

		if (previewAudioClip != null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.loop = false;
			audioSource.playOnAwake = false;
			audioSource.volume = 0f;
			audioSource.clip = previewAudioClip;
		}

		GetComponent<Slider>().value = value;
		UpdateSlider(value);
		awake = true;
	}

	public void UpdateSlider(float val)
	{
		PlayerPrefs.SetFloat(playerPrefString, val);

		if (playerPrefString == "Music Volume") MusicManager.instance.UpdateVolume();

		if (sliderType == "Volume")
		{
			sliderValueText.text = Mathf.Round(val * 100f) + "%";

			if (awake && previewAudioClip != null) 
			{ 
				audioSource.volume = val; 
				if (!audioSource.isPlaying) audioSource.Play(); 
			}
		}
		if (sliderType == "Speed") sliderValueText.text = Mathf.Round(val * 10f) / 10f + "x";
	}
}
