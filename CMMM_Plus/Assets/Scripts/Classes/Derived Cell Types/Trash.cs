using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : Cell
{
    public override (bool, bool) Push(Direction_e dir, int bias)
    {
        if (bias > 0)
        {
            AudioManager.i.PlaySound(GameAssets.i.destroy);
            return (true, true);
        }
        else return (false, false);
    }
}
