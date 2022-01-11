using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TrackedCell
{
    public ParticleSystem deathParticals;

    public override void Step()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            this.Move(Direction_e.UP, 1);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            this.Move(Direction_e.DOWN, 1);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            this.Move(Direction_e.RIGHT, 1);
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.Move(Direction_e.LEFT, 1);
        }
        this.suppresed = false;
    }

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        base.Setup(position, Direction_e.RIGHT, generated);
    }

    public (bool, bool) Move(Direction_e dir, int bias)
    {
        return base.Push(dir, bias);
    }
    
    public override void Delete(bool destroy)
    {
        base.Delete(destroy);
    }

    public override (bool, bool) Push(Direction_e dir, int bias)
    {
        if (bias < 1)
            return (false, false);

        if (this.generated)
            this.Delete(true);
        else this.Delete(false);

        AudioManager.i.PlaySound(GameAssets.i.destroy);
        Instantiate(deathParticals, this.gameObject.transform.position, Quaternion.identity);

        return (true, true);
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
