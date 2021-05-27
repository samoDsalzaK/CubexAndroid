using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Research : MonoBehaviour {
    [SerializeField] GameObject menu;
    [SerializeField] GameObject selectionCanvas;
    [SerializeField] ResearchConf oBGResearch;
    [Header ("Research info")]
    [SerializeField] int researchLevel;
    [SerializeField] int researchCost;
    [SerializeField] float spawnDelay;
    [SerializeField] int troopLevel;
    [SerializeField] int heavyTroopLevel;
    [SerializeField] int troopLevelCost;
    [SerializeField] int heavyLevelCost;
    [SerializeField] private int maxHP;
    [SerializeField] private int heavyMaxHP;
    [SerializeField] private int lightHealthIncrease;
    [SerializeField] private int heavyHealthIncrease;
    [SerializeField] private int lightDamageIncrease;
    [SerializeField] private int heavyDamageIncrease;
    [SerializeField] int damage;
    [SerializeField] int maxTroopLevel;
    [SerializeField] int maxHeavyTroopLevel;
    [SerializeField] int maxResearchLevel;
    [SerializeField] private float heavyTroopScalingCoef;
    [SerializeField] private float lightTroopScalingCoef;
    [Header ("Buttons")]
    [SerializeField] Button increaseButton;
    [SerializeField] Button troopIncreaseButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button lightTroopLevelButton;
    [SerializeField] Button heavyTroopLevelButton;
    private Base playerBase;
    [Header ("Panels")]
    [SerializeField] GameObject researchMenu;
    [SerializeField] GameObject troopSelectMenu;
    private bool isUpgraded = false;
    private bool isHeavyUpgraded = false;
    [Header ("Panel Code")]
    private openErrorPanel error;
    private TroopHealth lightHealth;
    private HeavyHealth heavyHealth;
    private HealthOfRegBuilding troopsResearchHealth;
    PanelManager panelManager;
    createAnimatedPopUp animatedPopUps;
    LocalPanelManager localPanelManager;
    public int ResearchLevel { get { return researchLevel; }}
    public ResearchConf OBGResearch {get { return oBGResearch; }}

    private void Start() {
    researchLevel = oBGResearch.getResearchLevel();
    researchCost = oBGResearch.getResearchCost();
    spawnDelay = oBGResearch.getSpawnDelay();
    troopLevel = oBGResearch.getTroopLevel();
    heavyTroopLevel = oBGResearch.getHeavyTroopLevel();
    troopLevelCost = oBGResearch.getLightLevelCost();
    heavyLevelCost = oBGResearch.getHeavyLevelCost();
    maxHP = oBGResearch.getMaxHP();
    heavyMaxHP = oBGResearch.getHeavyMaxHP();
    lightHealthIncrease = oBGResearch.getLightHealthIncrease();
    heavyHealthIncrease = oBGResearch.getHeavyHealthIncrease();
    lightDamageIncrease = oBGResearch.getLightDamageIncrease();
    heavyDamageIncrease = oBGResearch.getHeavyDamageIncrease();
    damage = oBGResearch.getDamage();
    maxTroopLevel = oBGResearch.getMaxLightTroopLevel();
    maxHeavyTroopLevel = oBGResearch.getMaxHeavyTroopLevel();
    maxResearchLevel = oBGResearch.getMaxResearchLevel();
    heavyTroopScalingCoef = oBGResearch.getHeavyTroopScalingCoef();
    lightTroopScalingCoef = oBGResearch.getLightTroopScalingCoef();
        if(FindObjectOfType<Base>() == null)
        {
           return;
        }
        else{
           playerBase = FindObjectOfType<Base>();
        }
        if(FindObjectOfType<openErrorPanel>() != null)
            error = FindObjectOfType<openErrorPanel>();
        troopsResearchHealth = GetComponent<HealthOfRegBuilding>();
        troopsResearchHealth.setHealthOfStructureOriginal(oBGResearch.getTroopResearchHealth());
        troopsResearchHealth.setHealth(oBGResearch.getTroopResearchHealth());
        panelManager = GetComponent<PanelManager>();
        animatedPopUps = playerBase.GetComponent<createAnimatedPopUp>();
        localPanelManager = GetComponent<LocalPanelManager>();
    }
    void Update () {
        if (!menu.activeSelf) {
            troopSelectMenu.SetActive (false);
        }
        if (isUpgraded) {
            oBGResearch.setLightDamageIncrease(lightDamageIncrease);
            var troops = FindObjectsOfType<TroopHealth> ();
            for (int i = 0; i < troops.Length; i++) {
                troops[i].setHP (lightHealthIncrease);
            }
            isUpgraded = false;
        }
        if (isHeavyUpgraded) {
           oBGResearch.setHeavyDamageIncrease(heavyDamageIncrease);
            var heavyTroops = FindObjectsOfType<HeavyHealth> ();
            for (int i = 0; i < heavyTroops.Length; i++) {
                heavyTroops[i].setHP (heavyHealthIncrease);
            }
            isHeavyUpgraded = false;
        }
        if(troopsResearchHealth.getHealth() <= 0)
        {
            playerBase.GetComponent<setFidexAmountOfStructures>().changePlayerTroopsResearchAmountInLevel = playerBase.GetComponent<setFidexAmountOfStructures>().changePlayerTroopsResearchAmountInLevel - 1;
            playerBase.GetComponent<setFidexAmountOfStructures>().changeBuildStructureButton(7);
            Destroy(gameObject);
        }
    }

    void OnMouseDown () {
        // check for active panels in this building hierarchy if yes do not trigger on mouse click
        var status = panelManager.checkForActivePanels();
        if (status){
            return;
        }  
        else{
            // set main window
            selectionCanvas.SetActive(true);
            openMenu ();    
            // deactivate other building panels
            panelManager.changeStatusOfAllPanels();
        }  
    }
    public void openMenu () {
        menu.SetActive (true);
    }
    IEnumerator Increase () {
        increaseButton.interactable = false;
        troopIncreaseButton.interactable = false;
        exitButton.interactable = false;
        lightTroopLevelButton.interactable = false;
        heavyTroopLevelButton.interactable = false;
        yield return new WaitForSeconds (spawnDelay);
        increaseButton.interactable = true;
        troopIncreaseButton.interactable = true;
        exitButton.interactable = true;
        lightTroopLevelButton.interactable = true;
        heavyTroopLevelButton.interactable = true;

    }
    public void increaseTroopLevel () {
        if (playerBase.getCreditsAmount () >= troopLevelCost) {
            if(oBGResearch.getTroopLevel()>maxTroopLevel-1){
                closeResearch ();
                error.openError();
                error.setText("You have reached the maximum light troop level");
                return;
            }
            if (oBGResearch.getTroopLevel() < 3) {
                animatedPopUps.createDecreaseCreditsPopUp(troopLevelCost);
                StartCoroutine (Increase ());
                playerBase.setCreditsAmount (playerBase.getCreditsAmount () - troopLevelCost);
                oBGResearch.setTroopLevel(1);
                isUpgraded = true;
                oBGResearch.setMaxHP(lightHealthIncrease);
                oBGResearch.setLightTroopScalingCoef(lightHealthIncrease);
                oBGResearch.setDamage(lightDamageIncrease);
            }
            else if ((oBGResearch.getTroopLevel() > 2) && (oBGResearch.getResearchLevel() < 1)) {
                closeResearch ();
                error.openError();
                error.setText("Not enough research level");
            }
            else if ((oBGResearch.getTroopLevel() > 5) && (oBGResearch.getResearchLevel() < 2)) {
                closeResearch ();
                error.openError();
                error.setText("Not enough research level");
            }
            else {
                animatedPopUps.createDecreaseCreditsPopUp(troopLevelCost);
                StartCoroutine (Increase ());
                playerBase.setCreditsAmount (playerBase.getCreditsAmount () - troopLevelCost);
                oBGResearch.setTroopLevel(1);
                isUpgraded = true;
                oBGResearch.setMaxHP(lightHealthIncrease);
                oBGResearch.setLightTroopScalingCoef(lightHealthIncrease);
                oBGResearch.setDamage(lightDamageIncrease);
            }
        } else {
            closeResearch ();
            error.openError();
            error.setText("Not enough credits");
        }
    }
    public void increaseHeavyTroopLevel () {
        if (playerBase.getCreditsAmount () >= heavyLevelCost) {
            if(oBGResearch.getHeavyTroopLevel()>maxHeavyTroopLevel-1){
                closeResearch ();
                error.openError();
                error.setText("You have reached the maximum heavy troop level");
                return;
            }
            if (oBGResearch.getHeavyTroopLevel() < 3) {
                if(oBGResearch.getResearchLevel()<1){
                    closeResearch ();
                error.openError();
                error.setText("Not enough research level");
                }
                else{
                StartCoroutine (Increase ());
                playerBase.setCreditsAmount (playerBase.getCreditsAmount () - heavyLevelCost);
                oBGResearch.setHeavyTroopLevel(1);
                isHeavyUpgraded = true;
                oBGResearch.setHeavyMaxHP(heavyHealthIncrease);
                oBGResearch.setHeavyTroopScalingCoef(heavyHealthIncrease);
                oBGResearch.setHeavyDamage(heavyDamageIncrease);
                }
            }
            else if ((oBGResearch.getHeavyTroopLevel()  > 2) && (oBGResearch.getResearchLevel()  < 1)) {
                closeResearch ();
                error.openError();
                error.setText("Not enough research level");
            }
            else if ((oBGResearch.getHeavyTroopLevel()  > 5) && (oBGResearch.getResearchLevel()  < 2)) {
                closeResearch ();
                error.openError();
                error.setText("Not enough research level");
            }
            else {
                StartCoroutine (Increase ());
                playerBase.setCreditsAmount (playerBase.getCreditsAmount () - heavyLevelCost);
                oBGResearch.setHeavyTroopLevel(1);
                isUpgraded = true;
                oBGResearch.setHeavyMaxHP(heavyHealthIncrease);
                oBGResearch.setHeavyTroopScalingCoef(heavyHealthIncrease);
                oBGResearch.setHeavyDamage(heavyDamageIncrease);
            }
        } else {
            closeResearch ();
            error.openError();
            error.setText("Not enough credits");
        }
    }
    public void increaseResearch () {
        if ((playerBase.getCreditsAmount () >= researchCost)&&(oBGResearch.getResearchLevel()<maxResearchLevel)) {
            // animatedPopUps.createDecreaseCreditsPopUp(researchCost);
            StartCoroutine (Increase ());
            playerBase.setCreditsAmount (playerBase.getCreditsAmount () - researchCost);
            oBGResearch.setResearchLevel(1);
            playerBase.GetComponent<PlayerScoring>().addScoreAfterStructureUpgrade("troopsResearch", oBGResearch.getResearchLevel());
            Debug.Log(oBGResearch.getResearchLevel());
        } 
        else if (oBGResearch.getResearchLevel()>=maxResearchLevel){
            closeResearch ();
            error.openError();
            error.setText("You have reached the maximum research level");
        }
        else {
            closeResearch ();
            error.openError();
            error.setText("Not enough credits");
        }
    }
    /*public int getResearch () {
        return researchLevel;
    }
    public int getTroopLevel () {
        return troopLevel;
    }
    public int getHeavyTroopLevel () {
        return heavyTroopLevel;
    }*/
    public void closeResearch () {
        researchMenu.SetActive (false);
    }
    /*public int getMaxHP () {
        return maxHP;
    }
    public int getHeavyMaxHP () {
        return heavyMaxHP;
    }
    public int getDamage () {
        return damage;
    }
    public float getLightTroopScalingCoef(){
        return lightTroopScalingCoef;
    }
    public float getHeavyTroopScalingCoef(){
        return heavyTroopScalingCoef;
    }*/
}