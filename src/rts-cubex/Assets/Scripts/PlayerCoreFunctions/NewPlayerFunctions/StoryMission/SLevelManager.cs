using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SLevelManager : MonoBehaviour
{
    //NOTES:
    //FIX troop movement
    //Add starting enemies
    [SerializeField] Transform mStartPoint;
    [SerializeField] Transform mSquadStartArrivalPoint;
    [SerializeField] GameObject startingHero;
    [SerializeField] List<GameObject> troopTypesToSpawn; //Holds light and heavy
    [SerializeField] int troopAmount = 6;
    [SerializeField] List<GameObject> spawnedSquad = new List<GameObject>();
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

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
