using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownSetting : MonoBehaviour
{
    public string playerPrefString;
    public int defaultValue = 0;

    void Start()
    {
        var dropdown = gameObject.GetComponent<TMP_Dropdown>();
        dropdown.value = PlayerPrefs.GetInt(playerPrefString, defaultValue);
    }

    public void DropdownValueChanged(int value)
    {
        PlayerPrefs.SetInt(playerPrefString, value);
    }
}
