using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLane : Spawner
{
    public GameObject[] spawners;
    int   index = 0;

    protected override void OnEnable()
    {
        foreach (GameObject spawner in spawners) spawner.SetActive(false);
        index = Random.Range(0, spawners.Length);
    }

    protected override void Spawn()
    {
        spawners[index].SetActive(false);
        spawners[index].SetActive(true);

        int next_index = (index + Random.Range(1, spawners.Length)) % spawners.Length;

        index = next_index;
    }
}
