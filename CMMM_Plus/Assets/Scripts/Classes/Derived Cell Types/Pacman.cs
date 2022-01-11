using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : TrackedCell
{

    private Direction_e dir;

    void Update()
    {
        if (GridManager.playSimulation == false && GridManager.stepSimulation == false)
            return;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            dir = Direction_e.UP;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            dir = Direction_e.DOWN;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            dir = Direction_e.RIGHT;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Direction_e.LEFT;
        }
    }

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        base.Setup(position, rotation, generated);
        dir = rotation;
    }

    public override void Step()
    {
        base.FaceDirection(dir);
        
        this.Push(dir, 1);

        this.suppresed = false;
    }

    public override (bool, bool) Push(Direction_e dir, int bias)
    {
        return base.Push(dir, bias);
    }
}
