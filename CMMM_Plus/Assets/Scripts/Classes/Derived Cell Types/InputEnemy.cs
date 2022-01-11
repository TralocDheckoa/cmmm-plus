using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEnemy : TickedCell
{   

    public ParticleSystem deathParticals;

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        GridManager.enemyCount++;
        base.Setup(position, rotation, generated);
    }

    public override void Delete(bool destroy)
    {
        GridManager.enemyCount--;
        base.Delete(destroy);
    }

    void OnMouseDown()
    {
        if (GridManager.playSimulation == true)
        {
            if (this.generated)
                this.Delete(true);
            else this.Delete(false);

            AudioManager.i.PlaySound(GameAssets.i.destroy);
            Instantiate(deathParticals, this.gameObject.transform.position, Quaternion.identity);
        }
    }

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
