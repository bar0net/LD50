using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    public float delay = 0.1f;
    const string initial_space = "            ";
    public Animator anim;
    public AudioSource _as;

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
        anim.SetBool("talking", index < text.Length);

        if (index < text.Length)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                index++;
                _ui.text = initial_space + text.Substring(0, index);
                timer = delay;
            }

            if (_as.isPlaying && !Globals.sfxActive) _as.Stop();
            if (index == text.Length) _as.Stop();
        }

    }

    public void Write(string value)
    {
        text = value;
        index = 0;
        if (Globals.sfxActive) _as.Play();
    }
}
