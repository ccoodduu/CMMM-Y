using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class FullscreenSetting : MonoBehaviour
{
    Toggle m_Toggle;

    Resolution largestRes;

    void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.isOn = Screen.fullScreen;

        Resolution[] resolutions = Screen.resolutions;
        largestRes = resolutions[resolutions.Length - 1];
    }

    public void ToggleValueChanged(bool value)
    {
        Screen.SetResolution(largestRes.width, largestRes.height, value);
    }
}
