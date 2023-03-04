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

	void Start()
	{
		var value = PlayerPrefs.GetFloat(playerPrefString, deafaultSliderValue);

		GetComponent<Slider>().value = value;
		UpdateSlider(value);
	}

	public void UpdateSlider(float val)
	{
		PlayerPrefs.SetFloat(playerPrefString, val);

		if (playerPrefString == "Music Volume") MusicManager.instance.UpdateVolume();

		if (sliderType == "Volume") sliderValueText.text = Mathf.Round(val * 100f) + "%";
		if (sliderType == "Speed") sliderValueText.text = Mathf.Round(val * 10f) / 10f + "x";
	}
}
