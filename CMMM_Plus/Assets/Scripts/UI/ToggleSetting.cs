using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSetting : MonoBehaviour
{
    public bool defaultSetting;
    public string playerPrefString;
    Toggle m_Toggle;

    void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.isOn = PlayerPrefs.GetInt(playerPrefString, defaultSetting ? 1 : 0) == 1;
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
    }

    void ToggleValueChanged(Toggle change)
    {
        PlayerPrefs.SetInt(playerPrefString, m_Toggle.isOn ? 1 : 0);
    }
}
