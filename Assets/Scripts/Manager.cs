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
        public GameObject[] entities;
        public string commentFirst;
        public string[] commentOther;
        public float time;
        public float initial_delay;
    }

    [Header("UI")]
    public Text uiConnectionStatus;
    public Text uiTimeText;
    public Text uiRecordTimeText;
    public TextWriter uiComms;
    public GameObject TutorialModeText;
    public Toggle easierMode;

    [Header("Connect Button")]
    public Color activeNormal;
    public Color activeHover;
    public Color activeDown;

    [Space()]
    public Color inactiveNormal;
    public Color inactiveHover;
    public Color inactiveDown;

    [Header("Challenges")]
    public GameObject targetPrefab;
    public Challenge[] tutorialChallenges;
    public Challenge[] randomChallenges;

    [Header("Audio")]
    public AudioClip musicConnected;
    public AudioClip musicDisconnected;
    AudioSource _as;


    bool connected = false;
    Target target;
    List<Target> extraTargets = new List<Target>();

    float startTime = 0;
    int   recordTime = 0;
    float challengeTime = 0;

    bool gettingReady = false;
    float readyTimer = 0;

    int tutorialIndex = 0;
    int randomIndex = 0;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Target>();
        _as = GetComponent<AudioSource>();

        recordTime = PlayerPrefs.GetInt("record", 0);
        int count = PlayerPrefs.GetInt("first_time", 0);
        PlayerPrefs.SetInt("first_time", count + 1);
        tutorialIndex = PlayerPrefs.GetInt("tutorial", -1);

        Disconnect();
        PaintUI();

        // Disconnect uses comms, leave CommsStart after Disconnect!
        CommsStart(count);

        //Debug
        tutorialIndex = -2;
        if (tutorialIndex < -1) TutorialModeText.SetActive(false);
        else TutorialModeText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (connected)
        {
            uiTimeText.text = Mathf.FloorToInt(Time.time - startTime).ToString();

            if (gettingReady)
            {
                readyTimer -= Time.deltaTime;
                if (readyTimer < 0)
                {
                    Challenge c = tutorialIndex >= 0 ? tutorialChallenges[tutorialIndex] : randomChallenges[randomIndex];
                    if (tutorialIndex < 0) DisplayChallengeComms(c);
                    foreach (GameObject e in c.entities) e.SetActive(true);
                    challengeTime = c.time;
                    gettingReady = false;
                }
            }
            else
            {
                challengeTime -= Time.deltaTime;
                if (challengeTime < 0)
                {
                    if (tutorialIndex > -2) StartNextTutorial();
                    else StartRandomChallenge();
                }
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
        // Paint Target
        target.SetColors(activeNormal, activeHover, activeDown);
        foreach(Target t in extraTargets) t.SetColors(activeNormal, activeHover, activeDown);

        // Set Difficulty
        /*
        Globals.countDifficulty = 1;
        Globals.timeDifficulty  = 1;
        Globals.hpExtra = 0;
        Globals.sizeDifficulty = 1.0f;
        */
        Globals.ResetGlobals();

        // Select level
        if (tutorialIndex > -2) StartNextTutorial();
        else StartRandomChallenge();

        startTime = Time.time;
        PaintUI();
        _as.clip = musicConnected;
        _as.Play();
        easierMode.interactable = false;
    }

    void StartNextTutorial()
    {
        if (tutorialIndex >= 0)
        {
            foreach (GameObject entity in tutorialChallenges[tutorialIndex].entities)
                entity.SetActive(false);
        }
        tutorialIndex++;

        bool found = false;
        for (; tutorialIndex < tutorialChallenges.Length; ++tutorialIndex)
        {
            Challenge c = tutorialChallenges[tutorialIndex];
            if (!c.enabled) continue;

            found = true;
            DisplayChallengeComms(c);

            gettingReady = true;
            readyTimer = c.initial_delay;
            break;
        }

        if (!found)
        {
            tutorialIndex = -2;
            TutorialModeText.SetActive(false);
            PlayerPrefs.SetInt("tutorial", -2);
            StartRandomChallenge();
        }

    }

    void StartRandomChallenge()
    {
        IncreaseDifficulty();

        int N = randomChallenges.Length;
        bool found = false;
        int prev_challenge = randomIndex;

        if (randomIndex >= 0)
        {
            foreach (GameObject entity in randomChallenges[randomIndex].entities)
                entity.SetActive(false);
        }

        randomIndex = (randomIndex + Random.Range(1, N)) % N;

        // Check for the next available challenge
        for (int i = randomIndex; i < N; ++i)
        {
            Challenge c = randomChallenges[i];
            if (!c.enabled || i == prev_challenge) continue;

            found = true;
            gettingReady = true;

            randomIndex = i;
            readyTimer = c.initial_delay;
            return;
        }

        // Loop back from the start
        for (int i = 0; i < randomIndex; ++i)
        {
            Challenge c = randomChallenges[i];
            if (!c.enabled || i == prev_challenge) continue;

            found = true;
            gettingReady = true;

            randomIndex = i;
            readyTimer = c.initial_delay;
            return;
        }

        if (!found)
        {
            Disconnect();
            uiComms.Write("You failed because I say so! (Not because there is some bug somewhere and this is just a failsafe...)");
        }
    }

    void DisplayChallengeComms(Challenge c)
    {
        if (PlayerPrefs.GetInt("first_" + c.Name, 0) == 0)
        {
            PlayerPrefs.SetInt("first_" + c.Name, 1);
            uiComms.Write(c.commentFirst);
        }
        else uiComms.Write(c.commentOther[Random.Range(0, c.commentOther.Length)]);
    }

    private void Disconnect(bool userAction = false)
    {
        // Update Tutorial Index
        if (tutorialIndex > -2) tutorialIndex = -1;

        // Target Management
        target.transform.localScale = Vector3.one;
        target.SetColors(inactiveNormal, inactiveHover, inactiveDown);
        target.Drop();

        // Stop Active spawners and Despawn Pooled Objects
        foreach (PoolObject po in FindObjectsOfType<PoolObject>()) po.DestroySelf();
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
        if (userAction) CommsSelfDisconect();
        else CommsOtherDisconnect();

        foreach (Target t in extraTargets) Destroy(t.gameObject);
        extraTargets.Clear();

        _as.clip = musicDisconnected;
        _as.Play();
        easierMode.interactable = true;
    }
    
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

    void CommsStart(int count)
    {
        if (count == 0) uiComms.Write("A new player! Welcome! Ehm... Look... You won't beat the game so DO NOT press the red button, ok?");
        else if (count == 1) uiComms.Write("Welcome back! You gave up once already so... again... DO NOT press the red button, there's no point to that.");
        else if (count == 2) uiComms.Write("Again? REALLY?!?! Haven't you learnt by now that there is NO POINT in pressing the red button!");
        else if (count == 3) uiComms.Write("I don't know why but you seem to be enjoying this... you are just that weird, I guess.");
        else uiComms.Write("Go on... press the red button again... I don't know why I even try...");
    }

    void CommsSelfDisconect()
    {
            int rng = Random.Range(0, 5);
            if (rng == 0) uiComms.Write("Oh! I see... you gave up!");
            else if (rng == 1) uiComms.Write("You switch the game off by yourself. That is funny! Ha ha ha!");
            else if (rng == 2) uiComms.Write("So you understood that you had to stop at some point?");
            else if (rng == 3) uiComms.Write("You didn't even delay the inevitable for THAT long...");
            else if (rng == 4) uiComms.Write("Bored already? I didn't expect you to switch the game off by yourself. Good job!");
    }

    void CommsOtherDisconnect()
    {
            int rng = Random.Range(0, 5);
            if (rng == 0) uiComms.Write("Well... that was an exercise in futility...");
            else if (rng == 1) uiComms.Write("You had to make that difficult, hadn't you? Now we can stop all this nonsense.");
            else if (rng == 2) uiComms.Write("YOU JUST GOT SLICED! Muahahahaha!");
            else if (rng == 3) uiComms.Write("You lost some time. I lost some time. Now we can go on with our lives...");
            else if (rng == 4) uiComms.Write("Good job. Now, don't do that again!");
    }

    public Target GetTarget()
    {
        
        int rng = Random.Range(0, 1 + extraTargets.Count);

        if (rng == 0) return target;
        else return extraTargets[rng - 1];
        
        //return target;
    }

    public void CreateTarget()
    {
        GameObject go = (GameObject)Instantiate(targetPrefab, this.transform);
        go.transform.position = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-4.0f, 4.0f));
        go.transform.localScale = new Vector3(Globals.sizeDifficulty, Globals.sizeDifficulty, Globals.sizeDifficulty);
        extraTargets.Add(go.GetComponent<Target>());
    }

    void IncreaseDifficulty()
    {
        float rng = Globals.easierMode ? 0.2f : Random.value;

        if (rng < 0.2f)
        {
            Globals.timeDifficulty += 0.2f;
            uiComms.Write("Let's speed thing up a bit!");
        }

        else if (0.2f <= rng && rng < 0.5f)
        {
            int idx = Random.Range(0, 6);
            if (idx==0)        uiComms.Write("I think this level is difficult enough for now...");
            else if (idx == 1) uiComms.Write("I feel like pizza tonight. WITH PINEAPPLE!");
            else if (idx == 2) uiComms.Write("Here. For you. A handful of nothing.");
            else if (idx == 3) uiComms.Write(";)");
            else if (idx == 4) uiComms.Write("No difficulty increase this round. YEY!");
            else uiComms.Write("I'm blue daba-dee daba-da. YEY, I'm old!");
        }

        else if (0.5f <= rng && rng < 0.7f)
        {
            Globals.hpExtra += 2;
            uiComms.Write("Ah! I know! I'll make things more sturdy.");
        }

        else if (0.7f <= rng && rng < 0.9f)
        {
            target.transform.localScale = target.transform.localScale + 0.5f * Vector3.one;
            foreach (Target t in extraTargets) t.transform.localScale = new Vector3(Globals.sizeDifficulty, Globals.sizeDifficulty, Globals.sizeDifficulty);
            Globals.sizeDifficulty = target.transform.localScale.x;
            uiComms.Write("Wasn't the power button too small? Better now, don't you think?");
        }

        else
        {
            CreateTarget();
            uiComms.Write("You won't mind if I place another button, won't you?");
        }
    }

    public void MuteMusic()
    {
        _as.mute = !_as.mute;
    }

    public void MuteSFX()
    {
        Globals.sfxActive = !Globals.sfxActive;
    }

    public void ToggleEasierMode()
    {
        Globals.easierMode = !Globals.easierMode;
    }
}
