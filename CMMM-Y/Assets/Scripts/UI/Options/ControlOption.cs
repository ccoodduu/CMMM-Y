using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlOption : MonoBehaviour
{
	public string controlName;
	public List<TMP_Text> keyLabels;

	public GameObject buttonPrefab;
	public GameObject plusPrefab;

	public Button resetButton;

	public Transform buttonContainer;

	private TabNavigationComponent tabNavigationComponent;

	void Start()
	{
		keyLabels = new List<TMP_Text>();
		tabNavigationComponent = gameObject.GetComponent<TabNavigationComponent>();
		SetLabels();
	}

	private void SetLabels()
	{
		var currentControl = ControlsManager.GetControl(controlName);
		for (int i = 0; i < currentControl.Keycodes.Length; i++)
		{
			SetLabel(i);
		}
	}
	private void SetLabel(int index)
	{
		if (keyLabels.Count <= index)
		{
			for (int i = 0; i < index + 1 - keyLabels.Count; i++)
			{
				AddKeyButton();
			}
			return;
		}
		var currentControl = ControlsManager.GetControl(controlName);
		keyLabels[index].text = ControlsManager.GetDisplayName(currentControl.Keycodes[index]);

		resetButton.interactable = !ControlsManager.IsDefault(controlName);
	}

	private void AddKeyButton()
	{
		int index = keyLabels.Count;

		if (keyLabels.Count != 0) Instantiate(plusPrefab, buttonContainer);

		var gameObject = Instantiate(buttonPrefab, buttonContainer);

		var button = gameObject.GetComponent<Button>();
		button.onClick.AddListener(delegate { ChangeKey(index); });
		tabNavigationComponent.AddStart(button);

		keyLabels.Add(gameObject.GetComponentInChildren<TMP_Text>());

		SetLabel(index);
	}

	public void InitButtons()
	{
		for (int i = buttonContainer.childCount - 1; i >= 0; i--)
		{
			var gameObject = buttonContainer.GetChild(i).gameObject;
			tabNavigationComponent.Remove(gameObject.GetComponent<Selectable>());

			Destroy(gameObject);
		}
		keyLabels.Clear();
		
		SetLabels();
	}

	public void AddKey()
	{
		ControlsManager.AddKeyToControl(controlName, KeyCode.None);
		InitButtons();
		ChangeKey(keyLabels.Count-1);
	}

	public void ResetControl()
	{
		ControlsManager.Reset(controlName);
		InitButtons();
	}

	public void ChangeKey(int index)
	{
		keyLabels[index].text = "Press any key";
		StartCoroutine(WaitForKeypress(index));
	}

	IEnumerator WaitForKeypress(int index)
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
			SetLabel(index);
			StopCoroutine(WaitForKeypress(index));
			yield break;
		}

		if (keyPressed is KeyCode.Escape) keyPressed = KeyCode.None;

		ControlsManager.SetKeyForControl(controlName, index, keyPressed);

		if (keyPressed is KeyCode.None && keyLabels.Count > 1) 
			InitButtons();
		else 
			SetLabel(index);
	}
}
