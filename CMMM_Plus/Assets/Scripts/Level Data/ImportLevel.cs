using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImportLevel : MonoBehaviour
{

    public GameObject errorCard;

    string convertString(string oldFormat)
    {
        string[] components = oldFormat.Split(';');

        List<string> cells = new List<string>();
        foreach (string enemyCell in components[3].Split(','))
        {
            if(enemyCell != "")
                cells.Add("7.0." + enemyCell);
        }


        string[] newCell = { "0.0", "0.2", "0.3", "0.1", "2.0", "1.0", "3.0", "3.2", "3.3", "3.1", "5.0", "4.0", "4.1", "6.0" };
        foreach (string oldCell in components[4].Split(','))
        {
            cells.Add(newCell[int.Parse(oldCell.Split('.')[0])] + oldCell.Substring(oldCell.IndexOf('.')));
        }

        string[] newComponents = { "V1", components[0], components[1], components[2], string.Join(",", cells), components[5] };
        GUIUtility.systemCopyBuffer = string.Join(";", newComponents);
        return (string.Join(";", newComponents) + ";");
    }

    public void Play() {
        GridManager.currentLevel = 999;
        if (GUIUtility.systemCopyBuffer.StartsWith("V"))
        {
            GridManager.loadString = GUIUtility.systemCopyBuffer;
        }
        else
        {
            try
            {
                GridManager.loadString = convertString(GUIUtility.systemCopyBuffer);
            }
            catch
            {
                errorCard.GetComponent<CanvasGroup>().alpha = 1;
                errorCard.GetComponentInChildren<Text>().text = "Your clipboard doesn't contain a valid level!";

                CanvasGroup canvGroup = errorCard.GetComponent<CanvasGroup>();

                StartCoroutine(PauseThenFadeOut(canvGroup, canvGroup.alpha, 0));
                return;
            }
        }
        GridManager.mode = Mode_e.LEVEL;
        SceneManager.LoadScene("LevelScreen");
    }

    public void Edit() {
        GridManager.currentLevel = 999;
        if (GUIUtility.systemCopyBuffer.StartsWith("V"))
        {
            GridManager.loadString = GUIUtility.systemCopyBuffer;
        }
        else
        {
            try
            {
                GridManager.loadString = convertString(GUIUtility.systemCopyBuffer);
            }
            catch
            {
                errorCard.GetComponent<CanvasGroup>().alpha = 1;
                errorCard.GetComponentInChildren<Text>().text = "Your clipboard doesn't contain a valid level!";

                CanvasGroup canvGroup = errorCard.GetComponent<CanvasGroup>();

                StartCoroutine(PauseThenFadeOut(canvGroup, canvGroup.alpha, 0));
                return;
            }
        }
        GridManager.mode = Mode_e.EDITOR;
        SceneManager.LoadScene("LevelScreen");
    }

    public IEnumerator PauseThenFadeOut(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        while (counter < 3f)
        {
            counter += Time.deltaTime;
            if (counter > 2f) canvGroup.alpha = Mathf.Lerp(start, end, (counter - 2f) / 1f);
            yield return null;
        }
    }
}
