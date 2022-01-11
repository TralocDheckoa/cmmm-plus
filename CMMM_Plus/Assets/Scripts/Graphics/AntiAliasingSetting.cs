using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class AntiAliasingSetting : MonoBehaviour
{

    private void Start()
    {
        GetComponent<UniversalAdditionalCameraData>().antialiasing = 2 - (AntialiasingMode)PlayerPrefs.GetInt("Anti Aliasing");

    }
}