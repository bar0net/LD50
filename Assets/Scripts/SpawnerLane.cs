using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLane : MonoBehaviour
{
    public GameObject[] lanes;
    const float maxTime = 10.0f;

    float timer = 0;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0) gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        int rng = Random.Range(0, lanes.Length);
        for (int i = 0; i < lanes.Length; ++i)
        {
            lanes[i].SetActive(i != rng);
        }
        timer = maxTime;
    }

    private void OnDisable()
    {
        foreach (GameObject lane in lanes) lane.SetActive(false);
    }
}
