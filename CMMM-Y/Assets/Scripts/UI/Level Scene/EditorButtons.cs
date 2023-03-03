﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorButtons : MonoBehaviour
{
    public Tool_e tool;
    public bool animate;
    public KeyCode keybind;

    private void Update()
    {
        if (Input.GetKeyDown(keybind))
            switchTool();
    }

    public void switchTool() {
        GridManager.tool = tool;
        foreach (Transform button in PlacementManager.instance.buttons) {
            button.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        }
        this.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
}
