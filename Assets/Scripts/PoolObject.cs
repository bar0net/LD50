using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [System.NonSerialized]
    public Pool pool = null;

    public float lifespan = 10.0f;
    float timer = 0;

    private void OnEnable()
    {
        timer = lifespan;
    }

    private void Update()
    {
        if (timer > 0 && lifespan > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0) DestroySelf();
        }
    }

    public void DestroySelf()
    {
        if (pool == null) Destroy(this.gameObject);
        else pool.DeSpawn(this.gameObject);
    }
}
