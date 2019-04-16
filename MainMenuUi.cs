using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour 
{
    public static MainMenuUi mainMenuUi;

    public float volumeFactor;

    public Sprite availableStar, lockedStar, unlockedStar;
    public List<Image> additionStars, subtractionSTars, multiplicationStars, divisionStars, fractionsStars, timeStars, shapeStars, mesurementStars;
    public GameObject loadingScreen, earnedStarPanel, additionSubPanel, subtractionSubPanel, multiplicationSubPanel, divisionSubPanel, timeSubPanel, fractionsSubPanel, optionsPanel, creditsPanel, unlockAllPanel, resetAllDataPromptPanel, purchaseAppPanel;
    public GameObject processingPurchasePanel, purchaseCompletedPanel, purchaseFailedPanel, restorePurchaseButton, parentalGatePanel;
    public List<Button> additionSubPanelButtons, subtractionSubPanelButtons, multiplicationSubPanelButtons, divisionSubPanelButtons, timeSubPanelButtons, fractionSubPanelButtons;
    public List<Image> additionSubPanelImg, subtractionSubPanelImg, multiplicationSubPanelImg, divisionSubPanelImg, timeSubPanelImg, fractionSubPanelImg;
    public Text musicVolumeValueTxt, effectsVolumeValueTxt, unlockAllValueTxt;
    public Slider musicVolumeSlider, effectsVolumeSlider;
    public bool optionsOpen;
    public AudioSource musicAudioSource;
    public SaveClass savedData;
    public GameDataHandler gData;
    public string sceneName, unlockAllCode;

    public ParentalGateHandler parentalGateRef;
    //public IAPManager purchaserRef;

    private void Awake()
    {
        mainMenuUi = this;
    }


    // Use this for initialization
    void Start () 
	{
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            restorePurchaseButton.SetActive(true);
        }
        else restorePurchaseButton.SetActive(false);

        savedData = SaveLoad.saveLoad.savedData;
        musicVolumeSlider.value = savedData.musicVolume;
        musicAudioSource.volume = musicVolumeSlider.value / 100;
        additionSubPanel.SetActive(false);
        subtractionSubPanel.SetActive(false);
        multiplicationSubPanel.SetActive(false);
        gData = GameDataHandler.gDataHandler;
        if(gData.completedLevel == true)
        {
            gData.completedLevel = false;
            earnedStarPanel.SetActive(true);
            CheckStarEarned();
        }

        //Setting addition stars/subPanel
        SetStars(savedData.additionStarsEarned, additionStars, additionSubPanelButtons, additionSubPanelImg);
        //setting subtraction Stars/subPanel
        SetStars(savedData.subtractionStarsEarned, subtractionSTars, subtractionSubPanelButtons, subtractionSubPanelImg);
        //setting multiplication Stars/subPanel
        SetStars(savedData.multiplicationsStarEarned, multiplicationStars, multiplicationSubPanelButtons, multiplicationSubPanelImg);
        //setting Dibision Stars/subPanel
        SetStars(savedData.divisionStarEarned, divisionStars, divisionSubPanelButtons, divisionSubPanelImg);
        //setting Timne Stars/subPanel
        SetStars(savedData.timeStarEarned, timeStars, timeSubPanelButtons, timeSubPanelImg);
        //setting Fraction Stars/subpanel
        SetStars(savedData.fractionStarEarned, fractionsStars, fractionSubPanelButtons, fractionSubPanelImg);


        savedData.currentAdditionStage = savedData.additionStarsEarned + 1;
        savedData.currentSubtractionStage = savedData.subtractionStarsEarned + 1;
        savedData.currentMultiplicationStage = savedData.multiplicationsStarEarned + 1;
        //savedData.currentTimeStage = savedData.timeStarEarned + 1;
        //savedData.currentFractionStage = savedData.fractionStarEarned + 1;

	}
	
	// Update is called once per frame
	void Update () 
	{
        //musicAudioSource.volume = savedData.musicVolume / 100;
        if (optionsOpen)
        {
            musicVolumeValueTxt.text = musicVolumeSlider.value + "";
            effectsVolumeValueTxt.text = effectsVolumeSlider.value.ToString();
            musicAudioSource.volume = (musicVolumeSlider.value / 100) * volumeFactor;
        }


    }

    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);
        optionsOpen = true;
        musicVolumeSlider.value = savedData.musicVolume + ((1 - volumeFactor) * 100);
        effectsVolumeSlider.value = savedData.effectsVolume;
    }
    public void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);
        optionsOpen = false;
        savedData.musicVolume = (int)(musicVolumeSlider.value - ((1 - volumeFactor) * 100));
        savedData.effectsVolume = (int)effectsVolumeSlider.value;
        SaveLoad.saveLoad.Save();
    }
    public void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);
    }
    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }
    public void OpenUnlockAllPanel()
    {
        unlockAllPanel.SetActive(true);
    }
    public void CloseUnlockAllPanel()
    {
        unlockAllPanel.SetActive(false);
    }
    public void UnlockAll()
    {
        if(unlockAllValueTxt.text == unlockAllCode)
        {
            SaveLoad.saveLoad.UnlockAll();
            SaveLoad.saveLoad.Save();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ResetAllDataPrompt()
    {
        resetAllDataPromptPanel.SetActive(true);
    }
    public void ResetAllData(bool yesNo)
    {
        if (yesNo)
        {
            ResetProgress();
        }
        else
            resetAllDataPromptPanel.SetActive(false);
    }


    public void LoadSubject(int subjectId)
    {
        if(!SaveLoad.saveLoad.purchasedData.iapPurchased && subjectId > 1)
        {
            purchaseAppPanel.SetActive(true);
            parentalGateRef.ChooseQuestion();
            return;
        }

        switch (subjectId)
        {
            case 1:
                additionSubPanel.SetActive(true);
                break;

            case 2:
                subtractionSubPanel.SetActive(true);
                break;
            case 3:
                multiplicationSubPanel.SetActive(true);
                break;
            case 4:
                timeSubPanel.SetActive(true);
                break;
            case 5:
                fractionsSubPanel.SetActive(true);
                break;
            case 6:
                divisionSubPanel.SetActive(true);
                break;
        }
    }

    public void AttemptPurchase()
    {
        processingPurchasePanel.SetActive(true);
    }


    public void PurchaseGame()
    {
        processingPurchasePanel.SetActive(false);
        purchaseCompletedPanel.SetActive(true);
        SaveLoad.saveLoad.purchasedData.iapPurchased = true;
        //savedData.fullAppUnlocked = true;
        SaveLoad.saveLoad.SavePurchase();
        //purchaseAppPanel.SetActive(false);
    }

    public void PurchaseFailed()
    {
        purchaseFailedPanel.SetActive(true);
        processingPurchasePanel.SetActive(false);
    }

    public void ClosePurchaseAttemptPanels()
    {        
        purchaseFailedPanel.SetActive(false);
    }

    public void ClosePurchaseCompletedPanel()
    {
        purchaseCompletedPanel.SetActive(false);
        purchaseAppPanel.SetActive(false);
    }

    public void ClosePurchaseAppPanel()
    {
        purchaseAppPanel.SetActive(false);
    }


    public void LoadAdditionLevel()
    {
        if (savedData.additionStarsEarned > 0)
        {
            additionSubPanel.SetActive(true);
        }
        else
        {
            gData.newStage = true;
            loadingScreen.SetActive(true);
            //SceneManager.LoadScene("Addition Scene");
            sceneName = "Addition Scene";
            StartCoroutine(LoadNewScene());
        }
    }
    public void LoadAdditionLevelFromSubPanel(int stage)
    {
        savedData.currentAdditionStage = stage;
        if(stage <= savedData.additionStarsEarned)
        {
            savedData.currentAditionLevelSaved = savedData.currentAdditionLevel;
            savedData.currentAdditionLevel = 1;
            gData.newStage = false;
        }
        else
        {
            savedData.currentAdditionLevel = savedData.currentAditionLevelSaved;
            gData.newStage = true;
        }
        loadingScreen.SetActive(true);
        //SceneManager.LoadScene("Addition Scene");
        sceneName = "Addition Scene";
        StartCoroutine(LoadNewScene());
    }
    public void CloseAdditionSubPanel()
    {
        additionSubPanel.SetActive(false);
    }

    public void LoadSubtractionLevel()
    {
        
        if (savedData.subtractionStarsEarned > 0)
        {
            subtractionSubPanel.SetActive(true);
        }
        else
        {
            gData.newStage = true;
            loadingScreen.SetActive(true);
            //SceneManager.LoadScene("Subtraction Scene");
            sceneName = "Subtraction Scene";
            StartCoroutine(LoadNewScene());
        }
    }
    public void LoadSubtractionFromSubPanel(int stage)
    {
        savedData.currentSubtractionStage = stage;
        if (stage <= savedData.subtractionStarsEarned)
        {
            savedData.currentSubtractionLevelSaved = savedData.currentSubtractionLevel;
            savedData.currentSubtractionLevel = 1;
            gData.newStage = false;
        }
        else
        {
            savedData.currentSubtractionLevel = savedData.currentSubtractionLevelSaved;
            gData.newStage = true;
        }
        loadingScreen.SetActive(true);
        //SceneManager.LoadScene("Subtraction Scene");
        sceneName = "Subtraction Scene";
        StartCoroutine(LoadNewScene());
    }
    public void CloseSubtractionSubPanel()
    {
        subtractionSubPanel.SetActive(false);
    }

    public void LoadDivisionLevel()
    {
        if (savedData.divisionStarEarned > 0)
        {
            divisionSubPanel.SetActive(true);
        }
        else
        {
            gData.newStage = true;
            savedData.currentDivisionStage = 1;
            loadingScreen.SetActive(true);
            //SceneManager.LoadScene("DivisionScene");
            sceneName = "DivisionScene";
            StartCoroutine(LoadNewScene());
        }
    }

    public void LoadDivisionFromSubPanel(int stage)
    {
        savedData.currentDivisionStage = stage;
        if (stage <= savedData.divisionStarEarned)
        {
            savedData.currentDivisionLevelSaved = savedData.currentDivisionLevel;
            savedData.currentDivisionLevel = 1;
            gData.newStage = false;
        }
        else
        {
            savedData.currentDivisionLevel = savedData.currentDivisionLevelSaved;
            gData.newStage = true;
        }
        loadingScreen.SetActive(true);
        //SceneManager.LoadScene("DivisionScene");
        sceneName = "DivisionScene";
        StartCoroutine(LoadNewScene());
    }
    public void CloseDivisionSubPanel()
    {
        divisionSubPanel.SetActive(false);
    }

    public void LoadMultiplicationLevel()
    {
        if (savedData.multiplicationsStarEarned > 0)
        {
            multiplicationSubPanel.SetActive(true);
        }
        else
        {
            gData.newStage = true;
            loadingScreen.SetActive(true);
            //SceneManager.LoadScene("Multiplication Scene");
            sceneName = "Multiplication Scene";
            StartCoroutine(LoadNewScene());
        }
    }
    public void LoadMultiplicationFromSubPanel(int stage)
    {
        savedData.currentMultiplicationStage = stage;
        if (stage <= savedData.multiplicationsStarEarned)
        {
            savedData.currentMultiplicationLevelSaved = savedData.currentMultiplicationLevel;
            savedData.currentMultiplicationLevel = 1;
            gData.newStage = false;
        }
        else
        {
            savedData.currentMultiplicationLevel = savedData.currentMultiplicationLevelSaved;
            gData.newStage = true;
        }
        loadingScreen.SetActive(true);
        //SceneManager.LoadScene("Multiplication Scene");
        sceneName = "Multiplication Scene";
        StartCoroutine(LoadNewScene());
    }
    

    public void CloseMultiplicationSubPanel()
    {
        multiplicationSubPanel.SetActive(false);
    }

    
    public void LoadTimeScene()
    {
        //SceneManager.LoadScene("Time Scene");
        //sceneName = "Time Scene";
        //StartCoroutine(LoadNewScene());
        if (savedData.timeStarEarned > 0)
        {
            timeSubPanel.SetActive(true);
        }
        else
        {
            gData.newStage = true;
            savedData.currentTimeStage = 0;
            loadingScreen.SetActive(true);
            //SceneManager.LoadScene("DivisionScene");
            sceneName = "Time Scene";
            StartCoroutine(LoadNewScene());
        }
    }

    public void LoadTimeFromSubPanel(int stage)
    {
        
        savedData.currentTimeStage = stage;
        if (stage + 1 <= savedData.timeStarEarned)
        {
            savedData.currentTimeLevelSaved = savedData.currentTimeLevel ;
            savedData.currentTimeLevel = 0;
            if(stage == 7)
            {
                savedData.timeQuestionsChosen = new List<int>();
            }
            gData.newStage = false;
        }
        else
        {
            savedData.currentTimeLevel = savedData.currentTimeLevelSaved;
            gData.newStage = true;
        }
        loadingScreen.SetActive(true);
        sceneName = "Time Scene";
        StartCoroutine(LoadNewScene());
    }
    public void CloseTimeSubPanel()
    {
        timeSubPanel.SetActive(false);
    }
    public void LoadFractionScene()
    {
        
        if (savedData.fractionStarEarned > 0)
        {
            fractionsSubPanel.SetActive(true);
        }
        else
        {
            gData.newStage = true;
            savedData.currentFractionStage = 0;
            loadingScreen.SetActive(true);
            //SceneManager.LoadScene("DivisionScene");
            sceneName = "Fraction Scene";
            StartCoroutine(LoadNewScene());
        }
    }

    public void LoadFractionFromSubPanel(int stage)
    {

        savedData.currentFractionStage = stage;
        if (stage + 1 <= savedData.fractionStarEarned)
        {
            savedData.currentFractionLevelSaved = savedData.currentFractionLevel;
            savedData.currentFractionLevel = 0;
            if (stage == 7)
            {
                savedData.fractionQuestionChosen = new List<int>();
            }
            gData.newStage = false;
        }
        else
        {
            savedData.currentFractionLevel = savedData.currentFractionLevelSaved;
            gData.newStage = true;
        }
        loadingScreen.SetActive(true);
        sceneName = "Fraction Scene";
        StartCoroutine(LoadNewScene());
    }
    public void CloseFractionSubPanel()
    {
        fractionsSubPanel.SetActive(false);
    }

    public void SetStars(int starsEarned, List<Image> starsImg, List<Button> subPanelButtons, List <Image> subPanelImg)
    {
        ////////////////////////Setting the stars
        for (int i = 0; i < starsEarned; i++)
        {
            starsImg[i].enabled = true;
        }

        //////////////////////////Setting the sub panel
        for (int i = 0; i < subPanelButtons.Count; i++)
        {
            if (i <= starsEarned)
            {
                subPanelButtons[i].interactable = true;
                subPanelImg[i].sprite = availableStar;
                if (i == starsEarned)
                {
                    subPanelImg[i].sprite = unlockedStar;
                }
            }
            else
            {
                subPanelButtons[i].interactable = false;
                subPanelImg[i].sprite = lockedStar;
            }
        }
        
    }

    void CheckStarEarned()
    {
        if (savedData.currentAdditionStage - 1 > savedData.additionStarsEarned)
        {
            savedData.additionStarsEarned = savedData.currentAdditionStage - 1;
        }       
        if (savedData.currentSubtractionStage - 1 > savedData.subtractionStarsEarned)
        {
            savedData.subtractionStarsEarned = savedData.currentSubtractionStage - 1;
        }
        if (savedData.currentMultiplicationStage - 1 > savedData.multiplicationsStarEarned)
        {
            savedData.multiplicationsStarEarned = savedData.currentMultiplicationStage - 1;
        }
        if (savedData.currentDivisionStage - 1 > savedData.divisionStarEarned)
        {
            savedData.divisionStarEarned = savedData.currentDivisionStage - 1;
        }
        if (savedData.currentTimeStage - 1 > savedData.timeStarEarned)
        {
            savedData.timeStarEarned = savedData.currentTimeStage - 1;
        }
        if (savedData.currentFractionStage - 1 > savedData.fractionStarEarned)
        {
            savedData.fractionStarEarned = savedData.currentFractionStage - 1;
        }
        SaveLoad.saveLoad.Save();
    }

    //////////////////////////////////////DEBUG PORTION/////////////////////////////////////////////////
    public void ResetProgress()
    {
        SaveLoad.saveLoad.ResetValues();
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(0.1f);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
