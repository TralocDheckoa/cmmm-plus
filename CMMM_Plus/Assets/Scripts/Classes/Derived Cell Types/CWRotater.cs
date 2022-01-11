using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWRotater : TickedCell
{
    protected int rotationAmount = 1;

    int[][] rotationOffsets = new int[][] { 
        new int[] {1, 0}, 
        new int[] { 0, -1}, 
        new int[] { -1, 0}, 
        new int[] { 0, 1} };

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
        foreach (int[] offset in rotationOffsets)
        {
            rotateCell(offset[0], offset[1]);
        }
    }
}