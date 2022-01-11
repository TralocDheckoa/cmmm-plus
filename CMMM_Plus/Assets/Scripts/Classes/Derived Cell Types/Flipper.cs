using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : TickedCell
{
    protected int rotationAmount = 2;

    int[][] rotationOffsetsX = new int[][] { 
        new int[] {1, 0},
        new int[] { -1, 0}
    };

    int[][] rotationOffsetsY = new int[][] { 
        new int[] {0, 1},
        new int[] {0, -1}
    };

    void rotateCell(int xOffset, int yOffset)
    {

        if (this.position.x + xOffset >= CellFunctions.gridWidth || this.position.y + yOffset >= CellFunctions.gridHeight)
            return;
        if (this.position.x + xOffset < 0 || this.position.y + yOffset < 0)
            return;
        if (CellFunctions.cellGrid[(int)this.position.x + xOffset, (int)this.position.y + yOffset] == null)
            return;

        CellFunctions.cellGrid[(int)this.position.x + xOffset, (int)this.position.y + yOffset].Rotate(rotationAmount);
    }
    public override void Step()
    {
        if (this.getDirection() == Direction_e.UP || this.getDirection() == Direction_e.DOWN)
        {
            foreach (int[] offset in rotationOffsetsY)
            {
                rotateCell(offset[0], offset[1]);
            }
        }
        else
        {
            foreach (int[] offset in rotationOffsetsX)
            {
                rotateCell(offset[0], offset[1]);
            }
        }
    }
}