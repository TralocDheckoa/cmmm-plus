using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class has so much spaghetti code, I'm genuinely ashamed of myself
public class Teleporter : TrackedCell
{

    public override void Step()
    {
        //Subract to find refrence, add to find target
        int offsetX = 0;
        int offsetY = 0;

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
        {
            return;
        }
        else if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY].cellType == CellType_e.TELEPORTER)
        {
            return;
        }

        int goffsetX = offsetX;
        int goffsetY = offsetY;


        Check:
        {
            if (this.position.x + goffsetX >= CellFunctions.gridWidth)
            return;
            if (this.position.x + goffsetX < 0)
            return;
            if (this.position.y + goffsetY >= CellFunctions.gridHeight)
            return;
            if (this.position.y + goffsetY < 0)
            return;
            if (CellFunctions.cellGrid[(int)this.position.x + goffsetX, (int)this.position.y + goffsetY] != null) // If the teleport destination is unoccupied, teleport immediately
            {
                if (CellFunctions.cellGrid[(int)this.position.x + goffsetX, (int)this.position.y + goffsetY].cellType == CellType_e.TELEPORTER && CellFunctions.cellGrid[(int)this.position.x + goffsetX, (int)this.position.y + goffsetY].getDirection() == this.getDirection())
                { // If the occupying cell is a teleporter facing the same direction, move the teleport destination
                    if (goffsetX > 0)
                    {
                        goffsetX++;
                        goto Check;
                    }
                    else if (goffsetX < 0)
                    {
                        goffsetX--;
                        goto Check;
                    }
                    else if (goffsetY < 0)
                    {
                        goffsetY--;
                        goto Check;
                    }
                    else
                    {
                        goffsetY++;
                        goto Check;
                    }
                }
                else // If the occupying cell is NOT a teleporter facing the same direction, push it
                {
                    CellFunctions.cellGrid[(int)this.position.x + goffsetX, (int)this.position.y + goffsetY].Push(this.getDirection(), 1);
                    if (CellFunctions.cellGrid[(int)this.position.x + goffsetX, (int)this.position.y + goffsetY] == null)
                    {
                        AudioManager.i.PlaySound(GameAssets.i.place);
                        Cell refrenceCell = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY];
                        Cell newCell = GridManager.instance.SpawnCell(refrenceCell.cellType,new Vector2((int)this.position.x + goffsetX, (int)this.position.y + goffsetY),refrenceCell.getDirection(),true);
                        refrenceCell.Delete(false);
                    }
                }
            }
            else
            {
                AudioManager.i.PlaySound(GameAssets.i.place);
                Cell refrenceCell = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY];
                Cell newCell = GridManager.instance.SpawnCell(refrenceCell.cellType,new Vector2((int)this.position.x + goffsetX, (int)this.position.y + goffsetY),refrenceCell.getDirection(),true);
                // commented out bc the animation looked weird sometimes
                // newCell.oldPosition = refrenceCell.position;
                refrenceCell.Delete(false);
            }
        }
    }
}
