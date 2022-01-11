using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//input validation for level width and height settings
//unity input field already has a setting to limit imputs to integers, but it also includes the '-' char

public class NoNegativity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        numberInput = GetComponent<InputField> ();
    }

    public InputField numberInput;

    public void ValueChanged(string txt)
    {
        
        if (txt == "-")
        {
            numberInput.text = "";
        }
    }
}
