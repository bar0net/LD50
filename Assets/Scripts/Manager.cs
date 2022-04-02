using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [System.Serializable]
    public struct Challenge
    {
        public bool enabled;
        public string Name;
        public GameObject entity;
    }

    bool connected = false;

    [Header("Connect Button")]
    public Color activeNormal;
    public Color activeHover;
    public Color activeDown;

    [Space()]
    public Color inactiveNormal;
    public Color inactiveHover;
    public Color inactiveDown;

    [Header("Challenges")]
    public Challenge[] challenges;

    Target target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Target>();

        Disconnect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchConnect()
    {
        connected = !connected;

        if (connected) Connect();
        else Disconnect();
    }

    private void Connect()
    {
            target.normalColor = activeNormal;
            target.hoverColor  = activeHover;
            target.downColor   = activeDown;

            SetChallenge(0);
    }

    private void Disconnect()
    {
            target.normalColor = inactiveNormal;
            target.hoverColor  = inactiveHover;
            target.downColor   = inactiveDown;

            foreach (PoolObject p in FindObjectsOfType<PoolObject>())
            {
                p.pool.DeSpawn(p.gameObject);
            }
            SetChallenge(-1);
    }

    void SetChallenge(int index)
    {
        foreach (Challenge c in challenges)
        {
            c.entity.SetActive(index == 0);
            index--;
        }
    }
}
