using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public GameObject cursor;
    public GameObject arrows;

    public float arrowTime = 3.0f;
    float timer = 0;

    private void OnEnable()
    {
        cursor.transform.localPosition = -2 * Vector2.right;
        cursor.SetActive(false);
        arrows.SetActive(true);
        timer = arrowTime;
    }

    void Update()
    {
        if (timer < 0 && !cursor.activeSelf)
        {
            cursor.SetActive(true);
            arrows.SetActive(false);
        }
        else timer -= Time.deltaTime;
    }
}
