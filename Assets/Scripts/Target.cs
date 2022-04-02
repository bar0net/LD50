using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color hoverColor  = Color.white;
    bool mouseHover = false;
    int hover = 0;

    public Color downColor   = Color.white;
    bool mouseDown = false;

    SpriteRenderer _sr;
    Manager _manager;

    bool drag = false;

    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<Manager>();

        _sr = GetComponent<SpriteRenderer>();
        _sr.color = normalColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (_manager.Connected())
        {
            if (mouseHover && Input.GetMouseButtonDown(1)) drag = true;
            if (drag)
            {
                Vector2 new_pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                new_pos.x = Mathf.Clamp(new_pos.x, -5.25f, 5.25f);
                new_pos.y = Mathf.Clamp(new_pos.y, -4.05f, 4.05f);
                transform.position = new_pos;
                
                if (Input.GetMouseButtonUp(1)) drag = false;
            }
        }

    }


    void EntityEnter()
    {
        hover++;
        if (!mouseDown) _sr.color = hoverColor;
    }

    void EntityExit()
    {
        hover--;
        if (!mouseDown && hover < 1) _sr.color = normalColor;
    }

    private void OnMouseEnter()
    {
        mouseHover = true;
        EntityEnter();
    }

    private void OnMouseExit()
    {
        mouseHover = false;
        EntityExit();
    }

    private void OnMouseDown()
    {
        mouseDown = true;
        _sr.color = downColor;
    }

    private void OnMouseUp()
    {
        mouseDown = false;

        if (mouseHover) _manager.SwitchConnect(true);

        _sr.color = hover > 0 ? hoverColor : normalColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EntityEnter();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EntityExit();
    }

}
