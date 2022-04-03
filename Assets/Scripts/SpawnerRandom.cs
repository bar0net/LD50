using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRandom : Spawner
{
    [Header("SpawningArea")]
    public Transform topRight;
    public Transform botLeft;

    protected override void Spawn()
    {
        GameObject go = pool.Spawn();
        go.transform.position = new Vector2(Random.Range(botLeft.position.x, topRight.position.x), Random.Range(botLeft.position.y, topRight.position.y));
    }
}
