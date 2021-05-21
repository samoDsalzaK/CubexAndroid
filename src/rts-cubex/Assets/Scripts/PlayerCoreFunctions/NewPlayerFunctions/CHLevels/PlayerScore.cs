using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    //For challenge level managing
    [SerializeField] bool isChEnemy = false;

    private HealthEnemyAI ehealth;

    public bool IsChEnemy {set {isChEnemy = value;} get { return isChEnemy; }}

    private void Start() {
        ehealth = GetComponent<HealthEnemyAI>();
    } 
    private void Update() {
        if (isChEnemy)
        {
            if (ehealth.getHealth() <= 0)
            {
                var cMgr = FindObjectOfType<ChallengeMgr>();
                cMgr.EnemiesKilled++;
            }
        }
    }
}
