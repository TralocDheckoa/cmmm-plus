using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenSetting : MonoBehaviour
{
    Toggle m_Toggle;

    Resolution[] resolutions;
    Resolution largestRes;

    void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.isOn = Screen.fullScreen;
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });

        Resolution[] resolutions = Screen.resolutions;
        largestRes = resolutions[resolutions.Length - 1];
    }

    void ToggleValueChanged(Toggle change)
    {
        Screen.SetResolution(largestRes.width, largestRes.height, m_Toggle.isOn);
    }
}
