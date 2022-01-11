using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Next : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void clicked() {
        if (GridManager.currentLevel >= GameLevels.levels.Length)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else {
            GridManager.loadString = GameLevels.levels[GridManager.currentLevel + 1];
            GridManager.currentLevel += 1;
            GridManager.mode = Mode_e.LEVEL;
            SceneManager.LoadScene("LevelScreen");
        }
    }
}
