using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateLevel : MonoBehaviour
{
    public InputField width;
    public InputField height;
    public void Create() {
        GridManager.currentLevel = 999;
        if (width.text != "" && height.text != "")
        {
            GridManager.mode = Mode_e.EDITOR;
            CellFunctions.gridWidth = int.Parse(width.text);
            CellFunctions.gridHeight = int.Parse(height.text);
            GridManager.loadString = "";
            SceneManager.LoadScene("LevelScreen");
        }
    }
}
