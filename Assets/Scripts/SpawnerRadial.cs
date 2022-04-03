using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRadial : Spawner
{
    public float radius = 8.0f;
       
    protected override void Spawn()
    {
        for (int i = 0; i < Globals.countDifficulty; i++)
        {
            GameObject go = pool.Spawn();
            float value = Random.value;
            go.transform.position = new Vector2(Mathf.Cos(6.283f * value), Mathf.Sin(6.283f * value)) * radius;
        }        
    }
}
