using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//NOTES:
//Fix troop save zone
//Add hero to save
//Add spawning wave system
//Add objective system
//Add mini story interaction

[System.Serializable] //Objects that need to be guarded by enemies and the player needs to free them
public class LevelObject : System.Object {
    [Header ("Level object configuration")]
    [SerializeField] GameObject mlevelObject;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform guardSpawnPoint;
    [Header ("Level obj type(0-a,1-lb,2-t)")]
    [Range (0, 2)] // 0 - army camp, 1 - lootbox, 2 - troops to save
    [SerializeField] int type = 0;
    [SerializeField] bool saveOp = false;
    public int Type { get { return type; } }
    public bool SaveOp { get { return saveOp; } }
    public GameObject MlevelObject { set { mlevelObject = value; } get { return mlevelObject; } }
    public Transform SpawnPoint { set { spawnPoint = value; } get { return spawnPoint; } }
    public Transform GuardSpawnPoint { set { guardSpawnPoint = value; } get { return guardSpawnPoint; } }
}

[System.Serializable]
public class EnemySpawn : System.Object {
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform arrivalPoint;
    [Header ("Trap configuration")]
    [SerializeField] GameObject mapTrigger;
    [SerializeField] bool playerEntered = false;
    [Range (0, 1)]
    [SerializeField] int type = 0;
    public Transform SpawnPoint { get { return spawnPoint; } }
    public Transform ArrivalPoint { get { return arrivalPoint; } }
    public GameObject MapTrigger { get { return mapTrigger; } }
    public bool PlayerEntered { set { playerEntered = value; } get { return playerEntered; } }
    public int Type { set { type = value; } get { return type; } }
}
public class SLevelManager : MonoBehaviour {

    [Header ("Player cnf.:")]
    [SerializeField] Transform mStartPoint;
    [SerializeField] Transform mSquadStartArrivalPoint;
    [SerializeField] GameObject startingHero;
    [SerializeField] List<GameObject> troopTypesToSpawn; //Holds light and heavy
    [SerializeField] int troopAmount = 6;
    [SerializeField] List<GameObject> spawnedSquad = new List<GameObject> ();
    [SerializeField] GameObject playerBase;
    [SerializeField] Transform pSpawnPoint;
    [SerializeField] bool spawnPlayerBaseToSave = false;
    [Header ("Enemy cnf.:")]
    [SerializeField] bool spawnEnemies = true;
    [SerializeField] GameObject enemyUnit;
    [SerializeField] List<EnemySpawn> spawnEnemyPoints;
    [Tooltip ("Spawned enemy squad member count")]
    [SerializeField] int eSMCount = 3;
    [SerializeField] bool directSquadToStartPoint = false;
    [SerializeField] bool checkTraps = true;
    [SerializeField] int trapSquadAmount = 2;
    [Header ("Helper objects to spawn")]
    [SerializeField] int guardsCount = 3;
    [SerializeField] int troopSquadMToSave = 6; //Troop count to save
    [SerializeField] bool spawnLevelObjects = true;
    [SerializeField] List<LevelObject> levelObjects;
    [SerializeField] int energonRestore = 1000;
    [SerializeField] int creditsRestore = 1000;
    public List<EnemySpawn> SpawnEnemyPoints { get { return spawnEnemyPoints; } }
    public bool CheckTraps { set { checkTraps = value; } get { return checkTraps; } }
    private bool lootBoxSwitch = true;
    private GameObject spawnBase;
    //Spawn enemy waves logic
    private bool startSpawningWave = true;
    [Header("Enemy wave configuration")]
    [SerializeField] int eWaveAmount = 1;
    [SerializeField] int sMemberCount = 3;
    [SerializeField] float enemyWaveDelayStart = 30f;
    private TaskTimer tt;
    //private Base playerBase;
    void Start () {
        tt = GetComponent<TaskTimer>();
        //Spawning player bse that is required to save
        if (spawnPlayerBaseToSave) {
            spawnBase = Instantiate (playerBase, pSpawnPoint.position, playerBase.transform.rotation);
            if (spawnBase) {
                spawnBase.tag = "basesave";
                print (spawnBase.tag == "basesave" ? "Player base to save spawned!" : "Base spawned but bad tag setted!");

                var bMgr = spawnBase.GetComponent<Base> ();
                if (bMgr) {
                    bMgr.IsSaved = false;
                    bMgr.setEnergonAmount (0);
                    bMgr.setCreditsAmount (0);
                }
            }
        }

        var spawnedHero = Instantiate (startingHero, mStartPoint.transform.position, startingHero.transform.rotation);
        //Turning on hero interaction mode
        var hImanager = spawnedHero.GetComponent<IManager> ();
        if (hImanager) {
            if (!hImanager.SaveMode)
                hImanager.SaveMode = true;
        }
        var spHeroAgent = spawnedHero.GetComponent<NavMeshAgent> ();
        //spHeroAgent.destination = mSquadStartArrivalPoint.position;

        spawnedSquad.Add (spawnedHero);

        for (int tIndex = 0; tIndex < troopAmount / 2 - 1; tIndex++) {
            var spawnedSquadMember = Instantiate (troopTypesToSpawn[0], mStartPoint.transform.position, troopTypesToSpawn[0].transform.rotation);
            var sqAgent = spawnedSquadMember.GetComponent<NavMeshAgent> ();
            //sqAgent.destination = mSquadStartArrivalPoint.position;

            spawnedSquad.Add (spawnedSquadMember);

        }

        for (int tIndex = troopAmount / 2; tIndex < troopAmount; tIndex++) {
            var spawnedSquadMember = Instantiate (troopTypesToSpawn[1], mStartPoint.transform.position, troopTypesToSpawn[0].transform.rotation);
            var sqAgent = spawnedSquadMember.GetComponent<NavMeshAgent> ();
            //sqAgent.destination = mSquadStartArrivalPoint.position;

            spawnedSquad.Add (spawnedSquadMember);

        }

        if (spawnedSquad.Count > 0) directSquadToStartPoint = !directSquadToStartPoint;

    }

    // Update is called once per frame
    void Update () {
        if (directSquadToStartPoint) {
            foreach (var m in spawnedSquad) {
                var moveCtrl = m.GetComponent<NavMeshAgent> ();
                if (moveCtrl)
                    moveCtrl.destination = mSquadStartArrivalPoint.position;
            }
            //Reseting value to false
            directSquadToStartPoint = !directSquadToStartPoint;
        }

        //Check if player base is saved, then start spawning waves
        var baseClr = spawnBase.GetComponent<Base> ();
        if (baseClr.IsSaved && startSpawningWave) 
        {   
             
            if (spawnEnemyPoints.Count > 0) 
            {                
                if (eWaveAmount > 0) 
                {
                    if (!tt.StartCountdown && !tt.FinishedTask)  
                    {  
                        print("Starting countdown to spawning enemy wave!");
                        tt.startTimer(enemyWaveDelayStart);  
                    } 
                    //Spawn enemy wave
                    if (tt.FinishedTask)
                    {
                     print("Spawning enemy waves!");
                        foreach (var p in spawnEnemyPoints) {
                            if (p.Type == 0)
                            {
                                for (int eindex = 0; eindex < sMemberCount; eindex++) {
                                    var spawnEnemy = Instantiate (enemyUnit, p.SpawnPoint.position, enemyUnit.transform.rotation);
                                    var eMCtrl = spawnEnemy.GetComponent<NavMeshAgent> ();
                                    if (eMCtrl && spawnBase) {
                                        eMCtrl.destination = spawnBase.transform.position;
                                    }
                                }
                            }
                        }
                        eWaveAmount--;
                    }
                }
               
            }
        }

        if (spawnEnemies) {

            //Spawning enemies in the level
            if (spawnEnemyPoints.Count > 0) {
                foreach (var p in spawnEnemyPoints) {
                    if (p.Type == 0) {
                        //Spawn enemy squad at the specific location
                        for (int eindex = 0; eindex < eSMCount; eindex++) {
                            var spawnEnemy = Instantiate (enemyUnit, p.SpawnPoint.position, enemyUnit.transform.rotation);
                            var eMoveCtrl = spawnEnemy.GetComponent<NavMeshAgent> ();
                            if (eMoveCtrl) {
                                eMoveCtrl.destination = p.ArrivalPoint.position;
                            }
                        }
                    }
                }
            }
            spawnEnemies = !spawnEnemies;
        }
        if (checkTraps) {
            if (triggeredTraps ()) {
                //Spring the traps...:)
                print ("Attacked the player from the trap!!!!!");
                foreach (var p in spawnEnemyPoints) {
                    if (p.PlayerEntered && p.Type == 1) {
                        //Spawning enemies
                        for (int eindex = 0; eindex < eSMCount + trapSquadAmount; eindex++) {
                            var spawnEnemy = Instantiate (enemyUnit, p.SpawnPoint.position, enemyUnit.transform.rotation);
                            var moveCtrl = spawnEnemy.GetComponent<NavMeshAgent> ();
                            if (moveCtrl) {
                                moveCtrl.destination = p.ArrivalPoint.position;
                                moveCtrl.speed = moveCtrl.speed + moveCtrl.speed / 2;
                            }
                        }
                    }
                }
                checkTraps = false;

            }
        }
        //Spawn level objects
        if (spawnLevelObjects) {
            if (levelObjects == null || levelObjects.Count <= 0) {
                spawnLevelObjects = false;
                return;
            }

            foreach (var o in levelObjects) {
                //Spawning level object
                if (o.Type == 0) {
                    var spawnLevelObject = Instantiate (o.MlevelObject, o.SpawnPoint.position, o.MlevelObject.transform.rotation);
                    if (spawnLevelObject) {
                        print ("Level object spawned! " + spawnLevelObject.name);
                    }
                    //Spawning guards
                    for (int eindex = 0; eindex < guardsCount; eindex++) {
                        var spawnEnemy = Instantiate (enemyUnit, o.GuardSpawnPoint.position, o.GuardSpawnPoint.rotation);

                        var moveCtrl = spawnEnemy.GetComponent<NavMeshAgent> ();

                        if (moveCtrl) {
                            print ("Sending guards to guard " + spawnLevelObject.name);
                            //NOTE: Add later distance measurement
                            moveCtrl.destination = spawnLevelObject.transform.position;
                        }
                    }
                } else if (o.Type == 1) {

                    var spawnLootBox = Instantiate (o.MlevelObject, o.SpawnPoint.position, o.MlevelObject.transform.rotation);
                    if (spawnLootBox) {
                        var lCtrl = spawnLootBox.GetComponent<LootBox> ();
                        if (lCtrl) {
                            if (lootBoxSwitch) {
                                lCtrl.EnergonToAdd += energonRestore;
                                lCtrl.BoxType = 0;
                            } else {
                                lCtrl.BoxType = 1;
                                lCtrl.CreditsToAdd += creditsRestore;
                            }

                            lootBoxSwitch = !lootBoxSwitch;
                        }
                        //Spawning guards
                        for (int eindex = 0; eindex < guardsCount; eindex++) {
                            var spawnEnemy = Instantiate (enemyUnit, o.GuardSpawnPoint.position, o.GuardSpawnPoint.rotation);

                            var moveCtrl = spawnEnemy.GetComponent<NavMeshAgent> ();

                            if (moveCtrl) {
                                print ("Sending guards to guard " + spawnLootBox.name);
                                //NOTE: Add later distance measurement
                                moveCtrl.destination = spawnLootBox.transform.position;
                            }
                        }
                    }

                }
                //NOTE: Fix later
                else if (o.Type == 2 && o.SaveOp) {
                    //Spawning troops to save and moving them to certain position
                    for (int tIndex = 0; tIndex < troopSquadMToSave; tIndex++) {
                        var spawnTroop = Instantiate (o.MlevelObject, o.GuardSpawnPoint.position, o.GuardSpawnPoint.rotation);
                        spawnTroop.GetComponent<move> ().NeedsCamp = false;
                        spawnTroop.tag = "usave";
                        var moveCtrl = spawnTroop.GetComponent<NavMeshAgent> ();
                        if (moveCtrl) {
                            //print("Sending guards to guard " + spawnLootBox.name);
                            //NOTE: Add later distance measurement
                            moveCtrl.destination = spawnTroop.transform.position;
                        }
                    }
                }
            }
            //Closing level object spawning
            spawnLevelObjects = false;
        }

    }
    
    private bool triggeredTraps () {
        foreach (var p in spawnEnemyPoints) {
            if (p.PlayerEntered) {
                return true;
            }
        }
        return false;
    }
}