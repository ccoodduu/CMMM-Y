using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class ResizeLevelWindowButton : MonoBehaviour
{
	public GameObject window;

	private void Update()
	{
		if (ControlsManager.GetControl("ResizeLevel").GetDown())
		{
			Clicked();
		}
	}

	public void Clicked()
	{
		if (!GridManager.clean) return;
		
		window.SetActive(window.active);
	}
}
