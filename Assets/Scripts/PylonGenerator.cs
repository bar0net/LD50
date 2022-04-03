using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonGenerator : Spawner
{
    public Pool cursorPool;

    protected override void Spawn()
    {
        GameObject go = pool.Spawn();
        go.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-4, 4));
        SpawnerPylon pylon = go.GetComponent<SpawnerPylon>();
        pylon.pool = cursorPool;
    }
}
