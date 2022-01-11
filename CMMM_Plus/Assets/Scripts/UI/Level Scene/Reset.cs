using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public Image playImage;
    public Sprite playSprite;

    public void ResetGrid()
    {
        AudioManager.playSounds = false;
        playImage.sprite = playSprite;
        GridManager.instance.Reset();

        LinkedListNode<Cell> selectedNode = CellFunctions.generatedCellList.First;

        {
            Cell cell;
            while (selectedNode != null)
            {
                cell = selectedNode.Value;
                selectedNode = selectedNode.Next;
                cell.Delete(true);
            }
        }

        CellFunctions.generatedCellList.Clear();

        foreach (Cell cell in CellFunctions.cellList)
        {
            cell.Delete(false);
        }

        foreach (Cell cell in CellFunctions.cellList)
        {
            cell.gameObject.SetActive(true);
            cell.Setup(cell.spawnPosition, (Direction_e)cell.spawnRotation, false);
            cell.suppresed = false;
        }
        AudioManager.playSounds = true;
        GridManager.enemyCount = GridManager.initialEnemyCount;
    }
}
