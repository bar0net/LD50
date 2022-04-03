using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static float timeDifficulty   = 1.0f;
    public static int   countDifficulty  = 1;
    public static float sizeDifficulty   = 1.0f;
    public static int hpExtra = 0;

    public static bool sfxActive  = true;
    public static bool easierMode = false;

    public static void ResetGlobals()
    {
        if (easierMode)
        {
            timeDifficulty = 0.75f;
            countDifficulty = 1;
            sizeDifficulty = 1.0f;
            hpExtra = -2;
        }
        else
        {
            timeDifficulty = 1.0f;
            countDifficulty = 1;
            sizeDifficulty = 1.0f;
            hpExtra = 0;
        }
    }



}
