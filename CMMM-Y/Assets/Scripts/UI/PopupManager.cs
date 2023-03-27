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
			bool succeded = false;
			if (Input.GetKeyDown(popup.openOnKey)) 
			{ 
				succeded = popup.Open(); 
			}

			if (!succeded && Input.GetKeyDown(popup.closeOnKey)) popup.Close();
		}
    }
}
