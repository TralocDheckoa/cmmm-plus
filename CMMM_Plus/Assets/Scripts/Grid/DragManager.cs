using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DragManager : MonoBehaviour
{
    Vector3 mousePos;
    Cell selectedCell;
    bool inDrag = false;

    public void EndDrag() {
        if (mousePos.x < 0 || mousePos.y < 0 || mousePos.x >= CellFunctions.gridWidth || mousePos.y >= CellFunctions.gridHeight) 
        {
            CancelDrag();
            return;
        }

        if (GridManager.instance.tilemap.GetTile(new Vector3Int((int)mousePos.x, (int)mousePos.y, 0))
            != GridManager.instance.placebleTile && GridManager.mode != Mode_e.EDITOR) {
            CancelDrag();
            return;
        }
        AudioManager.i.PlaySound(GameAssets.i.place);
        if (CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y] == null)
        {
            selectedCell.setPosition((int)mousePos.x, (int)mousePos.y);
            inDrag = false;
            selectedCell.animate = true;
            selectedCell.spawnPosition = selectedCell.position;
        }
        else {
            Cell cell = CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y];
            CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y].setPosition(selectedCell.position);
            selectedCell.position = new Vector2((int)mousePos.x, (int)mousePos.y);
            selectedCell.setPosition((int)mousePos.x, (int)mousePos.y);
            selectedCell.animate = true;
            inDrag = false;
            cell.spawnPosition = cell.position;
            selectedCell.spawnPosition = selectedCell.position;
        }
        GridManager.hasSaved = false;
    }

    public void CancelDrag() {
        inDrag = false;
        if (selectedCell != null)
            selectedCell.animate = true;
    }

    public void StartDrag() {
        if (mousePos.x < 0 || mousePos.y < 0)
            return;
        if (mousePos.x >= CellFunctions.gridWidth || mousePos.y >= CellFunctions.gridHeight)
            return;

        if ((GridManager.instance.tilemap.GetTile(new Vector3Int((int)mousePos.x, (int)mousePos.y, 0))
            != GridManager.instance.placebleTile && GridManager.mode != Mode_e.EDITOR))
        {
            CancelDrag();
            return;
        }

        if (CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y] != null)
        {
            selectedCell = CellFunctions.cellGrid[(int)mousePos.x, (int)mousePos.y];
            selectedCell.animate = false;
            inDrag = true;
        }
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)) + new Vector3(.5f,.5f,0);


        if (inDrag) {
            selectedCell.transform.position = new Vector3(mousePos.x - .5f, mousePos.y - .5f, -5);
        }

        if (EventSystem.current.IsPointerOverGameObject() || (GridManager.tool != Tool_e.DRAG && GridManager.mode == Mode_e.EDITOR))
            return;
        if (!GridManager.clean)
            return;

        if (Input.GetMouseButtonDown(0)) {
            StartDrag();
        }
        if (Input.GetMouseButtonUp(0) && inDrag && selectedCell != null)
        {
            EndDrag();
        }
    }
}
