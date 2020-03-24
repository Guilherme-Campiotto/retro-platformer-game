using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageSelectController : MonoBehaviour
{
    public List<Button> listButtons;
    public List<Image> listImagesLocked;
    public GameObject panelPageOne;
    public GameObject panelPageTwo;
    private int playerCurrentScene;

    // Start is called before the first frame update
    void Start()
    {
        GetPlayerProgress();
        SetImagesLockedStage();
        SetStageButtons();

        //PlayerPrefs.DeleteKey("PlayerProgress"); // apaga o progresso
        
        SetButtonPreSelected("ButtonIntro");
    }

    void GetPlayerProgress()
    {
        playerCurrentScene = PlayerPrefs.GetInt("PlayerProgress");
        
        if(playerCurrentScene == 0)
        {
            playerCurrentScene = 2;
            PlayerPrefs.SetInt("PlayerProgress", playerCurrentScene);
        }
    }

    void GoToStage(int stageIndex)
    {
        if(playerCurrentScene >= stageIndex)
        {
            SceneManager.LoadScene(stageIndex);
        }
    }

    void SetImagesLockedStage()
    {
        int sceneIndex = 2;
        foreach(Image lockedImage in listImagesLocked)
        {
            if(sceneIndex < playerCurrentScene)
            {
                Destroy(lockedImage.gameObject);
            }

            sceneIndex++;
        }
    }

    void SetStageButtons()
    {
        int btnIndex = 2;
        foreach (Button button in listButtons)
        {
            int currentIndex = btnIndex;
            button.onClick.AddListener(delegate { GoToStage(currentIndex); });
            btnIndex++;
        }
    }

    public void NextPage()
    {
        panelPageOne.SetActive(false);
        panelPageTwo.SetActive(true);

        SetButtonPreSelected("Button (24)");

    }

    public void PreviosPage()
    {
        panelPageOne.SetActive(true);
        panelPageTwo.SetActive(false);

        SetButtonPreSelected("ButtonIntro");

    }

    void SetButtonPreSelected(string buttonName)
    {
        GameObject FirstButton = GameObject.Find(buttonName);
        if (FirstButton)
        {
            EventSystem.current.SetSelectedGameObject(FirstButton);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
