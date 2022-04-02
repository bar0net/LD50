using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    public float delay = 0.1f;
    const string initial_space = "            ";

    string text = "";
    int index = 0;
    float timer = 0;

    Text _ui;

    private void Start()
    {
        timer = delay;
        _ui = GetComponent<Text>();
    }

    private void Update()
    {

        if (index < text.Length)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                index++;
                _ui.text = initial_space + text.Substring(0, index);
                timer = delay;
            }
        }
    }

    public void Write(string value)
    {
        text = value;
        index = 0;
    }
}
