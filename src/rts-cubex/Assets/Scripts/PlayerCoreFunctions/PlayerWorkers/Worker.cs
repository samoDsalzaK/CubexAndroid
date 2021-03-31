using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Worker : MonoBehaviour
{
    [Header("Main Worker Configuration parameters")]
    [SerializeField] int energonAmount = 0;
    UnityEngine.AI.NavMeshAgent workerNav;
    bool workerState;
    private Vector3 pozitionOfCollector; 
    string buildingType;
    TimerForSpawningOriginal timerspawn;
    [SerializeField] string workerIndex;
    int pressedButtonIndex = 0;

    public int changePressedBtnIndex{get{return pressedButtonIndex;}set {pressedButtonIndex = value;}}
    // Start is called before the first frame update
    void Start()
    {
        timerspawn  = GetComponent<TimerForSpawningOriginal>();
        workerNav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        workerState = false;
    }
    // Update is called once per frame
    void Update()
    {
        ChekingDistance(); // method which is called each frame to check the distance to the barrack
        CheckingDistanceForTurret();
        CheckingDistanceForResearchCenter();
        CheckingDistanceForTroopsResearchCenter();
        //CheckingDistanceForPLayerWall();
        CheckingDistanceForMiningStation();
        CheckingDistanceForArmyCamp();
    }
    public void SetDestination(Vector3 energonPos) // method which which sets the destination of particular buildigs for worker
    {
        workerNav.destination = energonPos;  //workeris turi isiminti pozicija kur jis pastate energon collectoriu;
    }
    private void ChekingDistance() // kad zinoti ar jis ateina iki to paspausto tasko
    {
        if(!workerNav.pathPending && buildingType == "barrack")  // reikia pakeisti build type;
        {
            if(workerNav.remainingDistance <= workerNav.stoppingDistance)
            {
               if(!workerNav.hasPath || workerNav.velocity.sqrMagnitude == 0f)
               {
               timerspawn.startTimer(transform.position,0, pressedButtonIndex);   
               buildingType = "none";
               }
            }
        }
      
    }
   private void CheckingDistanceForTurret()
    {
        if(!workerNav.pathPending && buildingType == "turret")  // reikia pakeisti build type;
        {
            if(workerNav.remainingDistance <= workerNav.stoppingDistance)
            {
               if(!workerNav.hasPath || workerNav.velocity.sqrMagnitude == 0f)
               {
               timerspawn.startTimer(transform.position,2, pressedButtonIndex); 
               buildingType = "none";
               }
            }
        }
    }
    private void CheckingDistanceForResearchCenter()
    {
        if(!workerNav.pathPending && buildingType == "researchCenter")  // reikia pakeisti build type;
        {
            if(workerNav.remainingDistance <= workerNav.stoppingDistance)
            {
               if(!workerNav.hasPath || workerNav.velocity.sqrMagnitude == 0f)
               {
               timerspawn.startTimer(transform.position,3, pressedButtonIndex); 
               buildingType = "none";
               }
            }
        }
    }
    private void CheckingDistanceForTroopsResearchCenter()
    {
        if(!workerNav.pathPending && buildingType == "troopsReserach")  // reikia pakeisti build type;
        {
            if(workerNav.remainingDistance <= workerNav.stoppingDistance)
            {
               if(!workerNav.hasPath || workerNav.velocity.sqrMagnitude == 0f)
               {
               timerspawn.startTimer(transform.position = new Vector3(transform.position.x, 2f, transform.position.z),4, pressedButtonIndex); 
               buildingType = "none";
               }
            }
        }
    }
    /*private void CheckingDistanceForPLayerWall()
    {
        if(!workerNav.pathPending && buildingType == "wall")  // reikia pakeisti build type;
        {
            if(workerNav.remainingDistance <= workerNav.stoppingDistance)
            {
               if(!workerNav.hasPath || workerNav.velocity.sqrMagnitude == 0f)
               {
               timerspawn.startTimer(transform.position,5, pressedButtonIndex); 
               buildingType = "none";
               }
            }
        }
    }*/
    private void CheckingDistanceForMiningStation()
    {
        if(!workerNav.pathPending && buildingType == "miningStation")  // reikia pakeisti build type;
        {
            if(workerNav.remainingDistance <= workerNav.stoppingDistance)
            {
               if(!workerNav.hasPath || workerNav.velocity.sqrMagnitude == 0f)
               {
               timerspawn.startTimer(transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z),6, pressedButtonIndex); 
               buildingType = "none";
               }
            }
        }
    }
    private void CheckingDistanceForArmyCamp(){
        if(!workerNav.pathPending && buildingType == "armyCamp")  // reikia pakeisti build type;
        {
            if(workerNav.remainingDistance <= workerNav.stoppingDistance)
            {
               if(!workerNav.hasPath || workerNav.velocity.sqrMagnitude == 0f)
               {
               timerspawn.startTimer(transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z),7, pressedButtonIndex); 
               buildingType = "none";
               }
            }
        }
    }
    private void OnTriggerEnter(Collider other) { 
        if (other.gameObject.tag == "Depo" && (other.gameObject.GetComponent<BuildMiningStation>().getIndex() == workerIndex)) // condition which check for the tag Depo (Deposit Units) and also by checking index of each deposit unit avoids accidential collisions with other Deposits which have not been selected, 
        { 
            timerspawn.startTimer(other.gameObject.transform.position,1, pressedButtonIndex);  
            Destroy(other.gameObject); // desroyname ta deposit pointa.
        }
    }
    //Method that could be used for setting what amount of energon it can take from a energon deposit
    public void setEnergonInWorker(int e)
    {
        energonAmount = e;
    }
    //Method for checking what amount of energon does the player workers has
    public int getEnergonAmount()
    {
        return energonAmount;
    }

    public bool isWorkerAssigned()
    {
        return workerState;
    }

    public void setWorkerState(bool state)
    {
        workerState = state; 
    }

    public Vector3 getEnergonStationPozition()
    {
        return pozitionOfCollector;
    }

    public void setEnergonStationPozition(Vector3 coPozition)
    {
        pozitionOfCollector = coPozition;
    }

    public void SetBuildingOrder(string buildingtype)
    {
        buildingType = buildingtype;
    }

    public void setWorkerIndex(string index)
    {
      workerIndex = index;
    }

    public string getWorkerIndex()
    {
        return workerIndex;
    }
}

