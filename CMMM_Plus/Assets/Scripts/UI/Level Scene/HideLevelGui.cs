using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLevelGui : MonoBehaviour
{
    public GameObject go;

    [SerializeField]
    private KeyCode key = KeyCode.F1;

    private bool visible = true;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            visible = !visible;
            go.SetActive(visible);
        }
    }
}
