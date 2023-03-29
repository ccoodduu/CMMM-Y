using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
	[Header("Options")]
	public bool disableInputs;
	public bool pauseSimulation;
	public bool disableInPlaymode;
	[Header("Keybinds")]
	public KeyCode openOnKey;
	public KeyCode closeOnKey;
	[Header("Tab Navigation")]
	public bool useTabNavigation;
	public Selectable[] tabNavigationCycle;
	[Header("Linked Popups")]
	public bool closeAllOnOpen;
	public List<PopupWindow> closeWindowsOnOpen = new List<PopupWindow>();
	[Space]
	public List<PopupWindow> closeWindowsOnClose = new List<PopupWindow>();
	[Space]
	public List<PopupWindow> disableOnPopups = new List<PopupWindow>();


	public bool IsOpen
	{
		get { return gameObject.activeSelf; }
	}

	public void Toggle()
	{
		if (IsOpen) Close();
		else Open();
	}

	public void CloseFromButton()
	{
		Close();
	}

	public void OpenFromButton()
	{
		Open();
	}

	public bool Open()
	{
		if (IsOpen) return false;
		if (disableInPlaymode && !GridManager.clean) return false;

		foreach (var popupWindow in disableOnPopups)
		{
			if (popupWindow.IsOpen) return false;
		}

		if (disableInputs) ControlsManager.disableInputs = true;
		if (pauseSimulation && !GridManager.clean) GameObject.FindWithTag("PlayButton").GetComponent<PlayButton>().Play(false);

		if (useTabNavigation)
		{
			var tabNavigation = EventSystem.current.gameObject.GetComponent<TabNavigation>();

			tabNavigation.doCycle = true;
			tabNavigation.cycleIndex = 0;
			tabNavigation.cycle = tabNavigationCycle;

			EventSystem.current.SetSelectedGameObject(null, new BaseEventData(EventSystem.current));
		}

		if (closeAllOnOpen)
		{
			foreach (var popupWindow in PopupManager.instance.popups)
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
		gameObject.SetActive(true);

		return true;
	}

	public bool Close()
	{
		if (!IsOpen) return false;

		foreach (var popupWindow in closeWindowsOnClose)
		{
			popupWindow.Close();
		}

		gameObject.SetActive(false);

		if (useTabNavigation)
		{
			var tabNavigation = EventSystem.current.gameObject.GetComponent<TabNavigation>();

			tabNavigation.doCycle = false;
		}

		foreach (var popupWindow in PopupManager.instance.popups)
		{
			if (popupWindow.IsOpen && popupWindow.disableInputs) return true;
		}

		ControlsManager.disableInputs = false;

		return true;
	}
}
