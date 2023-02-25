using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAllControls : MonoBehaviour
{
    public Transform controlsContainer;
    public void ResetControls()
    {
        //ControlsManager.ResetAll();
        var controlOptions = controlsContainer.GetComponentsInChildren<ControlOption>();

        foreach (var controlOption in controlOptions)
        {
            controlOption.ResetControl();
        }
    }
}
