using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : TrackedCell
{
    public override void Step()
    {
        this.Push(this.getDirection(), 0);
        //Suppressed will get set to true so we have to reset it.
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
