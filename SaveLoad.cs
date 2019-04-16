using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad saveLoad;
    public SavedPurchase purchasedData;
    public SaveClass savedData, emptyData, unlockedAllData;
    public int multiplicationTutorials;

    void Awake()
    {
        if (SaveLoad.saveLoad == null)
        {
            saveLoad = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (!Directory.Exists(Application.persistentDataPath + "/Save"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Save");
        }
        Load();

        

        if(savedData.isAdditionTutorialCompleted == null)
        {
            savedData.isAdditionTutorialCompleted = new List<bool>();
            savedData.isAdditionTutorialCompleted.Add(new bool());
            savedData.isAdditionTutorialCompleted.Add(new bool());
        }
        if(savedData.isSubtractionTutorialCompleted == null)
        {
            savedData.isSubtractionTutorialCompleted = new List<bool>();
            savedData.isSubtractionTutorialCompleted.Add(new bool());
        }
        if(savedData.isMultiplicationTutorialCompleted == null)
        {
            savedData.isMultiplicationTutorialCompleted = new List<bool>();
            savedData.isMultiplicationTutorialCompleted.Add(new bool());
            savedData.isMultiplicationTutorialCompleted.Add(new bool());
            savedData.isMultiplicationTutorialCompleted.Add(new bool());
        }
        if (savedData.isDivisionTutorialCompleted == null)
        {
            savedData.isDivisionTutorialCompleted = emptyData.isDivisionTutorialCompleted;
        }
        if(savedData.timeQuestionsChosen == null)
        {
            savedData.timeQuestionsChosen = new List<int>();
        }
    }

    

    public void Save()
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save/PlayerInfo.sav");

        bf.Serialize(file, savedData);
        file.Close();

    }

    public void SavePurchase()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save/PurchaseData.sav");

        bf.Serialize(file, purchasedData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Save/PlayerInfo.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Save/PlayerInfo.sav", FileMode.Open);
            savedData = (SaveClass)bf.Deserialize(file);
            file.Close();
        }

        if (File.Exists(Application.persistentDataPath + "/Save/PurchaseData.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Save/PurchaseData.sav", FileMode.Open);
            purchasedData = (SavedPurchase)bf.Deserialize(file);
            file.Close();
        }
    }

    public void ResetValues()
    {
        savedData = emptyData;
        Save();
    }

    public void UnlockAll()
    {
        unlockedAllData.musicOn = savedData.musicOn;
        unlockedAllData.effectsOn = savedData.effectsOn;
        unlockedAllData.effectsVolume = savedData.effectsVolume;
        unlockedAllData.musicVolume = savedData.musicVolume;
        savedData = unlockedAllData;
    }

}


[Serializable]
public class SaveClass
{
    public int musicVolume, effectsVolume;
    public int additionStarsEarned, currentAdditionLevel, currentAdditionStage, currentAditionLevelSaved;
    public int subtractionStarsEarned, currentSubtractionLevel, currentSubtractionStage, currentSubtractionLevelSaved;
    public int multiplicationsStarEarned, currentMultiplicationLevel, currentMultiplicationStage, currentMultiplicationLevelSaved;
    public int divisionStarEarned, currentDivisionLevel, currentDivisionStage, currentDivisionLevelSaved;
    public int timeStarEarned, currentTimeLevel, currentTimeStage, currentTimeLevelSaved;
    public int fractionStarEarned, currentFractionLevel, currentFractionStage, currentFractionLevelSaved;
    public List<int> multiplicationQuestionsChosen, timeQuestionsChosen, fractionQuestionChosen;
    public int mQIndexValue;
    public bool mStage6QChosen, mStage8QChosen;
    public bool isAdditionTutorialDone, isSubtractionTutorialDone, isMultiplicationTutorialDone;
    public List<bool> isAdditionTutorialCompleted, isSubtractionTutorialCompleted, isMultiplicationTutorialCompleted, isDivisionTutorialCompleted, isTimeTutorialCompleted, isFractionTutorialCompleted;
    public float soundValue;
    public bool musicOn, effectsOn, fullAppUnlocked;
}

[Serializable]
public class SavedPurchase
{
    public bool iapPurchased;
}
