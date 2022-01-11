using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopulateLevelGrid : MonoBehaviour

// populates the level select grid

{
    public GameObject prefab;

    void Start()
    {
        Populate();
    }

    void Populate()
    {
        GameObject newObj;

        for (int i = 0; i < GameLevels.levels.Length; i++)
        {
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.GetComponentInChildren<Text>().text = (i + 1) + "";
            if (PlayerPrefs.GetInt("Level" + i, 0) == 1)
            {
                newObj.GetComponentInChildren<Text>().color = new Color32(159, 162, 243, 255);
            }

            int levelToLoad = i;

            newObj.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                GridManager.loadString = GameLevels.levels[levelToLoad];
                GridManager.currentLevel = levelToLoad;
                GridManager.mode = Mode_e.LEVEL;
                SceneManager.LoadScene("LevelScreen");
            });
        }
    }
}
