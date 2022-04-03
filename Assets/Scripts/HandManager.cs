using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    enum States {SHADOW, HAND_DELAY, HAND_IN, HAND_PRESS, HAND_OUT }

    public float radius = 1.3f;
    public GameObject shadow;
    public GameObject hand;
    public GameObject activeFlag;

    public float handSpeed = 5.0f;
    public float handDelay = 2.0f;
    public float pressDelay = 0.7f;

    public AudioSource source;

    States state = States.SHADOW;

    float timer = 0;
    Manager _manager;
    SpriteRenderer _sr;
    PolygonCollider2D _col;

    Vector2 _SPAWN_POINT_ = new Vector2(-8, -8);
    Color inactiveColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    private void Start()
    {
        _manager = FindObjectOfType<Manager>();
        _sr  = hand.GetComponent<SpriteRenderer>();
        _col = hand.GetComponent<PolygonCollider2D>();

        _col.enabled = false;
        _sr.color = inactiveColor;
    }

    private void OnEnable()
    {
        state = States.SHADOW;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        switch (state)
        {
            case States.SHADOW:

                shadow.SetActive(true);
                shadow.transform.position = radius * Random.insideUnitCircle + (Vector2)_manager.GetTarget().transform.position;
                hand.SetActive(true);
                hand.transform.position = _SPAWN_POINT_;
                state = States.HAND_DELAY;
                timer = handDelay / Globals.timeDifficulty;
                break;

            case States.HAND_DELAY:
                if (timer > 0) break;
                state = States.HAND_IN;
                break;

            case States.HAND_IN:
                {
                    Vector2 dir = (shadow.transform.position - hand.transform.position);
                    float dst = Time.deltaTime * handSpeed * Globals.timeDifficulty;

                    if (dir.magnitude < dst)
                    {
                        _col.enabled = true;
                        _sr.color = Color.white;
                        if (Globals.sfxActive) source.Play();

                        hand.transform.position = shadow.transform.position;
                        state = States.HAND_PRESS;
                        timer = pressDelay;
                    }
                    else hand.transform.Translate(dir.normalized * dst);
                }
                break;

            case States.HAND_PRESS:
                if (timer > 0) break;
        
                _col.enabled = false;
                _sr.color = inactiveColor;

                state = States.HAND_OUT;
                break;

            case States.HAND_OUT:
                {
                    Vector2 dir = (_SPAWN_POINT_ - (Vector2)hand.transform.position);
                    float dst = Time.deltaTime * handSpeed * Globals.timeDifficulty;

                    if (dir.magnitude < dst)
                    {
                        if (!activeFlag.activeSelf) this.gameObject.SetActive(false);
                        state = States.SHADOW;
                    }
                    else hand.transform.Translate(dir.normalized * dst);
                }
                break;

            default:
                break;
        }

    }
}
