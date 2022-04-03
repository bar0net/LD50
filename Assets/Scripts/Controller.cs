using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed = 1.0f;
    public float stopAfter = 0.25f;
    public float downTime = 0.1f;

    CursorPoint pointer = null;

    float timer = 1.0f;
    bool overlap = false;

    private void OnEnable()
    {
        timer = stopAfter;
    }

    // Start is called before the first frame update
    virtual protected void Start()
    {
        pointer = GetComponentInChildren<CursorPoint>();
    }

    // Update is called once per frame
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

    virtual protected void Move() { }
}
