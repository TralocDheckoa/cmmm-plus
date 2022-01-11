using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitOrSave : MonoBehaviour
{
    public GameObject confirmationScreen;


    public void Clicked() {
        if (GridManager.hasSaved || GridManager.mode != Mode_e.EDITOR)
        {
            SceneManager.LoadScene(0);
        }
        else {
            confirmationScreen.SetActive(true);
        }
    }
}
