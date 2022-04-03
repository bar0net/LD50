using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLaneManager : MonoBehaviour
{
    public float spawnDelay = 2.8f;

    public GameObject[] spawners;

    int index = 0;
    float timer = 0;

    private void OnEnable()
    {
        foreach (GameObject spawner in spawners) spawner.SetActive(false);
        index = Random.Range(0, spawners.Length);
        timer = spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            spawners[index].SetActive(false);
            spawners[index].SetActive(true);

            int next_index = (index + Random.Range(1, spawners.Length)) % spawners.Length;

            print(index.ToString() + " -> " + next_index.ToString());

            index = next_index;
            timer = spawnDelay;
        }
    }
}
