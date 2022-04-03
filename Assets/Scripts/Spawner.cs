using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pool pool = null;
    public Vector2 delay = 3.0f * Vector2.one;
    protected float timer = 0;

    [System.NonSerialized]
    public float time;

    private void Awake()
    {
        time = Time.time;
    }

    virtual protected void OnEnable()
    {
        timer = delay.y / Globals.timeDifficulty;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
        {
            timer = Random.Range(delay.x, delay.y) / Globals.timeDifficulty;
            Spawn();
        }
        timer -= Time.deltaTime;
    }
    
    virtual protected void Spawn() { }
}
