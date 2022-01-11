using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("SeenHelpPanel", 0) == 1)
        {
            this.gameObject.SetActive(false);
        }
        else {
            PlayerPrefs.SetInt("SeenHelpPanel", 1);
            this.gameObject.SetActive(true);
        }
    }
}
