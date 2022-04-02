using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRadial : MonoBehaviour
{
    public Pool pool;
    public Vector2 delay = 3.0f *  Vector2.one;
    public float radius = 8.0f;

    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
        {
            timer = Random.Range(delay.x, delay.y);

            GameObject go = pool.Spawn();
            float value = Random.value;
            go.transform.position = new Vector2(Mathf.Cos(6.283f * value), Mathf.Sin(6.283f * value)) * radius;
        }

        timer -= Time.deltaTime;
    }
}
