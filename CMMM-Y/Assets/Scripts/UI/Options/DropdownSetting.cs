using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownSetting : MonoBehaviour
{
	public string playerPrefString;
	public int defaultValue = 0;

	private TabNavigation tabNavigation;

	private TMP_Dropdown dropdown;
	private TabNavigationComponent tabNavigationComponent;

	private bool isOpen;


	void Start()
	{
		dropdown = gameObject.GetComponent<TMP_Dropdown>();
		dropdown.value = PlayerPrefs.GetInt(playerPrefString, defaultValue);

		tabNavigationComponent = gameObject.GetComponent<TabNavigationComponent>();
		tabNavigation = EventSystem.current.gameObject.GetComponent<TabNavigation>();
	}

	void Update()
	{
		if (dropdown.IsExpanded && !isOpen)
		{
			tabNavigation.cycle = dropdown.GetComponentsInChildren<Toggle>(false);
			tabNavigation.doCycle = true;
			isOpen = true;
		}

		if (!dropdown.IsExpanded && isOpen)
		{
			tabNavigation.doCycle = false;
			isOpen = false;
		}
	}

	public void DropdownValueChanged(int value)
	{
		PlayerPrefs.SetInt(playerPrefString, value);
	}
}