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

    private GameObject steamAchievements;
    SteamAchievements scriptAchievments;

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
        }

        Time.timeScale = 1f;

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
            if(scriptAchievments != null)
            {
                scriptAchievments.UnlockSteamAchievement("achiev6");
            }

            if(player.GetComponent<CharacterController2D>().deathCountStage == 0 && scriptAchievments != null)
            {
                scriptAchievments.UnlockSteamAchievement("achiev9");
            }

            if(SceneManager.GetActiveScene().buildIndex == 8 && player.GetComponent<CharacterController2D>().stageTime <= 10f && scriptAchievments != null)
            {
                scriptAchievments.UnlockSteamAchievement("achiev7");
            }

            endGamePanel.SetActive(true);
        } else
        {
            SceneManager.LoadScene(nextScene);

            SaveProgress(nextScene);
        }
    }

    private void CheckMusicTheme(int sceneIndex)
    {
        switch(sceneIndex)
        {
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
                if(theme1 == null)
                {
                    theme2 = theme3 = theme4 = theme5 = theme6 = null;
                    theme1 = Resources.Load<AudioClip>("Sounds/Musics/Theme_1");
                    soundController.PlayWithLoop(theme1);
                }
                break;
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
            case 18:
                if (theme2 == null)
                {
                    theme1 = theme3 = theme4 = theme5 = theme6 = null;
                    theme2 = Resources.Load<AudioClip>("Sounds/Musics/Theme_2");
                    soundController.PlayWithLoop(theme2);
                }
                break;
            case 19:
            case 20:
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
                if (theme3 == null)
                {
                    theme1 = theme2 = theme4 = theme5 = theme6 = null;
                    theme3 = Resources.Load<AudioClip>("Sounds/Musics/Theme_3");
                    soundController.PlayWithLoop(theme3);
                }
                break;
            case 26:
            case 27:
            case 28:
            case 29:
            case 30:
            case 31:
            case 32:
                if (theme4 == null)
                {
                    theme1 = theme2 = theme3 = theme5 = theme6 = null;
                    theme4 = Resources.Load<AudioClip>("Sounds/Musics/Theme_4");
                    soundController.PlayWithLoop(theme4);
                }
                break;
            case 33:
            case 34:
            case 35:
            case 36:
            case 37:
            case 38:
            case 39:
                if (theme5 == null)
                {
                    theme1 = theme2 = theme3 = theme4 = theme6 = null;
                    theme5 = Resources.Load<AudioClip>("Sounds/Musics/Theme_5");
                    soundController.PlayWithLoop(theme5);
                }
                break;
            case 40:
            case 41:
            case 42:
            case 43:
            case 44:
            case 45:
            case 46:
                if (theme6 == null)
                {
                    theme1 = theme2 = theme3 = theme4 = theme5 = null;
                    theme6 = Resources.Load<AudioClip>("Sounds/Musics/Theme_6");
                    soundController.PlayWithLoop(theme6);
                }
                break;
        }
    }

    private void SaveProgress(int nextScene)
    {
        int progress = PlayerPrefs.GetInt("PlayerProgress");

        if (progress < nextScene && nextScene > 2)
        {
            PlayerPrefs.SetInt("PlayerProgress", nextScene);
        }
        if(scriptAchievments != null)
        {
            switch (nextScene)
            {
                case 10:
                    scriptAchievments.UnlockSteamAchievement("achiev1");
                    break;
                case 15:
                    scriptAchievments.UnlockSteamAchievement("achiev2");
                    break;
                case 20:
                    scriptAchievments.UnlockSteamAchievement("achiev3");
                    break;
                case 25:
                    scriptAchievments.UnlockSteamAchievement("achiev4");
                    break;
                case 30:
                    scriptAchievments.UnlockSteamAchievement("achiev5");
                    break;
            }
        }
    }

}
