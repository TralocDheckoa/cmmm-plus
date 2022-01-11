using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownSetting : MonoBehaviour
{
    public string playerPrefString;
    public int defaultValue = 0;
    Dropdown dropdown;

    void Start()
    {
        dropdown = gameObject.GetComponent<Dropdown>();
        dropdown.value = PlayerPrefs.GetInt(playerPrefString, defaultValue);
        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(dropdown);
        });
    }

    void DropdownValueChanged(Dropdown change)
    {
        PlayerPrefs.SetInt(playerPrefString, change.value);
    }
}
