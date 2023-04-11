using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
	public PopupWindow[] popups;

	public static PopupManager instance;
	void Start()
	{
		instance = this;
		popups = Resources.FindObjectsOfTypeAll<PopupWindow>();
	}

	void Update()
	{
		foreach (var popup in popups)
		{
			if (Input.GetKeyDown(popup.closeOnKey))
			{
				if (popup.Close()) return;
			}
		}

		foreach (var popup in popups)
        {
			if (Input.GetKeyDown(popup.openOnKey)) 
			{
				if (popup.Open()) return;
			}
		}
	}
}
