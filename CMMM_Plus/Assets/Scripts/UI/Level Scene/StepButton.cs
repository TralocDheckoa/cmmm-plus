using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepButton : MonoBehaviour
{
    public Image pauseImage;
    public Sprite pauseSprite;
    public GameObject resetButton;

    public void wasClicked() {
        if (GridManager.playSimulation)
        {
            GridManager.playSimulation = false;
        }
        else
        {
            GridManager.stepSimulation = true;
            resetButton.SetActive(true);
        }
        pauseImage.sprite = pauseSprite;
    }
}
