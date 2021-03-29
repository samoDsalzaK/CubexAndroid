using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StopMovement : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var troop = other.gameObject;
        if (troop.tag == "Unit")
        {
            if (troop.GetComponent<move>())
            {
                if (troop.GetComponent<move>().getOnItsWay())
                {
                    if (troop.GetComponent<NavMeshAgent>())
                    {
                        troop.GetComponent<NavMeshAgent>().isStopped = true;
                    }
                }

            }
        }
    }
}
