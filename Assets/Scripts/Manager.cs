using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [System.Serializable]
    public struct Challenge
    {
        public bool enabled;
        public string Name;
        public GameObject entity;
        public string commentFirst;
        public string[] commentOther;
        public float time;
    }

    [Header("UI")]
    public Text uiConnectionStatus;
    public Text uiTimeText;
    public Text uiRecordTimeText;
    public TextWriter uiComms;

    [Header("Connect Button")]
    public Color activeNormal;
    public Color activeHover;
    public Color activeDown;

    [Space()]
    public Color inactiveNormal;
    public Color inactiveHover;
    public Color inactiveDown;

    [Header("Challenges")]
    public Challenge[] tutorialChallenges;
    public Challenge[] randomChallenges;


    bool connected = false;
    Target target;

    float startTime = 0;
    int recordTime = 0;
    float challengeTime = 0;

    int tutorialIndex = 0;
    int randomIndex = 0;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Target>();

        recordTime = PlayerPrefs.GetInt("record", 0);

        Disconnect();
        PaintUI();

        int count = PlayerPrefs.GetInt("first_time", 0);

        if (count == 0) uiComms.Write("A new player! Welcome! Ehm... Look... You won't beat the game so DO NOT press the red button, ok?");
        else if (count == 1) uiComms.Write("Welcome back! You gave up once already so... again... DO NOT press the red button, there's no point to that.");
        else if (count == 2) uiComms.Write("Again? REALLY?!?! Haven't you learnt by now that there is NO POINT in pressing the red button!");
        else if (count == 3) uiComms.Write("I don't know why but you seem to be enjoying this... you are just that weird, I guess.");
        else uiComms.Write("Go on... press the red button again... I don't know why I even try...");

        PlayerPrefs.SetInt("first_time", count + 1);
        PlayerPrefs.GetInt("tutorial", -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (connected)
        {
            uiTimeText.text = Mathf.FloorToInt(Time.time - startTime).ToString();

            challengeTime -= Time.deltaTime;
            if (challengeTime < 0)
            {
                if (tutorialIndex > -2) StartNextTutorial();
                else StartRandomChallenge();
            }
        }
    }

    public void SwitchConnect(bool userAction = false)
    {
        connected = !connected;

        if (connected) Connect();
        else Disconnect(userAction);
    }

    private void Connect()
    {
        target.normalColor = activeNormal;
        target.hoverColor  = activeHover;
        target.downColor   = activeDown;

        if (tutorialIndex > -2) StartNextTutorial();
        else StartRandomChallenge();

        startTime = Time.time;
        PaintUI();
    }

    void StartNextTutorial()
    {
        if (tutorialIndex >= 0) tutorialChallenges[tutorialIndex].entity.SetActive(false);
        tutorialIndex++;

        bool found = false;
        for (; tutorialIndex < tutorialChallenges.Length; ++tutorialIndex)
        {
            Challenge c = tutorialChallenges[tutorialIndex];
            if (!c.enabled) continue;

            found = true;
            c.entity.SetActive(true);
            DisplayChallengeComms(c);
            challengeTime = c.time;
            break;
        }

        if (!found)
        {
            tutorialIndex = -2;
            PlayerPrefs.SetInt("tutorial", -2);
            StartRandomChallenge();
        }

    }

    void StartRandomChallenge()
    {

    }

    void DisplayChallengeComms(Challenge c)
    {
        if (PlayerPrefs.GetInt("first_" + c.Name, 0) == 0)
        {
            PlayerPrefs.SetInt("first_" + c.Name, 0);
            uiComms.Write(c.commentFirst);
        }
        else uiComms.Write(c.commentOther[Random.Range(0, c.commentOther.Length)]);
    }

    private void Disconnect(bool userAction = false)
    {
        // Update Tutorial Index
        if (tutorialIndex > -2) tutorialIndex = -1;

        // Target Management
        target.normalColor = inactiveNormal;
        target.hoverColor  = inactiveHover;
        target.downColor   = inactiveDown;
        target.Drop();

        // Stop Active spawners and Despawn Pooled Objects
        foreach (PoolObject p in FindObjectsOfType<PoolObject>()) p.pool.DeSpawn(p.gameObject);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Spawner")) go.SetActive(false);

        // Update Record Time
        int currTime = Mathf.FloorToInt(Time.time - startTime);
        if (currTime > recordTime)
        {
            recordTime = currTime;
            PlayerPrefs.SetInt("record", currTime);
        }
        PaintUI();

        // Give feedback via comms
        if (userAction)
        {
            int rng = Random.Range(0, 5);
            if (rng == 0) uiComms.Write("Oh! I see... you gave up!");
            else if (rng == 1) uiComms.Write("You switch the game off by yourself. That is funny! Ha ha ha!");
            else if (rng == 2) uiComms.Write("So you understood that you had to stop at some point?");
            else if (rng == 3) uiComms.Write("You didn't even delay the inevitable for THAT long...");
            else if (rng == 4) uiComms.Write("Bored already? I didn't expect you to switch the game off by yourself. Good job!");
        }
        else
        {
            int rng = Random.Range(0, 5);
            if (rng == 0) uiComms.Write("Well... that was an exercise in futility...");
            else if (rng == 1) uiComms.Write("You had to make that difficult, hadn't you? Now we can stop all this.");
            else if (rng == 2) uiComms.Write("YOU JUST GOT SLICED! Muahahahaha!");
            else if (rng == 3) uiComms.Write("You lost some time. I lost some time. Now we can go on with our lives...");
            else if (rng == 4) uiComms.Write("Good job. Now, don't do that again!");
        }
    }

    /*void SetChallenge(int index)
    {
        foreach (Challenge c in randomChallenges)
        {
            if (index == 0)
            {
                c.entity.SetActive(true);
                if (PlayerPrefs.GetInt("first_" + c.Name, 0) == 0)
                {
                    PlayerPrefs.SetInt("first_" + c.Name, 0);
                    uiComms.Write(c.commentFirst);
                }
                else uiComms.Write(c.commentOther[Random.Range(0,c.commentOther.Length)]);
            }
            else
            {
                c.entity.SetActive(false);
            }
            index--;
        }
    }*/

    void PaintUI()
    {
        uiConnectionStatus.text = connected ? "CONECTED" : "DISCONECTED";
        uiTimeText.text = "0";
        uiRecordTimeText.text = recordTime.ToString();
    }

    public bool Connected()
    {
        return connected;
    }
}
