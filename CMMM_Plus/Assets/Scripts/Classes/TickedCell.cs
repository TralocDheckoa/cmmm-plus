using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickedCell : Cell
{
    LinkedListNode<Cell> tickedCellListNode;

    public virtual void Step() { }

    public override void Delete(bool destroy) {
        if (tickedCellListNode.List != null)
            tickedCellListNode.List.Remove(tickedCellListNode);
        base.Delete(destroy);
    }

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        tickedCellListNode = CellFunctions.tickedCellList[CellFunctions.GetSteppedCellId(this.cellType)].AddLast(this);
        base.Setup(position, rotation, generated);
    }
}