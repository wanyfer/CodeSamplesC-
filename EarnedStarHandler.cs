using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarnedStarHandler : MonoBehaviour
{
    public GameDataHandler gData;
    SaveClass savedData;
    public Text text3;


    public float activeTime, activeTimer;

	// Use this for initialization
	void Start ()
    {
        gData = GameDataHandler.gDataHandler;
        savedData = SaveLoad.saveLoad.savedData;
        activeTimer = activeTime;
        if(gData.levelType == GameDataHandler.LevelTypes.addition)
        {
            text3.text = "Stage " + savedData.additionStarsEarned + " Addition";
        }
        if (gData.levelType == GameDataHandler.LevelTypes.subtraction)
        {
            text3.text = "Stage " + savedData.subtractionStarsEarned + " Subtraction";
        }
        if (gData.levelType == GameDataHandler.LevelTypes.multiplication)
        {
            text3.text = "Stage " + savedData.multiplicationsStarEarned + " Multiplication";
        }
        if (gData.levelType == GameDataHandler.LevelTypes.division)
        {
            text3.text = "Stage " + savedData.divisionStarEarned + " Division";
        }
        if (gData.levelType == GameDataHandler.LevelTypes.time)
        {
            text3.text = "Stage " + savedData.timeStarEarned + " Time";
        }
        if (gData.levelType == GameDataHandler.LevelTypes.fraction)
        {
            text3.text = "Stage " + savedData.fractionStarEarned + " Fraction";
        }


    }
	
	// Update is called once per frame
	void Update ()
    {
        activeTimer -= Time.deltaTime;
        if(activeTimer <= 0)
        {
            gameObject.SetActive(false);
        }
	}
}
