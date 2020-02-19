using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuOptions : MonoBehaviour
{

    public Text soundText;
    public static string soundConfig;
    public GameObject settingsPanel;
    public SoundController soundController;
    public GameObject controlsPanel;
    public GameObject instructionsPanel;
    public GameObject creditsPanel;
    public GameObject pauseMenuPanel;
    public GameObject resumeBtn;
    public bool fullScreen;
    public Toggle windowModeToggle;
    public Dropdown dropdownMenu;
    public bool gamePaused = false;
    public Animator animator;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public GameObject soundConfigButton;
    public AudioMixer audioMixer;

    private bool ps4Controller = false;
    private bool xboxOneController = true;

    // Start is called before the first frame update
    void Start()
    {
        RemoveResolutionsNotSupported();

        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
        GetPlayerSettings();

        if (soundConfig == null || soundConfig == "")
        {
            setAudioDefault();
        }

        SetButtonPreSelected("ButtonPlay");

    }

    private void RemoveResolutionsNotSupported()
    {
        if(dropdownMenu != null)
        {
            Resolution resolutionMax = Screen.resolutions[Screen.resolutions.Length - 1];

            if(resolutionMax.width < 1920)
            {
                dropdownMenu.options.RemoveAt(0);
            }

            if(resolutionMax.width < 1600)
            {
                dropdownMenu.options.RemoveAt(0);
            }

            if (resolutionMax.width < 1366)
            {
                dropdownMenu.options.RemoveAt(0);
            }

            if (resolutionMax.width < 1280)
            {
                dropdownMenu.options.RemoveAt(0);
            }
        }
    }

    private void GetPlayerSettings()
    {
        soundConfig = null;
        soundConfig = PlayerPrefs.GetString("Sound");

        if(soundConfigButton != null)
        {
            if (soundConfig == "On")
            {
                soundConfigButton.GetComponent<Image>().sprite = soundOnSprite;
            }
            else
            {
                soundConfigButton.GetComponent<Image>().sprite = soundOffSprite;
            }
            windowModeToggle.isOn = false;
        }

    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if(controlsPanel != null && controlsPanel.activeSelf)
            {
                CloseControlsPanel();
            } else if(settingsPanel != null && settingsPanel.activeSelf)
            {
                closeSettingsPanel();
            } else if(instructionsPanel != null && instructionsPanel.activeSelf)
            {
                CloseInstructionsPanel();
            }
        }

        CheckControllerConected();
    }

    public void changeAudio()
    {
        if(soundConfig == "On")
        {
            soundConfig = "Off";
            soundConfigButton.GetComponent<Image>().sprite = soundOffSprite;
            soundController.MuteAudio();
        } else
        {
            soundConfig = "On";
            soundConfigButton.GetComponent<Image>().sprite = soundOnSprite;
            soundController.PlayAudio();
        }

        PlayerPrefs.SetString("Sound", soundConfig);
    }

    /**
     * When audio preference is not set the default value is on
     */
    void setAudioDefault()
    {
        soundConfig = "On";
        PlayerPrefs.SetString("Sound", soundConfig);
        soundConfigButton.GetComponent<Image>().sprite = soundOnSprite;
        SetVolume(-10f);
    }

    public void closeSettingsPanel()
    {
        settingsPanel.SetActive(false);
        if (resumeBtn)
        {
            EventSystem.current.SetSelectedGameObject(resumeBtn);
        } else
        {
            SetButtonPreSelected("ButtonPlay");
        }
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);

        SetButtonPreSelected("Dropdown");
    }

    public void OpenControlsPanel()
    {
        controlsPanel.SetActive(true);
        settingsPanel.SetActive(false);
        SetButtonPreSelected("CloseBtn");
    }

    public void CloseControlsPanel()
    {
        controlsPanel.SetActive(false);
        OpenSettingsPanel();
    }

    public void CloseInstructionsPanel()
    {
        instructionsPanel.SetActive(false);
    }

    public void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);
        SetButtonPreSelected("CloseCreditsButton");
    }

    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
        SetButtonPreSelected("ButtonPlay");
    }

    public void OpenInstructionsPanel()
    {
        instructionsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CheckControllerConected()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            print(names.Length);
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                ps4Controller = true;
                xboxOneController = false;
            } else if (names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                ps4Controller = false;
                xboxOneController = true;
            }
            else if (names[x].Length == 0)
            {
                print("Default CONTROLLER IS CONNECTED");
                ps4Controller = false;
                xboxOneController = false;
            }
        }

        if (xboxOneController)
        {
            //Adicionar imagem dos controles do xbox
        }
        else if (ps4Controller)
        {
            //Adicionar imagem dos controles do ps4
        }
        else
        {
            //Adicionar imagem dos controles do teclado
        }
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if(Screen.fullScreen)
        {
            fullScreen = true;
        } else
        {
            fullScreen = false;
        }
    }

    public void ChangeResolution()
    {
        int menuIndex = dropdownMenu.value;
        string name = dropdownMenu.options[menuIndex].text;
        switch(name)
        {
            case "1920x1080":
                Screen.SetResolution(1920, 1080, true);
                break;
            case "1600x900":
                Screen.SetResolution(1600, 900, true);
                break;
            case "1366x768":
                Screen.SetResolution(1366, 768, true);
                break;
            case "1280x720":
                Screen.SetResolution(1280, 720, true);
                break;
            case "800x600":
                Screen.SetResolution(800, 600, true);
                break;
        }

        if(!fullScreen)
        {
            ToggleFullScreen();
        }
    }

    void SetButtonPreSelected(string buttonName)
    {
        GameObject FirstButton = GameObject.Find(buttonName);
        if (FirstButton)
        {
            EventSystem.current.SetSelectedGameObject(FirstButton);
        }
    }

    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        PlayerMovement player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        player.isGamePaused = false;

        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        if (resumeBtn)
        {
            EventSystem.current.SetSelectedGameObject(resumeBtn);
        }
        
        Time.timeScale = 0;
    }

    /**
     * Change to pause/unpause of the game
     */
    public void ChangeGameState()
    {
        gamePaused = !gamePaused;
        if(gamePaused)
        {
            PauseGame();
        } else
        {
            ResumeGame();
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);

        if(volume == -80)
        {
            soundConfig = "Off";
            soundConfigButton.GetComponent<Image>().sprite = soundOffSprite;
        } else
        {
            if (soundConfig == "Off") {
                soundConfig = "On";
                soundController.PlayAudio();
                PlayerPrefs.SetString("Sound", soundConfig);
            }

            soundConfigButton.GetComponent<Image>().sprite = soundOnSprite;
        }
        PlayerPrefs.SetString("Sound", soundConfig);
    }
}
