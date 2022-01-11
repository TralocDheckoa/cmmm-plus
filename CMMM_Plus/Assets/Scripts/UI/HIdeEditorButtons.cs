using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIdeEditorButtons : MonoBehaviour
{
    public void Start()
    {
        if (GridManager.mode != Mode_e.EDITOR)
            this.gameObject.SetActive(false);
    }
}
