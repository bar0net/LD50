using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPylon : Spawner
{
    public Transform uiTimeBar;

    private void LateUpdate()
    {
        float ratio = 1 - Mathf.Clamp01(timer / delay.x);
        uiTimeBar.localScale = new Vector3(1.0f, ratio, 1.0f);
    }

    protected override void Spawn()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject go = pool.Spawn();
            go.transform.position = this.transform.position;
            go.transform.localRotation = Quaternion.Euler(0, 0, this.transform.localRotation.eulerAngles.z + i * 45.0f);
            go.transform.localScale = 0.66f * Vector3.one;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spawner")
        {
            Spawner s = collision.GetComponent<Spawner>();
            if (this.time > s.time) Destroy(this.gameObject);
        }
    }
}
