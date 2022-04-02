using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPoint : MonoBehaviour
{
    bool hover = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
            hover = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
            hover = false;
    }

    public bool Hover() { return hover; }
}
