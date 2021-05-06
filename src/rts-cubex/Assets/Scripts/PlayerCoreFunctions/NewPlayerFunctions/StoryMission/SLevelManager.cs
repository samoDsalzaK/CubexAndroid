using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemySpawn : System.Object {
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform arrivalPoint;
    [Header("Trap configuration")]
    [SerializeField] GameObject mapTrigger;
    [SerializeField] bool playerEntered = false;

    public Transform SpawnPoint {get {return spawnPoint;}}
    public Transform ArrivalPoint {get {return arrivalPoint;}}
    public GameObject MapTrigger { get {return mapTrigger; }}
    public bool PlayerEntered {set {playerEntered = value;} get { return playerEntered;}}
}
public class SLevelManager : MonoBehaviour
{
    //NOTES:
    //FIX troop movement
    //Add starting enemies
    [Header("Player cnf.:")]
    [SerializeField] Transform mStartPoint;
    [SerializeField] Transform mSquadStartArrivalPoint;
    [SerializeField] GameObject startingHero;
    [SerializeField] List<GameObject> troopTypesToSpawn; //Holds light and heavy
    [SerializeField] int troopAmount = 6;
    [SerializeField] List<GameObject> spawnedSquad = new List<GameObject>();
    [Header("Enemy cnf.:")]
    [SerializeField] bool spawnEnemies = true;
    [SerializeField] GameObject enemyUnit;
    [SerializeField] List<EnemySpawn> spawnEnemyPoints;
    [Tooltip("Spawned enemy squad member count")]
    [SerializeField] int eSMCount = 3;
    [SerializeField] bool directSquadToStartPoint = false;
    [SerializeField] bool checkTraps = true;
    public List<EnemySpawn> SpawnEnemyPoints {get {return spawnEnemyPoints;}}
    public bool CheckTraps {set { checkTraps = value; } get {return checkTraps;}}
    //private Base playerBase;
    void Start()
    {
        //Mission squad spawning logic
       // playerBase = FindObjectOfType<Base>();

        var spawnedHero = Instantiate(startingHero, mStartPoint.transform.position, startingHero.transform.rotation);
        var spHeroAgent = spawnedHero.GetComponent<NavMeshAgent>();
        //spHeroAgent.destination = mSquadStartArrivalPoint.position;

        spawnedSquad.Add(spawnedHero);
        
        for (int tIndex = 0; tIndex < troopAmount / 2 - 1; tIndex++)
        {
            var spawnedSquadMember = Instantiate(troopTypesToSpawn[0], mStartPoint.transform.position, troopTypesToSpawn[0].transform.rotation);
            var sqAgent = spawnedSquadMember.GetComponent<NavMeshAgent>();
            //sqAgent.destination = mSquadStartArrivalPoint.position;

            spawnedSquad.Add(spawnedSquadMember);

            
        }

        for (int tIndex = troopAmount / 2; tIndex < troopAmount; tIndex++)
        {
            var spawnedSquadMember = Instantiate(troopTypesToSpawn[1], mStartPoint.transform.position, troopTypesToSpawn[0].transform.rotation);
            var sqAgent = spawnedSquadMember.GetComponent<NavMeshAgent>();
            //sqAgent.destination = mSquadStartArrivalPoint.position;
            
            spawnedSquad.Add(spawnedSquadMember);

            
        }

        if (spawnedSquad.Count > 0) directSquadToStartPoint = !directSquadToStartPoint;

       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (directSquadToStartPoint)
        {
            foreach(var m in spawnedSquad)
            {
                var moveCtrl = m.GetComponent<NavMeshAgent>();
                if(moveCtrl)
                    moveCtrl.destination = mSquadStartArrivalPoint.position;
            }
            //Reseting value to false
            directSquadToStartPoint = !directSquadToStartPoint;
        }
        if (spawnEnemies)
        {
             //Spawning enemies in the level
            if (spawnEnemyPoints.Count > 0)
            {
                foreach(var p in spawnEnemyPoints)
                {
                    if (!p.PlayerEntered)
                    {
                        //Spawn enemy squad at the specific location
                        for(int eindex = 0; eindex < eSMCount; eindex++)
                        {
                            var spawnEnemy = Instantiate(enemyUnit, p.SpawnPoint.position, enemyUnit.transform.rotation);
                            var eMoveCtrl = spawnEnemy.GetComponent<NavMeshAgent>();
                            if (eMoveCtrl)
                            {
                                eMoveCtrl.destination = p.ArrivalPoint.position;
                            }
                        }
                    }
                }
            }
            spawnEnemies = !spawnEnemies;
        }
        if (checkTraps)
        {
            if (triggeredTraps())
            {
                //Spring the traps...:)
                print("Attacked the player from the trap!!!!!");
                foreach(var p in spawnEnemyPoints)
                {
                    if(p.PlayerEntered)
                    {
                        //Spawning enemies
                        for (int eindex = 0; eindex < eSMCount + 1; eSMCount++)
                        {
                            var spawnEnemy = Instantiate(enemyUnit, p.SpawnPoint.position, enemyUnit.transform.rotation);
                            var moveCtrl = spawnEnemy.GetComponent<NavMeshAgent>();
                            if(moveCtrl)
                            {
                                moveCtrl.destination = p.ArrivalPoint.position;
                                moveCtrl.speed = moveCtrl.speed + moveCtrl.speed / 2;
                            }
                        }
                    }
                }
                checkTraps = false;
            
            }
        }
    }
    private bool triggeredTraps()
    {         
        foreach( var p in spawnEnemyPoints)
        {
            if(p.PlayerEntered)
            {
                return true;
            }
        }
        return false;
    }
}
