using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
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
        int hideAmount =  Mathf.FloorToInt(Globals.sizeDifficulty);
        if (4.0f <= Globals.sizeDifficulty && Globals.sizeDifficulty < 4.5f) hideAmount = 3;
        else if (4.5f <= Globals.sizeDifficulty && Globals.sizeDifficulty < 6.0f) hideAmount = 4;
        else if (Globals.sizeDifficulty >= 6.0f) hideAmount = 5;

        int rng = Random.Range(0, lanes.Length-hideAmount);

        for (int i = 0; i < rng; ++i)
        {
            lanes[i].SetActive(true);
        }
        for (int i = rng; i < rng + hideAmount; ++i)
        {
            lanes[i].SetActive(false);
        }
        for (int i = rng + hideAmount; i < lanes.Length; ++i)
        {
            lanes[i].SetActive(true);
        }
        timer = maxTime;
    }

    private void OnDisable()
    {
        foreach (GameObject lane in lanes) lane.SetActive(false);
    }
}
