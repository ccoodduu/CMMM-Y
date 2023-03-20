using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationBackButton : MonoBehaviour
{
    public GameObject menu;
    public GameObject confirmation;

	private void Update()
	{
			if (confirmation.active && Input.GetKeyDown(KeyCode.Escape)) Clicked();
	}

	public void Clicked() {
        menu.SetActive(false);
        confirmation.SetActive(false);  
    }
}
