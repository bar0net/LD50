using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerForward : Controller
{
    override protected void Move()
    {
        this.transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
