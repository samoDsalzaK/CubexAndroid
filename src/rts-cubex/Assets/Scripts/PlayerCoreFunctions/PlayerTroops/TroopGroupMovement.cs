using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TroopGroupMovement : MonoBehaviour
{
    // private List<GameObject> troopChildren;
    private NavMeshAgent agent;
    private int randomChild;
    [SerializeField] GameObject groupModel;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // troopChildren = new List<GameObject>();
        // randomChild = Random.Range(0,troopChildren.Count);
    }

    void Update()
    {
        agent.destination = groupModel.transform.position;
        // troopChildren = getChildren();
        // foreach (var child in troopChildren){
        //     print(child);
        // }
        // if (troopChildren.Count > 1)
        // {
            // agent.destination = troopChildren[randomChild].transform.position;
        // }
        // else
        // {
        //     agent.destination = troopChildren[0].transform.position;
        // }
    }

    // private List<GameObject> getChildren()
    // {
    //     List<GameObject> gs = new List<GameObject>();
    //     foreach (Transform child in transform)
    //         gs.Add(child.gameObject);
    //     return gs;
    // }
}
