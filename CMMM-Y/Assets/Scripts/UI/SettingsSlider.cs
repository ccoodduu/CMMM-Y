using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
	public GameObject sliderValueDisplay;
	public string playerPrefString;
	public string sliderType;

	private TMP_Text valueText;

	void Start()
	{
		valueText = sliderValueDisplay.GetComponent<TMP_Text>();
		var value = PlayerPrefs.GetFloat(playerPrefString);

		GetComponent<Slider>().value = value;
		UpdateSlider(value);
	}

	public void UpdateSlider(float val)
	{
		PlayerPrefs.SetFloat(playerPrefString, val);

		MusicManager.instance.UpdateVolume();

		if (sliderType == "Volume") valueText.text = Mathf.Round(val * 100f) + "%";
		if (sliderType == "Speed") valueText.text = Mathf.Round(val * 10f) / 10f + "x";
	}
}
