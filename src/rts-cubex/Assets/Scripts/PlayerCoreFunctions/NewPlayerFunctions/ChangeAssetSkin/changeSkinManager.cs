using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class changeSkinManager : MonoBehaviour
{
    // skin ids
    // 1 - Starter
    // 2 - Pyro
    // 3 - Ice
    // 4 - Earth 
    // Start is called before the first frame update
    [Header("Configuration parameters for asset skin management")]
    /*-------------------------------------------*/
    // Pyro skin addtional boosts (in persantage)
    // +15 % troops damage
    // -10 % troops health
    [SerializeField] int increaseTroopsDamage;
    [SerializeField] int decreaseTroopsHealth;
    /*-------------------------------------------*/
    // Ice skin addtional boosts (in persantage)
    //-10 % buiding time
    //-20 % building health
    [SerializeField] int decreaseBuildingTime;
    [SerializeField] int decreaseBuildingHealth;
    /*-------------------------------------------*/
    // Earth skin additional boosts (in persantage)
    //+15 % building health
    //+15 % building time
    [SerializeField] int increaseBuildingHealth;
    [SerializeField] int increaseBuildingTime;

    [SerializeField] ResearchConf oBGResearch;

    [SerializeField] int LightTroopsAmountCount = 0; // counting spawned troops amount (Light), reason - one scriptable object
    [SerializeField] int HeavyTroopsAmountCount = 0; // counting spawned troops amount (Heavy)

    // Pop up panel, when player starts new level
    [SerializeField] GameObject skinSelectionPopUpOnLevelStart;
    [SerializeField] Text skinSelectionPopUpOnLevelStartText; 
    [SerializeField] Button skinSelectionPopUpOnLevelStartButton;
    [SerializeField] Image skinSelectionPopUpOnLevelStartImage;
    [SerializeField] Text btnText;

    [SerializeField] Sprite Pyro;
    [SerializeField] Sprite Ice;
    [SerializeField] Sprite Earth;

    // getter and setter for troop counters\
    // when upgrading troop level in research center, set this counter to 0!
    int changeLightTroopCount {set {LightTroopsAmountCount=value;} get {return LightTroopsAmountCount;}}
    int changeheavyTroopCount {set {HeavyTroopsAmountCount=value;} get {return HeavyTroopsAmountCount;}}
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // function for player skin management
    public void applyChosenSkin(GameObject gameObject){
        if(!PlayerPrefs.HasKey("skinSelection")){   // for the first time it will set default skin
            PlayerPrefs.SetInt("skinSelection", 1);
            PlayerPrefs.SetInt("previousSelection", 1);
            PlayerPrefs.SetString("skinName", "Starter");
        }
        int selectedSkinValue = PlayerPrefs.GetInt("skinSelection"); // grab selected skin value
        //Debug.Log(PlayerPrefs.GetString("skinName"));
        switch(selectedSkinValue)
        {
            case 1: 
                // Starter skin changer
                // no actions needed
                Debug.Log("Default asset skin applied");
                break;
            case 2:
                // Pyro skin changer
                if (gameObject.tag == "Unit" || gameObject.tag == "bloom"){
                    if(gameObject.GetComponent<TroopSkinManager>().returnTroopType == "Light"){ //to understand where to take troop damage points
                        if(LightTroopsAmountCount > 0){
                            int damageDec = PlayerPrefs.GetInt("LightDamage");
                            oBGResearch.setDamage(-damageDec); // reset damage value
                        }
                        LightTroopsAmountCount++;
                        Debug.Log("Previuos damage " + oBGResearch.getDamage());
                        float finalDamageToAdd = ((float)oBGResearch.getDamage() * (float)(increaseTroopsDamage/100f)); // find 15 % of existing troop damage
                        oBGResearch.setDamage((int)finalDamageToAdd); // add this value to troop damage points
                        if (PlayerPrefs.HasKey("LightDamage")){ // check if entry exists
                            PlayerPrefs.DeleteKey("LightDamage"); // delete entry
                        }
                        PlayerPrefs.SetInt("LightDamage", (int)finalDamageToAdd); // add damage
                        Debug.Log("Troop damage to add " + finalDamageToAdd);
                        Debug.Log("Modified troop damage " + oBGResearch.getDamage());
                    }
                    else if(gameObject.GetComponent<TroopSkinManager>().returnTroopType == "Heavy"){
                        if (HeavyTroopsAmountCount > 0){
                            int HeavyDamageDec = PlayerPrefs.GetInt("HeavyDamage");
                            oBGResearch.setHeavyDamage(-HeavyDamageDec); // reset damage value
                        }
                        HeavyTroopsAmountCount++;
                        Debug.Log("Previuos damage " + oBGResearch.getHeavyDamage());
                        float finalDamageToAdd = ((float)oBGResearch.getHeavyDamage() * (float)(increaseTroopsDamage/100f)); // find 15 % of existing troop damage
                        oBGResearch.setHeavyDamage((int)finalDamageToAdd); // add this value to troop damage points
                        if (PlayerPrefs.HasKey("HeavyDamage")){ // check if entry exists
                            PlayerPrefs.DeleteKey("HeavyDamage"); // delete entry
                        }
                        PlayerPrefs.SetInt("HeavyDamage", (int)finalDamageToAdd); // add damage
                        Debug.Log("Troop damage to add " + finalDamageToAdd);
                        Debug.Log("Modified troop damage " + oBGResearch.getHeavyDamage());
                    }
                    /*else if (gameObject.GetComponent<TroopSkinManager>().returnTroopType == "Sniper"){
                        // modify damage points for Sniper Unit
                    }  */
                    else{
                        Debug.Log("No Damage component found!");
                    }
                    /*else{
                        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
                        foreach (Transform t in ts) {
                            if(t.gameObject.GetComponent<TroopsDamage>() != null)
                            {
                                Debug.Log("Previuos damage " + gameObject.GetComponent<TroopsDamage>().GetDamage());
                                float finalDamageToAdd = (gameObject.GetComponent<TroopsDamage>().GetDamage() * (float)(increaseTroopsDamage/100f));
                                gameObject.GetComponent<TroopsDamage>().setDamage((int)finalDamageToAdd);
                                Debug.Log("Troop damage to add " + finalDamageToAdd);
                                Debug.Log("Modified troop damage " + gameObject.GetComponent<TroopsDamage>().GetDamage());
                            }
                        }
                    }*/
                    if (gameObject.GetComponent<TroopHealth>() != null){
                        Debug.Log("Previuos health " + gameObject.GetComponent<TroopHealth>().UnitHP);
                        float finalTroopHealthToAdd = (-1)*(gameObject.GetComponent<TroopHealth>().UnitHP * (float)(decreaseTroopsHealth/100f));
                        gameObject.GetComponent<TroopHealth>().setHP((int)finalTroopHealthToAdd);
                        Debug.Log("Troop health to add " + finalTroopHealthToAdd);
                        Debug.Log("Modified troop health " + gameObject.GetComponent<TroopHealth>().UnitHP);
                        break;
                    }
                    else if (gameObject.GetComponent<HeavyHealth>() != null){
                        Debug.Log("Previuos health " + gameObject.GetComponent<HeavyHealth>().UnitHP);
                        float finalTroopHealthToAdd = (-1)*(gameObject.GetComponent<HeavyHealth>().UnitHP * (float)(decreaseTroopsHealth/100f));
                        gameObject.GetComponent<HeavyHealth>().setHP((int)finalTroopHealthToAdd);
                        Debug.Log("Troop health to add " + finalTroopHealthToAdd);
                        Debug.Log("Modified troop health " + gameObject.GetComponent<HeavyHealth>().UnitHP);
                        break;
                    }
                    else{
                        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
                        foreach (Transform t in ts) {
                            if(t.gameObject.GetComponent<TroopHealth>() != null)
                            {
                                Debug.Log("Previuos health " + gameObject.GetComponent<TroopHealth>().UnitHP);
                                float finalTroopHealthToAdd = (-1)*(gameObject.GetComponent<TroopHealth>().UnitHP * (float)(decreaseTroopsHealth/100f));
                                gameObject.GetComponent<TroopHealth>().setHP((int)finalTroopHealthToAdd);
                                Debug.Log("Troop health to add " + finalTroopHealthToAdd);
                                Debug.Log("Modified troop health " + gameObject.GetComponent<TroopHealth>().UnitHP);
                                break;
                            }
                        }
                        Debug.Log("There is no health component assigned to troop");
                    }
                }
                else if (gameObject.tag == "PlayerBase"){
                    //0F29F3
                    //set new color to playerbase
                    Color changeColour = new Color32(255,181, 0, 231); // orange
                    if(gameObject.GetComponent<Renderer>() != null){
                        gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
                    }
                    else{
                        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
                        foreach (Transform t in ts) {
                            if(t.gameObject.GetComponent<Renderer>() != null)
                            {
                                t.gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
                            }
                        }
                    }
                }
                Debug.Log("Pyro asset skin applied");
                break;
            case 3:
                if (gameObject.tag != "Unit" || gameObject.tag != "bloom"){ // check if tag is not player troops
                    if (gameObject.tag == "PlayerBase"){
                        //set new color to playerbase
                        Color changeColour = new Color32(0,198, 255, 255); // light blue
                        if(gameObject.GetComponent<Renderer>() != null){
                            gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
                        }
                        else{
                            Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
                            foreach (Transform t in ts) {
                                if(t.gameObject.GetComponent<Renderer>() != null)
                                {
                                    t.gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
                                }
                            }
                        }
                        // change player base health
                        Debug.Log(gameObject.tag + " Previuos building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                        float finalHealthToAdd = (-1)*(gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() * (float)(decreaseBuildingHealth/100f));
                        gameObject.GetComponent<HealthOfRegBuilding>().setHealthOfStructureOriginal(gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() + (int)finalHealthToAdd);
                        gameObject.GetComponent<HealthOfRegBuilding>().setHealth(gameObject.GetComponent<HealthOfRegBuilding>().getHealth() + (int)finalHealthToAdd);
                        Debug.Log(gameObject.tag + " Building health to add " + finalHealthToAdd);
                        Debug.Log(gameObject.tag + " Modified building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                        break;
                    }
                    else if (gameObject.tag == "Worker"){
                        // change building time
                        for(int i = 0; i < gameObject.GetComponent<TimerForSpawningOriginal>().changeBuildingTimers.Length; i++){
                            float timeToDecrease = gameObject.GetComponent<TimerForSpawningOriginal>().changeBuildingTimers[i] * (float)(decreaseBuildingTime/100f);
                            gameObject.GetComponent<TimerForSpawningOriginal>().changeBuildingTimers[i] = (float)Math.Floor((gameObject.GetComponent<TimerForSpawningOriginal>().changeBuildingTimers[i] - timeToDecrease));
                        }
                        break;
                    }
                    else{
                        // change other game structures health
                        if (gameObject.GetComponent<HealthOfRegBuilding>() != null){
                            Debug.Log(gameObject.tag + " Previuos building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                            float finalHealthToAdd = (-1)*(gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() * (float)(decreaseBuildingHealth/100f));
                            gameObject.GetComponent<HealthOfRegBuilding>().setHealthOfStructureOriginal(gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() + (int)finalHealthToAdd);
                            gameObject.GetComponent<HealthOfRegBuilding>().setHealth(gameObject.GetComponent<HealthOfRegBuilding>().getHealth() + (int)finalHealthToAdd);
                            Debug.Log(gameObject.tag + " Building health to add " + finalHealthToAdd);
                            Debug.Log(gameObject.tag + " Modified building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                            break;
                        }
                        else{
                            Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>(); 
                            foreach (Transform t in ts) {
                                if(t.gameObject.GetComponent<HealthOfRegBuilding>() != null)
                                {
                                    Debug.Log(gameObject.tag + " Previuos building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                                    float finalHealthToAdd = (-1)*(gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() * (float)(decreaseBuildingHealth/100f));
                                    gameObject.GetComponent<HealthOfRegBuilding>().setHealthOfStructureOriginal(gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() + (int)finalHealthToAdd);
                                    gameObject.GetComponent<HealthOfRegBuilding>().setHealth(gameObject.GetComponent<HealthOfRegBuilding>().getHealth() + (int)finalHealthToAdd);
                                    Debug.Log(gameObject.tag + " Building health to add " + finalHealthToAdd);
                                    Debug.Log(gameObject.tag + " Modified building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                                    break;
                                }
                                else if(t.gameObject.GetComponent<TurretHealth>() != null){
                                    Debug.Log(gameObject.tag + " Previuos building health " + gameObject.GetComponent<TurretHealth>().getTurretHealth());
                                    float finalHealthToAdd = (-1)*(gameObject.GetComponent<TurretHealth>().getTurretHealth() * (float)(decreaseBuildingHealth/100f));
                                    gameObject.GetComponent<TurretHealth>().setTurretHealth(gameObject.GetComponent<TurretHealth>().getTurretHealth() + (int)finalHealthToAdd);
                                    gameObject.GetComponent<TurretHealth>().setHP(gameObject.GetComponent<TurretHealth>().getTurretHealth() + (int)finalHealthToAdd);
                                    Debug.Log(gameObject.tag + " Building health to add " + finalHealthToAdd);
                                    Debug.Log(gameObject.tag + " Modified building health " + gameObject.GetComponent<TurretHealth>().getTurretHealth());
                                    break;
                                }
                            }
                        }
                    }
                    Debug.Log("Ice asset skin applied");
                }
                break;
            case 4:
                if (gameObject.tag != "Unit" || gameObject.tag != "bloom"){
                    // Earth skin changer 
                    if (gameObject.tag == "PlayerBase"){
                        //set new color to playerbase
                        Color changeColour = new Color32(34,140, 0, 255); // green
                        if(gameObject.GetComponent<Renderer>() != null){
                            gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
                        }
                        else{
                            Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
                            foreach (Transform t in ts) {
                                if(t.gameObject.GetComponent<Renderer>() != null)
                                {
                                    t.gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
                                }
                            }
                        }
                        // change player base health
                        Debug.Log(gameObject.tag + " Previuos building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                        float finalHealthToAdd = (gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() * (float)(increaseBuildingHealth/100f));
                        gameObject.GetComponent<HealthOfRegBuilding>().setHealthOfStructureOriginal(gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() + (int)finalHealthToAdd);
                        gameObject.GetComponent<HealthOfRegBuilding>().setHealth(gameObject.GetComponent<HealthOfRegBuilding>().getHealth() + (int)finalHealthToAdd);
                        Debug.Log(gameObject.tag + " Building health to add " + finalHealthToAdd);
                        Debug.Log(gameObject.tag + " Modified building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                        break;
                    }
                    else if (gameObject.tag == "Worker"){
                        // change building time
                        for(int i = 0; i < gameObject.GetComponent<TimerForSpawningOriginal>().changeBuildingTimers.Length; i++){
                            float timeToIncrease = gameObject.GetComponent<TimerForSpawningOriginal>().changeBuildingTimers[i] * (float)(increaseBuildingTime/100f);
                            gameObject.GetComponent<TimerForSpawningOriginal>().changeBuildingTimers[i] = (float)Math.Floor((gameObject.GetComponent<TimerForSpawningOriginal>().changeBuildingTimers[i] + timeToIncrease));
                        }
                        break;
                    }
                    else{
                        // change building health
                        if (gameObject.GetComponent<HealthOfRegBuilding>() != null){
                            Debug.Log(gameObject.tag + " Previuos building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                            float finalHealthToAdd = gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() * (float)(increaseBuildingHealth/100f);
                            gameObject.GetComponent<HealthOfRegBuilding>().setHealthOfStructureOriginal(gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() + (int)finalHealthToAdd);
                            gameObject.GetComponent<HealthOfRegBuilding>().setHealth(gameObject.GetComponent<HealthOfRegBuilding>().getHealth() + (int)finalHealthToAdd);
                            Debug.Log(gameObject.tag + " Building health to add " + finalHealthToAdd);
                            Debug.Log(gameObject.tag + " Modified building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                            break;
                        }
                        else{
                            Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>(); 
                            foreach (Transform t in ts) {
                                if(t.gameObject.GetComponent<HealthOfRegBuilding>() != null)
                                {
                                    Debug.Log(gameObject.tag + " Previuos building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                                    float finalHealthToAdd = gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() * (float)(increaseBuildingHealth/100f);
                                    gameObject.GetComponent<HealthOfRegBuilding>().setHealthOfStructureOriginal(gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal() + (int)finalHealthToAdd);
                                    gameObject.GetComponent<HealthOfRegBuilding>().setHealth(gameObject.GetComponent<HealthOfRegBuilding>().getHealth() + (int)finalHealthToAdd);
                                    Debug.Log(gameObject.tag + " Building health to add " + finalHealthToAdd);
                                    Debug.Log(gameObject.tag + " Modified building health " + gameObject.GetComponent<HealthOfRegBuilding>().getHealthOfStructureOriginal());
                                    break;
                                }
                                else if(t.gameObject.GetComponent<TurretHealth>() != null){
                                    Debug.Log(gameObject.tag + " Previuos building health " + gameObject.GetComponent<TurretHealth>().getTurretHealth());
                                    float finalHealthToAdd = gameObject.GetComponent<TurretHealth>().getTurretHealth() * (float)(increaseBuildingHealth/100f);
                                    gameObject.GetComponent<TurretHealth>().setTurretHealth(gameObject.GetComponent<TurretHealth>().getTurretHealth() + (int)finalHealthToAdd);
                                    gameObject.GetComponent<TurretHealth>().setHP(gameObject.GetComponent<TurretHealth>().getTurretHealth() + (int)finalHealthToAdd);
                                    Debug.Log(gameObject.tag + " Building health to add " + finalHealthToAdd);
                                    Debug.Log(gameObject.tag + " Modified building health " + gameObject.GetComponent<TurretHealth>().getTurretHealth());
                                    break;
                                }
                            }
                        }
                    }
                    Debug.Log("Earth asset skin applied");
                }
                break;
            default:
                break;
        }
        return;
    }

    // function for pop up window to inform player about skin selection
    public void onStartSkinSelectionPopUp(){
        int selectedSkinValue = PlayerPrefs.GetInt("skinSelection"); // grab selected skin value
        string skinName = PlayerPrefs.GetString("skinName"); // getting selected skin name
        Color btnColour;
        switch(selectedSkinValue){
            case 1:
                // Default
                skinSelectionPopUpOnLevelStartText.text = "You have selected " + skinName + " asset skin" + "\n" + "\n" + "This asset doesn't provide any boosts"; // setting panel text
                btnColour = new Color32(15,41, 243, 255);
                skinSelectionPopUpOnLevelStartImage.GetComponent<UnityEngine.UI.Image>().color  = btnColour;
                btnText.text = "Okey";
                skinSelectionPopUpOnLevelStart.SetActive(true); // pop uping panel
                Time.timeScale = 0; // stopping game
                break;
            case 2:
                // Pyro
                skinSelectionPopUpOnLevelStartImage.GetComponent<UnityEngine.UI.Image>().sprite = Pyro;
                skinSelectionPopUpOnLevelStartText.text = "You have selected " + skinName + " asset skin" + "\n" + "You get :" + "\n" + "+" + increaseTroopsDamage + " % troops damage" + "\n" + "-" + decreaseTroopsHealth + " % troops health";
                btnText.text = "Okey";
                skinSelectionPopUpOnLevelStart.SetActive(true);
                Time.timeScale = 0;
                break;
            case 3:
                // Ice
                skinSelectionPopUpOnLevelStartImage.GetComponent<UnityEngine.UI.Image>().sprite = Ice;
                skinSelectionPopUpOnLevelStartText.text = "You have selected " + skinName + " asset skin" + "\n" + "You get :" + "\n" + "-" + decreaseBuildingTime + " % building time" + "\n" + "-" + decreaseBuildingHealth + " % building health";
                btnText.text = "Okey";
                skinSelectionPopUpOnLevelStart.SetActive(true);
                Time.timeScale = 0;
                break;
            case 4:
                // Earth
                skinSelectionPopUpOnLevelStartImage.GetComponent<UnityEngine.UI.Image>().sprite = Earth;
                skinSelectionPopUpOnLevelStartText.text = "You have selected " + skinName + " asset skin" + "\n" + "You get :" + "\n" +  "+" + increaseBuildingTime + " % building time" + "\n" + "+" + increaseBuildingHealth + " % building health";
                btnText.text = "Okey";
                skinSelectionPopUpOnLevelStart.SetActive(true);
                Time.timeScale = 0;
                break;
            default:
                break;
        }
	}
}
