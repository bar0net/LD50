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
        Debug.Log("Mouse Enter");
        mouseHover = true;
        EntityEnter();
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        mouseHover = false;
        EntityExit();
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        mouseDown = true;
        _sr.color = downColor;
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse Up");
        mouseDown = false;

        if (mouseHover) _manager.SwitchConnect();

        _sr.color = hover > 0 ? hoverColor : normalColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger!");
        EntityEnter();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EntityExit();
    }

}
