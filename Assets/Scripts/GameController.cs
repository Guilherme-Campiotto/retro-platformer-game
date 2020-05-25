using Steamworks;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Vector2 levelInicialPosition;
    public GameObject endGamePanel;
    public bool panelActive = false;
    public GameObject player;
    public CharacterController2D playerController;
    public GravityController gravityController;
    public PlatformButton button;
    public SoundController soundController;
    public bool levelComplete = false;
    public Animator animatorFadeLevel;

    public static AudioClip menu;
    public static AudioClip theme1;
    public static AudioClip theme2;
    public static AudioClip theme3;
    public static AudioClip theme4;
    public static AudioClip theme5;
    public static AudioClip theme6;
    public static AudioClip themeIntro;
    public static AudioClip themeEnding;

    private GameObject steamAchievements;
    SteamAchievements scriptAchievments;

    private int achievmentsCount;
    private Coroutine coroutineHideCursor;

    private string[] achievementsIdList = new string[10] { "NEW_ACHIEVEMENT_1_0", "NEW_ACHIEVEMENT_1_1", "NEW_ACHIEVEMENT_1_2", "NEW_ACHIEVEMENT_1_3", "NEW_ACHIEVEMENT_1_4", "NEW_ACHIEVEMENT_1_6", "NEW_ACHIEVEMENT_1_7", "NEW_ACHIEVEMENT_1_9", "NEW_ACHIEVEMENT_1_10", "NEW_ACHIEVEMENT_1_11" };

    public void Start()
    {

        if(gravityController != null)
        {
            gravityController.setGravityDefault();
        }

        GameObject soundObject = GameObject.Find("SoundController");
        GameObject canvas = GameObject.Find("Canvas");

        if (!soundObject.activeSelf)
        {
            soundObject.SetActive(true);
        }

        soundController = soundObject.GetComponent<SoundController>();

        CheckMusicTheme(SceneManager.GetActiveScene().buildIndex);

        steamAchievements = GameObject.Find("SteamAchievements");

        if(steamAchievements != null)
        {
            scriptAchievments = steamAchievements.GetComponent<SteamAchievements>();
            LoadStatistics();
        }

        Time.timeScale = 1f;

        // Verificar depois nos testes como pegar essa informação na steam
        achievmentsCount = PlayerPrefs.GetInt("achievmentsCount");

    }

    public void Update()
    {
        CheckMouseMovement();
    }

    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void nextLevel()
    {

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        levelComplete = true;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            // Conquista zerar o jogo
            if(scriptAchievments != null)
            {
                Debug.Log("Achievement 1/6: Zerar o jogo");
                scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_6");
            }

            SceneManager.LoadScene("Ending");
        }
        else
        {
            SaveProgress(nextScene);

            SceneManager.LoadScene(nextScene);
        }
    }

    private void CheckMusicTheme(int sceneIndex)
    {
        switch(sceneIndex)
        {
            case 2:
                if (themeIntro == null)
                {
                    theme1 = theme2 = theme3 = theme4 = theme5 = theme6 = themeEnding = null;
                    themeIntro = Resources.Load<AudioClip>("Sounds/Musics/Intro music - Deep_In_Space");
                    soundController.PlayWithLoop(themeIntro);
                }
                break;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
                if (theme1 == null)
                {
                    themeIntro = theme2 = theme3 = theme4 = theme5 = theme6 = themeEnding = null;
                    theme1 = Resources.Load<AudioClip>("Sounds/Musics/Theme_1");
                    soundController.PlayWithLoop(theme1);
                }
                break;
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:
                if (theme2 == null)
                {
                    themeIntro = theme1 = theme3 = theme4 = theme5 = theme6 = themeEnding = null;
                    theme2 = Resources.Load<AudioClip>("Sounds/Musics/Theme_2");
                    soundController.PlayWithLoop(theme2);
                }
                break;
            case 20:
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
                if (theme3 == null)
                {
                    themeIntro = theme1 = theme2 = theme4 = theme5 = theme6 = themeEnding = null;
                    theme3 = Resources.Load<AudioClip>("Sounds/Musics/Theme_3");
                    soundController.PlayWithLoop(theme3);
                }
                break;
            case 27:
            case 28:
            case 29:
            case 30:
            case 31:
            case 32:
            case 33:
                if (theme4 == null)
                {
                    themeIntro = theme1 = theme2 = theme3 = theme5 = theme6 = themeEnding = null;
                    theme4 = Resources.Load<AudioClip>("Sounds/Musics/Theme_4");
                    soundController.PlayWithLoop(theme4);
                }
                break;
            case 34:
            case 35:
            case 36:
            case 37:
            case 38:
            case 39:
            case 40:
                if (theme5 == null)
                {
                    themeIntro = theme1 = theme2 = theme3 = theme4 = theme6 = themeEnding = null;
                    theme5 = Resources.Load<AudioClip>("Sounds/Musics/Theme_5");
                    soundController.PlayWithLoop(theme5);
                }
                break;
            case 41:
            case 42:
            case 43:
            case 44:
            case 45:
            case 46:
            case 47:
                if (theme6 == null)
                {
                    themeIntro = theme1 = theme2 = theme3 = theme4 = theme5 = themeEnding = null;
                    theme6 = Resources.Load<AudioClip>("Sounds/Musics/Theme_6");
                    soundController.PlayWithLoop(theme6);
                }
                break;
            case 48:
                
                if (themeEnding == null)
                {
                    themeIntro = theme1 = theme2 = theme3 = theme4 = theme5 = theme6 = null;
                    themeEnding = Resources.Load<AudioClip>("Sounds/Musics/Ending_Music");
                    soundController.PlayWithLoop(themeEnding);
                }
                break;
        }
    }

    private void SaveProgress(int nextScene)
    {
        Debug.Log("salvando progresso...");
        int progress = PlayerPrefs.GetInt("PlayerProgress");

        if (progress < nextScene && nextScene > 2)
        {
            PlayerPrefs.SetInt("PlayerProgress", nextScene);
        }

        if(player)
        {
            Debug.Log("Tempo fase: " + player.GetComponent<CharacterController2D>().stageTime);
        }

        if(scriptAchievments != null)
        {
            switch (nextScene)
            {
                case 13:
                    // Conquista passar todas as fases do mundo 1
                    Debug.Log("Achievement 1/0: Passar mundo 1");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_0");
                    break;
                case 20:
                    // Conquista passar todas as fases do mundo 2
                    Debug.Log("Achievement 1/1: Passar mundo 2");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_1");
                    break;
                case 27:
                    // Conquista passar todas as fases do mundo 3
                    Debug.Log("Achievement 1/2: Passar mundo 3");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_2");
                    break;
                case 34:
                    // Conquista passar todas as fases do mundo 4
                    Debug.Log("Achievement 1/3: Passar mundo 4");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_3");
                    break;
                case 41:
                    // Conquista passar todas as fases do mundo 5
                    Debug.Log("Achievement 1/4: Passar mundo 5");
                    scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_4");
                    break;
            }

            // Conquista passar fase 1-4 em menos de 10 segundos
            if (SceneManager.GetActiveScene().buildIndex == 9 && player.GetComponent<CharacterController2D>().stageTime <= 10f)
            {
                Debug.Log("Achievement 1/11: Passar fase 1-4 em menos de 10 segundos");
                scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_10");
            }
        } else
        {
            Debug.Log("scriptAchievments is null");
        }
    }

    public void LoadStatistics()
    {
        if(scriptAchievments != null && steamAchievements.activeSelf)
        {
            CheckNumberOfAchievementsCount();
            if (SteamAchievements.unlockAchievementsCount == 9)
            {
                scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_11");
            }
        }
    }

    private void CheckMouseMovement()
    {
        if (Input.GetAxis("Mouse X") == 0 && (Input.GetAxis("Mouse Y") == 0))
        {
            if (coroutineHideCursor == null)
            {
                coroutineHideCursor = StartCoroutine("HideCursor");
            }
        }
        else
        {
            if (coroutineHideCursor != null)
            {
                StopCoroutine(coroutineHideCursor);
                coroutineHideCursor = null;
                Cursor.visible = true;
            }
        }
    }

    IEnumerator HideCursor()
    {
        yield return new WaitForSeconds(3);
        Cursor.visible = false;
    }

    private void CheckNumberOfAchievementsCount()
    {
        SteamAchievements.unlockAchievementsCount = 0;

        foreach (string achiev in achievementsIdList) {
            scriptAchievments.TestSteamAchievement(achiev);
            if(scriptAchievments.isAchievementCollected)
            {
                SteamAchievements.unlockAchievementsCount++;
            }
        }

        //Debug.Log("Achievements count total: " + SteamAchievements.unlockAchievementsCount);

    }

}
