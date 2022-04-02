using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBasic : MonoBehaviour
{
    public float speed = 1.0f;
    public float stopAfter = 0.25f;
    public float downTime = 0.1f;

    GameObject target = null;
    CursorPoint pointer = null;

    float timer  = 1.0f;
    bool overlap = false;

    private void OnEnable()
    {
        timer   = stopAfter;
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        pointer = GetComponentInChildren<CursorPoint>();

        Debug.Assert(target != null && pointer != null);
    }

    void Update()
    {
        // check for overlap enter or exit
        if (overlap != pointer.Hover())
        {
            overlap = pointer.Hover();
            if (!overlap) timer = stopAfter;
        }

        // move while not overlaping or on overlap inertia 
        if (timer > 0)
        {
            Move();

            if (overlap) timer -= Time.deltaTime;
        }
        else if (-downTime < timer && timer <= 0)
        {
            if (overlap) timer -= Time.deltaTime;
        }
        else
        {
            FindObjectOfType<Manager>().SwitchConnect();
        }
    }

    void Move()
    {
        Vector2 move = ((Vector2)target.transform.position - (Vector2)transform.position);
        this.transform.Translate(move.normalized * speed * Time.deltaTime);
    }
}
