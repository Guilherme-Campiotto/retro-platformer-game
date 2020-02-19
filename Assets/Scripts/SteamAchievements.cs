using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{

    public static SteamAchievements script;
    public bool isAchievementCollected = false;

    public bool debugForSteam= false;
    public int debugSteamAchievementCounter = 0;

    private void Awake()
    {
        //debug only
        debugForSteam = true;

        script = this;
        if(!SteamManager.Initialized)
        {
            gameObject.SetActive(false);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if(debugForSteam)
        {
            DEBUG_checkDebugControls();
        }
        
    }

    public void UnlockSteamAchievement(string id)
    {
        TestSteamAchievement(id);
        if(!isAchievementCollected)
        {
            SteamUserStats.SetAchievement(id);
            SteamUserStats.StoreStats();
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
    }
}
