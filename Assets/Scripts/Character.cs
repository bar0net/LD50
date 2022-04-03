using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : PoolObject
{
    public int initialHealth = 3;
    public Transform uiHealth = null;

    [SerializeField]
    int health = 10;
    int maxHealth = 10;

    private void OnEnable()
    {
        ResetValues();
    }

    private void OnMouseUp()
    {
        SetHealth(health - 1);

        if (health <= 0)
        {
            Die();
        }
    }

    void ResetValues()
    {
        SetHealth(initialHealth + Globals.hpExtra, initialHealth + Globals.hpExtra);
    }

    void Die()
    {
        if (pool == null) Destroy(this.gameObject);
        else pool.DeSpawn(this.gameObject);
    }

    void SetHealth(int value, int maxValue = -1)
    {
        health = value > 0 ? value : 0;
        if (maxValue > 0) maxHealth = maxValue;
        uiHealth.localScale = new Vector3(uiHealth.localScale.x, (float)health / (float)maxHealth, uiHealth.localScale.z);
    }
}
