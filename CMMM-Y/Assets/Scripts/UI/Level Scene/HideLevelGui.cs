using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLevelGui : MonoBehaviour
{
    public GameObject canvas;

    private bool visible = true;

    void Update()
    {
        if (ControlsManager.GetControl("HideUI").GetDown())
        {
            visible = !visible;
            canvas.SetActive(visible);
        }
    }
}
