using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puller : TrackedCell
{

    public override void Step()
    {
        int offsetX = 0;
        int offsetY = 0;
        bool outranged = false;

        Cell pull = null;

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

        if (this.position.x - offsetX < 0 || this.position.y - offsetY < 0)
            outranged = true;
        if (this.position.x - offsetX >= CellFunctions.gridWidth || this.position.y - offsetY >= CellFunctions.gridHeight)
            outranged = true;
        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0)
            outranged = true;
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight)
            outranged = true;
        
        if (outranged == false)
        {
            if(CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY] != null)
            {
                pull = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY];
            }
        }
        
        if (pull != null)
        {
            pull.Push(this.getDirection(), 1);
        }
        else
        {
            Go:
                this.Push(this.getDirection(), 0);
        }
        this.suppresed = false;
    }

    public override (bool, bool) Push(Direction_e dir, int bias)
    {
        if(this.suppresed)
            return base.Push(dir, bias);
        if (this.getDirection() == dir)
        {
            bias += 1;
        }

        //if bias is opposite our direction
        else if ((int)(dir + 2) % 4 == (int)this.getDirection()) {
            bias -= 1;
        }

        return base.Push(dir, bias);
    }
}
