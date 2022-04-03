using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBasic : Controller
{
    GameObject target = null;

    override protected void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Target");
    }

    override protected void Move() 
    {
        Vector2 move = ((Vector2)target.transform.position - (Vector2)transform.position);
        this.transform.Translate(move.normalized * speed * Time.deltaTime);
    }
}
