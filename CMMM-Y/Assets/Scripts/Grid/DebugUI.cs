using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    float fps = 0;
    double mspt = 0;
    bool show = false;

    float smoothTime = 0;

    public Rect windowRect = new Rect(10, 100, 100, 120);

    public void Update()
    {
        smoothTime += Time.deltaTime;

        if (ControlsManager.GetControl("Debug").GetDown())
        {
            show = !show;
        }
    }

    private void OnGUI()
    {
        if (smoothTime > .25)
        {
            fps = (1 / Time.deltaTime);
            mspt = GridManager.MSPT;
            smoothTime = 0;
        }

        if (show)
        {
            windowRect.width = 140;
            windowRect.height = 100;
            windowRect = GUI.Window(0, windowRect, DoMyWindow, "Debug");
        }
    }

    private void DoMyWindow(int windowId)
    {
        GUI.Label(new Rect(20, 20, 100, 20), "FPS " + Mathf.Floor(fps));
        GUI.Label(new Rect(20, 40, 100, 20), "MSPT " + Math.Floor(mspt) + "/" + Mathf.Floor(1000 / (1 / GridManager.animationLength)));
        GUI.Label(new Rect(20, 60, 100, 20), "TPS " + Math.Floor(1000 / mspt) + "/" + Mathf.Floor(1 / GridManager.animationLength));
        GridManager.animationLength = GUI.HorizontalSlider(new Rect(20, 80, 100, 20), GridManager.animationLength, .0f, 1);
        GUI.DragWindow(new Rect(0, 0, 100000, 100000));
    }
}
