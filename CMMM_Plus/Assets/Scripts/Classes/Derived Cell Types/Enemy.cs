using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Cell
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
}
