﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWindowButton : MonoBehaviour
{
    public GameObject window;

	public KeyCode closeOnKey;
	public KeyCode openOnKey;

	private void Update()
	{
		if (!window.active && Input.GetKeyDown(openOnKey)) Open();
		else if (window.active && Input.GetKeyDown(closeOnKey)) Close();
	}
	public void Open()
    {
        GridManager.playSimulation = false;
		window.SetActive(true);
    }

	public void Close()
	{
		window.SetActive(false);
	}
}
