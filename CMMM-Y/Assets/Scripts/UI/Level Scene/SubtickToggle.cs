using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtickToggle : MonoBehaviour
{
    public void ToggleValueChanged(bool isOn)
    {
        GridManager.subTick = isOn;
    }
}
