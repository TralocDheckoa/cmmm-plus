using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : Cell
{
    public ParticleSystem deathParticals;

    private CellType_e[] cellTypes = { CellType_e.GENERATOR, CellType_e.MOVER, CellType_e.CWROTATER, CellType_e.CCWROTATER, CellType_e.BLOCK, CellType_e.SLIDE, CellType_e.ENEMY, CellType_e.TRASH, CellType_e.WALL };

    public override void Setup(Vector2 position, Direction_e rotation, bool generated)
    {
        base.Setup(position, rotation, generated);
    }

    public override void Delete(bool destroy)
    {
        GridManager.enemyCount--;
        base.Delete(destroy);
    }

    public override (bool, bool) Push(Direction_e dir, int bias)
    {
        CellType_e present = CellType_e.MOVER;

        if (bias < 1)
            return (false, false);

        if (this.generated)
            this.Delete(true);
        else this.Delete(false);

        AudioManager.i.PlaySound(GameAssets.i.destroy);
        Instantiate(deathParticals, this.gameObject.transform.position, Quaternion.identity);

        present = cellTypes[Random.Range(0, cellTypes.Length)];

        Cell newCell = GridManager.instance.SpawnCell(present, this.position, this.getDirection(), true);

        return (true, true);
    }
}
