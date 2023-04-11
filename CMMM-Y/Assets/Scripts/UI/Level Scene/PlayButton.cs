using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
	public GameObject resetButton;
	public Sprite playSprite;
	public Sprite pauseSprite;

	void Update()
	{
		if (ControlsManager.GetControl("PlayPause").GetDown()) Play();
	}

	public void Play()
	{
		Play(!GridManager.playSimulation);
	}

	public void Play(bool play)
	{
		GridManager.playSimulation = play;

		if (play)
		{
			this.GetComponent<Image>().sprite = pauseSprite;
		}
		else
		{
			resetButton.SetActive(true);
			this.GetComponent<Image>().sprite = playSprite;
		}
	}
}
