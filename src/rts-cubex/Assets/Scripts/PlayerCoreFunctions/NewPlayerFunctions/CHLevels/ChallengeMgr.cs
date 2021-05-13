﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class ChallengeMgr : MonoBehaviour
{
    [Header("Main challenge level config parameters")]
    [SerializeField] bool isOpeningTextOpened = false;
    [SerializeField] float printDelay = 3f;
    [SerializeField] GameObject matchGreatText;
    [SerializeField] bool levelEnd = false;
    [SerializeField] GameObject levelEndWindow;
    [SerializeField] List<string> allPlayerBTags;
    [SerializeField] Text scoreText;
    [SerializeField] Text rankText;
    [SerializeField] GameObject menuButton;
    [SerializeField] bool challengeLevel = true; //For testing
    [SerializeField] GameObject enemyUnit;
    [SerializeField] int enemyGroupSize = 3;
    private MapMaker maker;
    private List<List<GameObject>> platformCubes; //Platform must be 6x6
    private bool playerBaseSpawned = false;
    [SerializeField] bool levelTick = false;
    [SerializeField] bool attackStarted = false;
    [SerializeField] bool readyingForAttack = false;    
    [SerializeField] float enemyReadyTime = 30f;
    [SerializeField] float enemyAttackTime = 30f;
    [SerializeField] int waveAmount = 3;
    [SerializeField] GameObject actionWindow;
    [SerializeField] Text actionText;

    [Header("Player score")]
    [SerializeField] int enemiesKilled = 0;
    [SerializeField] int buildingsBuilt = 0;
    [SerializeField] int troopsTrained = 0;

    public int EnemiesKilled { set {enemiesKilled = 0; } get {return enemiesKilled;}}
    public int BuildingsBuilt { set {buildingsBuilt = 0; } get {return buildingsBuilt;}}
    public int TroopsTrained { set {troopsTrained = 0; } get {return troopsTrained; }}

    private TaskTimer tt;
    private GameObject spawnPlayerBase;
    void Start()
    {
        maker = GetComponent<MapMaker>();
        tt = GetComponent<TaskTimer>();
        if (challengeLevel)
            actionWindow.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (challengeLevel)
        {
            if (!isOpeningTextOpened)
            {
                printMatchText();
                isOpeningTextOpened = true;
            }
            
                     
            //Initializing the platform variable
            if (platformCubes == null && maker.SCubes != null)
            {
                platformCubes = maker.SCubes;
            }
            if (maker.ReadyToBuildChallenge)
            {
                //Spawning the player
                if (!playerBaseSpawned)
                {
                    if (maker.SPlayerBase)
                    {
                        var sPlayerBase = maker.SPlayerBase;
                        if (maker.GameHood)
                            maker.GameHood.SetActive(true);
                        else
                            Debug.LogWarning("WARNING! Game hood not assigned!");

                        if (!maker.MainCamera)
                        {
                            Debug.LogError("ERROR: Camera prefab not added to MapMaker! Stopping CH level build!");
                            challengeLevel = false;
                        }

                        var middleLaneIndex = (platformCubes.Count - 1) / 2;
                        var middleLaneIndexPos = middleLaneIndex;
                        var pCube = platformCubes[middleLaneIndex][middleLaneIndexPos];
                        var spawnPos = pCube.transform.position;
                        
                        var yPosOffset = pCube.transform.localScale.y + 2f;
                        spawnPlayerBase = Instantiate(sPlayerBase, spawnPos + new Vector3(0f, yPosOffset, 0f), sPlayerBase.transform.rotation);

                        var mainCamera = maker.MainCamera;
                        mainCamera.transform.position = new Vector3(spawnPlayerBase.transform.position.x, mainCamera.transform.position.y, spawnPlayerBase.transform.position.z);
                
                        if (spawnPlayerBase)
                        {
                            print("Player Base is spawned and ready for battle!");
                            playerBaseSpawned = true;
                        }
                    }
                    else
                    {
                        Debug.LogError("ERROR: PlayerBase prefab not added to MapMaker! Stopping CH level build!");
                        challengeLevel = false;
                    }
                }
            //Checking main objective
            //Checking level pass state
            if (waveAmount <= 0 || !spawnPlayerBase)
            {
               print("[Match MGR] Game Over!");
               print(!spawnPlayerBase ? "Player base has been destroyed" : "Player base has been saved!");
               print(waveAmount <= 0 ? "Player has survived all the enemy waves" : "Player has not survived all the enemy waves");
               
               challengeLevel = false;               
                //Destryoing remaing enemies

                var existingEnemies = GameObject.FindGameObjectsWithTag("enemyTroop");
                if (existingEnemies.Length > 0)
                {
                    print("Destroying existing enemies");
                    foreach(var e in existingEnemies)
                    {
                        Destroy(e);
                    }
                }

               //Opening level end window and displaying score
               levelEndWindow.SetActive(true);
               scoreText.text = ""; //Cleaning text field from existing text
               scoreText.text += "Enemies killed: " + enemiesKilled + "\n";

               //Calculating built buildings amount
               if (allPlayerBTags.Count > 0)
               {
                    foreach(var btag in allPlayerBTags)
                    {
                        var buildingsWithTag = GameObject.FindGameObjectsWithTag(btag);
                        if (buildingsWithTag.Length > 0)
                        buildingsBuilt += buildingsWithTag.Length;
                    }
               }
               scoreText.text += "Buildings built: " + buildingsBuilt + "\n";

               //Calculating troops amount
               var trainedTroop = GameObject.FindGameObjectsWithTag("Unit");
               if (trainedTroop.Length > 0)
               {
                   troopsTrained += trainedTroop.Length;
               }
               scoreText.text += "Troops trained: " + troopsTrained;
               rankText.text = "Rank:";
               //Picking rank for the player
               if (enemiesKilled >= 10)
               {
                   rankText.text += "Rookie Soldier";
               }
               else if (enemiesKilled >= 25)
               {
                   rankText.text += "Real Field Commander";
               }
               else if (enemiesKilled >= 40)
               {
                   rankText.text += "True War Veteran";
               }
               else
               {
                   rankText.text += "Beginner";
               }

               menuButton.SetActive(false); 
               
               return;
            }
                
                if (attackStarted)
                {
                    if (!tt.StartCountdown)
                    {
                            levelTick = true; 
                            print("Readying next enemy wave!");
                            attackStarted = false;
                            readyingForAttack = false;
                    }
                    else
                    {
                        actionText.text = "Enemy attack stops after:" + Mathf.Round(tt.TimeStart) + " s";
                    }
                }
                if (waveAmount > 0)
                {
                    if (levelTick && !attackStarted)
                    {
                        if (!readyingForAttack)
                        {
                            tt.startTimer(enemyReadyTime);
                            print("Enemy is readying for the attack!");
                            readyingForAttack = true;                            
                                
                        }
                        else
                        {
                            print("Enemy is readying for an attack!");
                            actionText.text = "Enemy attacking after:" + Mathf.Round(tt.TimeStart) + " s";
                        }
                        if (tt.FinishedTask)
                        {
                            print("Enemy is attacking the base!");                            
                            attackStarted = true;
                            readyingForAttack = false;
                            //Spawning enemy logic
                            //Getting attack lanes
                            var attackLanes = new List<List<GameObject>>();
                            attackLanes.Add(platformCubes[0]);
                            attackLanes.Add(platformCubes[platformCubes.Count - 1]);
                            //Getting attack position indexes
                            var attackPositionIndexes = new List<int>();
                            attackPositionIndexes.Add(0);
                            attackPositionIndexes.Add(platformCubes.Count - 2);

                            //Spawning enemies
                            foreach(var lane in attackLanes)
                            {
                                //Spawning left cube group
                                foreach(var index in attackPositionIndexes)
                                {
                                    //Spawning group
                                    for(int eindex = 0; eindex < enemyGroupSize; eindex++)
                                    {
                                        var spawnEnemy = Instantiate(enemyUnit, lane[index].transform.position, enemyUnit.transform.rotation);
                                        
                                        //Turning off skirmish mode
                                        if (spawnEnemy.GetComponent<DeathManagerTroop>())
                                        {
                                            var dMgr = spawnEnemy.GetComponent<DeathManagerTroop>();
                                            dMgr.IsInCh = true;
                                        }
                                        //Turining on challenge mode
                                        if (spawnEnemy.GetComponent<PlayerScore>())
                                        {
                                            var sMgr = spawnEnemy.GetComponent<PlayerScore>();
                                            sMgr.IsChEnemy = true;
                                        }

                                        if (spawnEnemy.GetComponent<NavMeshAgent>())
                                        {
                                            var moveCtrl = spawnEnemy.GetComponent<NavMeshAgent>();
                                            print("Enemy unit is travelling to playerBase position!");
                                            moveCtrl.destination = spawnPlayerBase.transform.position;
                                        }
                                    }
                                }
                            }
                            //Starting attack timer
                            tt.startTimer(enemyAttackTime);
                            levelTick = false;
                            enemyGroupSize++;
                            waveAmount--;
                        }
                    }
                }

            }

        }
    }
    private void printMatchText()
    {
        StartCoroutine(handleMatchText("Match begins!", printDelay));

        StartCoroutine(handleMatchText("Fight!", printDelay));
    }
    IEnumerator handleMatchText(string msg, float delay)
    {
        matchGreatText.SetActive(true);
        matchGreatText.GetComponent<Text>().text = msg;
        yield return new WaitForSeconds(delay);
         matchGreatText.SetActive(false);
    }
}
