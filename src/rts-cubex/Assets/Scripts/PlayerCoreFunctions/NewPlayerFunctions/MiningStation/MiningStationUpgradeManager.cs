using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningStationUpgradeManager : MonoBehaviour
{
    [Header("Upgrade Manager configuration parameters")]
    // upgrade button
    [SerializeField] Button upgradeBtn;
    [SerializeField] Text upgradeBtnText;
    [SerializeField] Text miningStationLevelText;
    [SerializeField] Text miningStationHealthText;

    [SerializeField] int miningStationLevel;
    [SerializeField] int miningStationMaxLevel;
    [SerializeField] int[] creditsNeededForUpgrade;
    [SerializeField] int[] energonNeededForUpgrade;
    [SerializeField] int[] upgradeHpAmount;
    
    // research centre type variable 
    private ResearchLevel researchLevel;

    // playerbase type variable
    private Base playerbase;

    // Health type variable
    private HealthOfRegBuilding miningStationHealth;

    // button handler type varibale
    private buttonHadler button_Hadler;

    private CreditsMiningStation miningStation;

    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectOfType<Base>() == null)
        {
           return;
        }
        else
        {
           playerbase = FindObjectOfType<Base>();
        }
        researchLevel = FindObjectOfType<ResearchLevel>();
        miningStationHealth = GetComponent<HealthOfRegBuilding>();
        button_Hadler = GetComponent<buttonHadler>();
        miningStation = GetComponent<CreditsMiningStation>();
        upgradeBtnText.text = "Upgrade mining station to level " + (miningStationLevel + 1) + "\n" + "(" + creditsNeededForUpgrade[0] + " credits & " + energonNeededForUpgrade[0] + " energon)";
        miningStationLevelText.text = "Mining station level : " + miningStationLevel;
        miningStationHealthText.text = "Mining station health : " + miningStationHealth.getHealth() + " / " + miningStationHealth.getHealthOfStructureOriginal();
    }

    // Update is called once per frame
    void Update()
    {
        miningStationHealthText.text = "Mining station health : " + miningStationHealth.getHealth() + " / " + miningStationHealth.getHealthOfStructureOriginal();
    }
    
    public void updateText(){
        if(miningStationLevel == miningStationMaxLevel) // change button text when max level is reached
        {
            upgradeBtnText.text = "You have reached max level";
            Color btnColour = new Color32(255, 0, 5, 255);
            upgradeBtn.GetComponent<UnityEngine.UI.Image>().color = btnColour;
            upgradeBtn.interactable = false;
        }
        else {
            upgradeBtnText.text = "Upgrade mining station to level " + (miningStationLevel + 1) + "\n" + "(" + creditsNeededForUpgrade[miningStationLevel-1] + " credits & " + energonNeededForUpgrade[miningStationLevel-1] + " energon)";
        }
        miningStationLevelText.text = "Mining station level : " + miningStationLevel;
    }
    
    public void upgradeAction(){
        bool state = false;
        // max level of research center = 5
        // max level of mining station = 3
        // check if research center is already build on the map
        if(researchLevel == null)
        {    
        playerbase.setErrorStateToBuildStructure(true);
        return;
        }
        // check if building already reached max level
        if(miningStationLevel == miningStationMaxLevel)
        {
           return; 
        }
        state = canUpgrade();
        if (state){
             // check for available resources
            if(playerbase.getCreditsAmount() < creditsNeededForUpgrade[miningStationLevel-1] || playerbase.getEnergonAmount() < energonNeededForUpgrade[miningStationLevel-1])
            {
                playerbase.setResourceAMountScreenStateForUpgrade(true); 
                return;
            }
            // change credits/energon values after upgrade
            playerbase.setCreditsAmount(playerbase.getCreditsAmount() - creditsNeededForUpgrade[miningStationLevel-1]);
            playerbase.setEnergonAmount(playerbase.getEnergonAmount() - energonNeededForUpgrade[miningStationLevel-1]);
            miningStationHealth.setHealthOfStructureOriginal(miningStationHealth.getHealthOfStructureOriginal() + upgradeHpAmount[miningStationLevel-1]);
            // increase structure level
            miningStationLevel++;
            // unlocking next credits mining slot 
            button_Hadler.unlockBtn(miningStationLevel);
            button_Hadler.setButtonTextBack(miningStationLevel, miningStation.changeTimeNeedForMining[miningStationLevel-1]);
            updateText();
        }
        else {
            playerbase.setErrorStateForResearchCenter(true);
            return;
        }
    }

    public bool canUpgrade(){
        // depending on level of research center manage upgrades of mining station
        switch (researchLevel.getBaseResearchLevel()) 
        {
            // start upgrading mining station can be possible at least having research level = 2
            case 2:
                if(miningStationLevel == 1){
                    return true;
                }
                else{
                    return false;
                }
            break;
            case 3:
                // this will allow player to upgrade mining station if he skiiped upgrading it when research level = 2
                if(miningStationLevel == 1){
                    return true;
                }
                else {
                    return false;
                }
            break;
            case 4:
                if(miningStationLevel == 1 || miningStationLevel == 2){
                    return true;
                }
                else{
                    return false;
                }
            break;
            case 5:
                if(miningStationLevel == 1 || miningStationLevel == 2){
                    return true;
                }
                else{
                    return false;
                }
            break;
        }
        return false;
    }
}
