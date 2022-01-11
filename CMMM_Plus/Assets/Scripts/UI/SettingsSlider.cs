using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
    public GameObject sliderValueDisplay;
    public string playerPrefString;
    public string sliderType;

    void Start()
    {
        this.GetComponent<Slider>().value = PlayerPrefs.GetFloat(playerPrefString);
        if (sliderType == "Volume") sliderValueDisplay.GetComponent<Text>().text = Mathf.Round(PlayerPrefs.GetFloat(playerPrefString) * 100f) + "%";
        if (sliderType == "Speed") sliderValueDisplay.GetComponent<Text>().text = PlayerPrefs.GetFloat(playerPrefString) + "x";
    }

    public void updateSlider(float val)
    {
        PlayerPrefs.SetFloat(playerPrefString, val);
        if (sliderType == "Volume") sliderValueDisplay.GetComponent<Text>().text = Mathf.Round(val * 100f) + "%";
        if (sliderType == "Speed") sliderValueDisplay.GetComponent<Text>().text = val + "x";
    }
}
