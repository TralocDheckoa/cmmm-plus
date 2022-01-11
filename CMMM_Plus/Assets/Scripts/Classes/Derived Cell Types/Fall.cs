using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : TrackedCell
{

    public override void Step()
    {
        
        int fallDistance = 0;

        Check:
        {
            if (this.position.y - fallDistance - 1 < 0)
            {
                if (fallDistance != 0)
                {
                    Cell newCell = GridManager.instance.SpawnCell(this.cellType, new Vector2((int)this.position.x, (int)this.position.y - fallDistance), this.getDirection(), true);
                    newCell.oldPosition = this.position;
                    this.Delete(false);
                    this.suppresed = false;
                }
            }
            else if (CellFunctions.cellGrid[(int)this.position.x, (int)this.position.y - fallDistance - 1] == null)
            {
                fallDistance++;
                goto Check;
            }
            else
            {
                if (fallDistance != 0)
                {
                    Cell newCell = GridManager.instance.SpawnCell(this.cellType, new Vector2((int)this.position.x, (int)this.position.y - fallDistance), this.getDirection(), true);
                    newCell.oldPosition = this.position;
                    this.Delete(false);
                    this.suppresed = false;
                }
            }
        }
    }

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        base.Setup(position, Direction_e.DOWN, generated);
    }

    public override void Rotate(int amount)
    {
        return;
    }
}