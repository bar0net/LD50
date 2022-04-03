using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBasic : Controller
{
    GameObject target = null;
    Manager _manager;
    

    override protected void OnEnable()
    {
        base.OnEnable();
        _manager = FindObjectOfType<Manager>();
        target = _manager.GetTarget().gameObject;
    }

    override protected void Move() 
    {
        Vector2 move = ((Vector2)target.transform.position - (Vector2)transform.position);
        this.transform.Translate(move.normalized * speed * Time.deltaTime);
    }
}
