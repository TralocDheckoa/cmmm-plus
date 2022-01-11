using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Cell
{
    public override (bool, bool) Push(Direction_e dir, int bias)
    {
        return (false, false);
    }

    public override void Rotate(int amount)
    {
        if (suppresed)
        {
            base.Rotate(amount);
            return;
        }
        return;
    }

}
