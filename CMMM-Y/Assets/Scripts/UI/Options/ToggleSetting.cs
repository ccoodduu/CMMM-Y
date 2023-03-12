using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSetting : MonoBehaviour
{
    public bool defaultSetting;
    public string playerPrefString;
    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = PlayerPrefs.GetInt(playerPrefString, defaultSetting ? 1 : 0) == 1;
    }

    public void ToggleValueChanged(bool value)
    {
        PlayerPrefs.SetInt(playerPrefString, value ? 1 : 0);
    }
}
