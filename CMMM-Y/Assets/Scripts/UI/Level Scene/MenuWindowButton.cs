using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWindowButton : MonoBehaviour
{
    public GameObject window;
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
