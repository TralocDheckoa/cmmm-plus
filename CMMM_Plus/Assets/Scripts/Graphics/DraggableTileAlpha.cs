using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DraggableTileAlpha : MonoBehaviour
{
    void Update()
    {
        if (GridManager.clean && (PlayerPrefs.GetInt("AlwaysFade") == 1 || Input.GetKey(KeyCode.Space)))
        {
            for (int y = 0; y < CellFunctions.gridHeight; y++)
            {
                for (int x = 0; x < CellFunctions.gridWidth; x++)
                {
                    if (GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) != GridManager.instance.placebleTile)
                    {
                        if (CellFunctions.cellGrid[x, y] != null)
                            CellFunctions.cellGrid[x, y].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
                    }
                }
            }
        }
        else
        {
            for (int y = 0; y < CellFunctions.gridHeight; y++)
            {
                for (int x = 0; x < CellFunctions.gridWidth; x++)
                {
                    if (CellFunctions.cellGrid[x, y] != null)
                        CellFunctions.cellGrid[x, y].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                }
            }
        }
    }
}
