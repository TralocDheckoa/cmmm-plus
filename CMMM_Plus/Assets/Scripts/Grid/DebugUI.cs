using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    float fps = 0;
    float mspt = 0;
    bool show = false;

    float smoothTime = 0;

    public Rect windowRect = new Rect(10, 100, 100, 120);

    public void Update()
    {
        smoothTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            print("A");
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
            windowRect.width = 120;
            windowRect.height = 100;
            windowRect = GUI.Window(0, windowRect, DoMyWindow, "Debug");
        }
    }

    private void DoMyWindow(int windowId)
    {
        GridManager.subTick = GUI.Toggle(new Rect(20, 100, 80, 20), GridManager.subTick, "ST");
        GUI.Label(new Rect(20, 20, 80, 20), "FPS " + Mathf.Floor(fps));
        GUI.Label(new Rect(20, 40, 80, 20), "MSPT " + Mathf.Floor(mspt) + "/" + GridManager.animationLength * 100);
        GUI.Label(new Rect(20, 60, 80, 20), "TPS " + 1000 / mspt + "/" + 1 / GridManager.animationLength);
        GridManager.animationLength = GUI.HorizontalSlider(new Rect(20, 80, 80, 20), GridManager.animationLength, .0f, 1);
        GUI.DragWindow(new Rect(0, 0, 100000, 100000));
    }
}
