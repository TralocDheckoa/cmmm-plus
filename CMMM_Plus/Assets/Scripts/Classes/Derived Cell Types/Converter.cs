using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : TrackedCell
{

    public override void Step()
    {
        int offsetX = 0;
        int offsetY = 0;

        switch (this.getDirection())
        {
            case (Direction_e.RIGHT):
                offsetX -= 1;
                break;
            case (Direction_e.DOWN):
                offsetY -= -1;
                break;
            case (Direction_e.LEFT):
                offsetX -= -1;
                break;
            case (Direction_e.UP):
                offsetY -= 1;
                break;
        }
        //Array index error prevention
        if (this.position.x - offsetX < 0 || this.position.y - offsetY < 0)
            return;     
        if (this.position.x - offsetX >= CellFunctions.gridWidth || this.position.y - offsetY >= CellFunctions.gridHeight)
            return;
        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0)
            return;
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight)
            return;

        //If we don't have a refrence cell return
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY] == null)
            return;
        
        //Subract to find refrence, add to find target
        offsetX = 0;
        offsetY = 0;

        switch (this.getDirection())
        {
            case (Direction_e.RIGHT):
                offsetX += 1;
                break;
            case (Direction_e.DOWN):
                offsetY += -1;
                break;
            case (Direction_e.LEFT):
                offsetX += -1;
                break;
            case (Direction_e.UP):
                offsetY += 1;
                break;
        }
        //Array index error prevention
        if (this.position.x - offsetX < 0 || this.position.y - offsetY < 0)
            return;     
        if (this.position.x - offsetX >= CellFunctions.gridWidth || this.position.y - offsetY >= CellFunctions.gridHeight)
            return;
        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0)
            return;
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight)
            return;

        //If we don't have a refrence cell return
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY] == null)
            return;

        AudioManager.i.PlaySound(GameAssets.i.place);
        Cell refrenceCell = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY];
        Cell convertingCell = CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY];
        convertingCell.Delete(false);
        Cell newCell = GridManager.instance.SpawnCell(refrenceCell.cellType, new Vector2(this.position.x + offsetX, this.position.y + offsetY), convertingCell.getDirection(), true);
    }
}
