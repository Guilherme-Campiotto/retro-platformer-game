using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{

    public static SteamAchievements script;
    public static int unlockAchievementsCount = 0;
    public bool isAchievementCollected = false;

    public static SteamAchievements instance { get; private set; } = null;

    public bool debugForSteam= false;
    public int debugSteamAchievementCounter = 0;

    void Awake()
    {
        //PlayerPrefs.SetInt("unlockAchievementsCount", 0);
        //SteamUserStats.ResetAllStats(true); // apaga todas as conquistas

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //debug only
        debugForSteam = true;

        script = this;
        if(!SteamManager.Initialized)
        {
            gameObject.SetActive(false);
        }

    }

    public void Update()
    {
        /*
        if(debugForSteam)
        {
            DEBUG_checkDebugControls();
        }
        */
        
    }

    public void UnlockSteamAchievement(string id)
    {
        TestSteamAchievement(id);
        if(!isAchievementCollected)
        {
            unlockAchievementsCount++;
            PlayerPrefs.SetInt("unlockAchievementsCount", unlockAchievementsCount);
            Debug.Log("Conquistas conquistadas: " + unlockAchievementsCount);
            SteamUserStats.SetAchievement(id);
            SteamUserStats.StoreStats();


            if(unlockAchievementsCount == 10)
            {
                Debug.Log("Achievement 1/12: Pegar todas as conquistas");
                UnlockSteamAchievement("NEW_ACHIEVEMENT_1_11");
            }
        }
    }

    public void DEBUG_LockSteamAchievement(string id)
    {
        TestSteamAchievement(id);
        if(isAchievementCollected)
        {
            SteamUserStats.ClearAchievement(id);
        }
    }

    public void TestSteamAchievement(string id)
    {
        SteamUserStats.GetAchievement(id, out isAchievementCollected);
    }

    public void DEBUG_checkDebugControls()
    {
        /*
        if(Input.GetButtonDown("UnlockAchiev."))
        {
            UnlockSteamAchievement(debugSteamAchievementCounter.ToString());
            debugSteamAchievementCounter++;
        }

        if (Input.GetButtonDown("LockAchiev."))
        {
            DEBUG_LockSteamAchievement(debugSteamAchievementCounter.ToString());
            debugSteamAchievementCounter++;
        }

        if(debugSteamAchievementCounter > 11)
        {
            debugSteamAchievementCounter = 0;
        }
        */
    }
}
