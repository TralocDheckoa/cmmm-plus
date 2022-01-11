using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationBackButton : MonoBehaviour
{
    public GameObject menu;
    public GameObject confirmation;
    public void Clicked() {
        menu.SetActive(false);
        confirmation.SetActive(false);  
    }
}
