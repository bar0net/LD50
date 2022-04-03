using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float delay  = 5.0f;
    public float timeON = 0.25f;

    public float wobbleRange = 10.0f;
    public float wobbleSpeed = 1.0f;

    public SpriteMask mask;
    public SpriteRenderer maskSprite;
    public SpriteRenderer eyeColor;

    bool onDelay = true;
    float timer = 0;
    Color offColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);

    Manager _manager;

    private void Start()
    {
        _manager = FindObjectOfType<Manager>();
    }

    void OnEnable()
    {
        timer = (delay + Random.value) / Globals.timeDifficulty;
        eyeColor.color = offColor;
        onDelay = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (onDelay)
        {
            mask.alphaCutoff = 1.0f - Mathf.Clamp01(Globals.timeDifficulty * timer / delay);

            if (timer < 0)
            {
                timer = timeON / Globals.timeDifficulty;
                onDelay = false;
                eyeColor.color = maskSprite.color;
            }
        }
        else
        {
            if (timer < 0)
            {
                Act();
                timer = delay / Globals.timeDifficulty;
                onDelay = true;
                eyeColor.color = offColor;
            }
        }


        this.transform.localRotation = Quaternion.Euler(0, 0,wobbleRange * Mathf.Sin(Time.time * wobbleSpeed));
    }

    void Act()
    {
        _manager.GetTarget().SetPosition(3.5f * Random.insideUnitCircle);
    }
}
