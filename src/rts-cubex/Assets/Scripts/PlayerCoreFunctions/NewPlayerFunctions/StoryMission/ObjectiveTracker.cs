using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ObjectiveTracker : MonoBehaviour
{
    //NOTE: Add you loose state to the story mission and remove restart
    [Header("UI configuration")]
    [SerializeField] List<GameObject> objectivesTexts;
    [SerializeField] GameObject storyLooseWindow;
    [SerializeField] GameObject menuBtn;
    [SerializeField] Text looseDescriptionText;
    [Header("Main system cnf.:")]
    [SerializeField] float openScreenDelay = 5f;
    [SerializeField] bool levelPassed = false;
    [SerializeField] bool levelFailed = false;
    [SerializeField] string desiredUnitName = "zax";
    [SerializeField] string desiredBuildTag = "PlayerBase";
    [SerializeField] string lootBoxTag = "lootbox";
    [SerializeField] string tagTroopsToSave = "usave";
    [SerializeField] string armyCampTag = "ArmyCamp";
    [SerializeField] bool baseFounded = false;
    private GameObject proUnitSpawned;
    private GameObject playerBase;
    private bool proUnitFound = false;
    private bool buildSaved = false;
    private GameObject[] spawnedLootBoxes;
    private GameObject[] troopsToSave;
    private GameObject[] armyCampsToSave;
    private bool lootBoxesSpawned = false;
    private SLevelManager slm;
    [SerializeField] bool checkLootBoxes = false;
    [SerializeField] bool checkTroopsSaved = false;
    [SerializeField] bool checkArmyCamps = false;
    [SerializeField] int savedArmyCamps = 0;
    public bool CheckArmyCamps { set { checkArmyCamps = value; } get { return checkArmyCamps; }}
    public bool CheckLootBoxes { set { checkLootBoxes = value; } get { return checkLootBoxes; }}
    public bool CheckTroopsSaved { set { checkTroopsSaved = value; } get { return checkTroopsSaved; }}
    public bool BaseFounded { set {baseFounded = value; } get {return baseFounded; }}
    [SerializeField] bool checkIfBaseFound = true;
    void Start()
    {
        slm = GetComponent<SLevelManager>();

        if (checkArmyCamps) print("Checking army camps!");
        if (checkLootBoxes) print("Checking loot boxes!");
        if (checkTroopsSaved) print("Checking save troops!");
    }

    // Update is called once per frame
    void Update()
    {   
        //Check if player has won the game
        if (slm.CheckIfPlayerWon)
        {
            if (playerBase && proUnitSpawned)
            {
                //Open last dialogue
                var mDmgr = GetComponent<MissionDialogueMgr>();
                if (mDmgr)
                {
                    mDmgr.Act5Open = true; 
                }

                print("Player has passed the level! YOU WON!");
                objectivesTexts[0].GetComponent<Text>().text = "Objectives:\nDefend alpha base againts enemy waves[Passed]";
                levelPassed = true;
            }
        }
        //Setting the main leading hero
        if (!proUnitSpawned && !proUnitFound)
        {
            var allSpawnedUnits = GameObject.FindGameObjectsWithTag("Unit");
            if (allSpawnedUnits.Length > 0)
            {
                foreach (var a in allSpawnedUnits)
                {
                    if ((a.name.ToLower()).Contains(desiredUnitName))
                    {
                        print("The required main protogonist unit, is found!");
                        proUnitSpawned = a;
                        proUnitFound = !proUnitFound;

                        //Print objectives to player
                        objectivesTexts[0].SetActive(true);
                        objectivesTexts[0].GetComponent<Text>().text += "\n[Main]Find alpha base";

                        objectivesTexts[1].SetActive(true);
                        objectivesTexts[1].GetComponent<Text>().text = "[Bonus]Free discovered army camps";

                        objectivesTexts[2].SetActive(true);
                        objectivesTexts[2].GetComponent<Text>().text = "[Bonus]Save troop survivors";

                        objectivesTexts[3].SetActive(true);
                        objectivesTexts[3].GetComponent<Text>().text = "[Main]Don't let the main hero die";
                    }
                }
            }
        }
        if (!levelFailed && !levelPassed)
        {
            //Marking next objectives part
            if (checkIfBaseFound)
            {
                if (baseFounded)
                {                   
                    objectivesTexts[0].GetComponent<Text>().text = "Objectives:\n[Main]Find the stolen energon and credit boxes";
                    checkIfBaseFound = false;
                }
            }
            //Checking if main leading unit is not destroyed!
            if (!proUnitSpawned && proUnitFound)
            {
                print("Mission failed! The main leading hero is lost!");
                objectivesTexts[0].GetComponent<Text>().text = "Objectives:\n[Main]Defend alpha base againts enemy waves[Failed]";
                levelFailed = true;
                //After the leading unit died, restart the level
                //NOTE: Later make a you lose screen, and give the player a chance to restart level, or go to main menu
                //handleLevelEnd();
               
                objectivesTexts[3].GetComponent<Text>().text = "[Main]Don't let the main hero die[Failed]";
                handleLevelEndWithDesc("The main leading hero is lost! Next time don't let the main hero die...");
            }
            //Checking founded playerBase health
            playerBase = GameObject.FindGameObjectWithTag(desiredBuildTag);
            if (playerBase)
            {
                if (!buildSaved)
                {
                    objectivesTexts[0].GetComponent<Text>().text = "Objectives:\n[Main]Defend alpha base againts enemy waves";
                    print("Player base is saved! Checking health. NOTE: If player base HP=0, then level lost!");
                    buildSaved = true;
                }
                var healthMgr = playerBase.GetComponent<HealthOfRegBuilding>();
                if (healthMgr)
                {
                   if (healthMgr.getHealth() <= 0)
                   {
                       objectivesTexts[0].GetComponent<Text>().text = "Objectives:\n[Main]Defend alpha base againts enemy waves[Failed]";
                       print("Mission failed! The alpha base has been destroyed!");
                       levelFailed = true;
                         
                       handleLevelEndWithDesc("The alpha base has been destroyed! Next time prepeare stronger defences!");
                   }
                }
            }
           
            //Finding lootboxes and checking if they are destroyed
            if (checkLootBoxes)
            {
                if (!lootBoxesSpawned)
                {
                    spawnedLootBoxes = GameObject.FindGameObjectsWithTag(lootBoxTag);                
                    lootBoxesSpawned = spawnedLootBoxes.Length > 0;  
                    checkLootBoxes = true;              
                }
                if (checkLootBoxes)
                {
                    if (spawnedLootBoxes.Length <= 0)
                    {
                        print("All lot boxes are picked up to save the base!");
                        checkLootBoxes = false;
                        lootBoxesSpawned = false;
                        //Mark that all lootboxes are collected in the displayed text
                    }
                }
            }
             //Bonus objectives tracking
            //Check if all troops are saved!
            if (checkTroopsSaved)
            {
                troopsToSave = GameObject.FindGameObjectsWithTag(tagTroopsToSave);
                if (troopsToSave.Length <= 0)
                {
                    print("All lost troops are saved!");
                    objectivesTexts[2].SetActive(true);
                    objectivesTexts[2].GetComponent<Text>().text = "[Bonus]Save troop survivors[Done]";
                    checkTroopsSaved = false;
                }
                
            }
            //Check if army are camps needed to be saved
            if (checkArmyCamps)
            {
                armyCampsToSave = GameObject.FindGameObjectsWithTag(armyCampTag);
                if (armyCampsToSave.Length <= savedArmyCamps)
                {
                    print("All army camps are saved!");
                    objectivesTexts[1].SetActive(true);
                    objectivesTexts[1].GetComponent<Text>().text = "[Bonus]Free discovered army camps[DONE]";
                    checkArmyCamps = false; 
                }

                foreach (var a in armyCampsToSave)
                {
                    var aMgr = a.GetComponent<ArmyCamp>();
                    if (aMgr)
                    {
                        if (aMgr.Occupied > 0)
                        {
                            savedArmyCamps++;
                        }
                    }
                }
            }
        }
    }
    private void handleLevelEndWithDesc(string msg)
    {         
         StartCoroutine(openLooseWithDelay(msg));
    }
    IEnumerator openLooseWithDelay(string msg)
    {        
        yield return new WaitForSeconds(openScreenDelay);        
        looseDescriptionText.text = msg;
        storyLooseWindow.SetActive(true);
        menuBtn.SetActive(false);
        Time.timeScale = 0f;
    }
}
