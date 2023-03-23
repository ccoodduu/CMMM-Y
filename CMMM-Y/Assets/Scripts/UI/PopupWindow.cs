using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
	public bool disableInputs;
	public bool closeAllOnOpen;
	public List<PopupWindow> closeWindowsOnOpen = new List<PopupWindow>();
	public List<PopupWindow> closeWindowsOnClose = new List<PopupWindow>();

	public bool IsOpen
	{
		get { return gameObject.activeSelf; }
	}

	public void Open()
	{
		if (disableInputs) ControlsManager.disableInputs = true;

		if (closeAllOnOpen)
		{
			foreach (var popupWindow in Resources.FindObjectsOfTypeAll<PopupWindow>())
			{
				popupWindow.Close();
			}
		}
		else
		{
			foreach (var popupWindow in closeWindowsOnOpen)
			{
				popupWindow.Close();
			}
		}
		gameObject.SetActive(false);
	}

	public void Close()
	{
		foreach (var popupWindow in closeWindowsOnClose)
		{
			popupWindow.Close();
		}

		gameObject.SetActive(false);

		foreach (var popupWindow in Resources.FindObjectsOfTypeAll<PopupWindow>())
		{
			if (popupWindow.IsOpen && popupWindow.disableInputs) return;
		}

		ControlsManager.disableInputs = false;
	}
}
