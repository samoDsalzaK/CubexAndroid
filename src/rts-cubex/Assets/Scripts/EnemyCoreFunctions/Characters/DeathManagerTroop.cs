using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManagerTroop : MonoBehaviour
{
   [SerializeField] int killPointsForPlayer = 5;
   [SerializeField] bool isTroop = false;
   [SerializeField] bool isInCh = false;
   private HealthEnemyAI health;
   private GameSession gs;
   public bool IsInCh {set {isInCh = value; } get { return isInCh; }}
   private void Start() {
       if (!isInCh)
       {
        gs = FindObjectOfType<GameSession>();
        health = GetComponent<HealthEnemyAI>();
       }
   }

   private void Update() {
       if (!isInCh)
       {
        if (health.getHealth() <= 0 && gs)
        {
            gs.AddPlayerScorePoints(killPointsForPlayer);
            if (isTroop)
                gs.increaseEPlayerUnitKillAmount();
            Destroy(gameObject);
        }
       }
   }
}
