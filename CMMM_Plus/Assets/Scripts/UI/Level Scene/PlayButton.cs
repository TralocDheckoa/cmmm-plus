using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public GameObject resetButton;
    public Sprite playSprite;
    public Sprite pauseSprite;

    public void Play() {
        if (GridManager.playSimulation)
        {
            GridManager.playSimulation = false;
            this.GetComponent<Image>().sprite = pauseSprite;
        }
        else {
            resetButton.SetActive(true);
            GridManager.playSimulation = true;
            this.GetComponent<Image>().sprite = playSprite;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Play();
        }
    }
}
