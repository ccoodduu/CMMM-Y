using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlOption : MonoBehaviour
{
	public string control;
	public TMP_Text keyLabel;

	void Start()
	{
		SetLabel();
	}

	private void SetLabel()
	{
		var currentKey = ControlsManager.GetKeyForControl(control);
		keyLabel.text = ControlsManager.GetDisplayName(currentKey);
	} 

	public void ChangeKey()
	{
		keyLabel.text = "Press any key";
		StartCoroutine(WaitForKeypress());
	}

	IEnumerator WaitForKeypress()
	{
		while (true)
		{
			if (Input.anyKeyDown) break;
			yield return null;
		}

		var keyPressed = KeyCode.None;
		foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKey(kcode))
			{
				keyPressed = kcode;
				break;
			}
		}

		if (keyPressed is KeyCode.None)
		{
			SetLabel();
			StopCoroutine(WaitForKeypress());
			yield break;
		}

		if (keyPressed is KeyCode.Escape) keyPressed = KeyCode.None;

		ControlsManager.SetKeyForControl(control, keyPressed);
		SetLabel();
	}
}
