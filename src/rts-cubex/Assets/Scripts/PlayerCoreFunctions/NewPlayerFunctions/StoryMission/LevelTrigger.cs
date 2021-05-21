using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] bool playerEntered = false;
    [SerializeField] string playerTroopTag = "Unit";

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == playerTroopTag && !playerEntered)
        {
            //Adding that this trap is triggered
            var lmgr = FindObjectOfType<SLevelManager>();
            
            if (lmgr)
            {
                lmgr.CheckTraps = true;
                foreach(var p in lmgr.SpawnEnemyPoints)
                {
                    if (p.MapTrigger)
                    {
                        if (p.MapTrigger.name == gameObject.name)
                        {
                            playerEntered = true;
                            p.PlayerEntered = playerEntered;
                            gameObject.SetActive(false);
                            return;
                            
                        }
                    }
                }
            }
        }
    }
}
