using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class ResizeLevelShortcut : MonoBehaviour
{
	public PopupWindow window;

	private void Update()
	{
		if (ControlsManager.GetControl("ResizeLevel").GetDown())
		{
			window.Open();
		}
	}
}
