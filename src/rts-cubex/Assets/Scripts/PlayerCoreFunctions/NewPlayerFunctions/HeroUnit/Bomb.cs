using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject mainModel;
    [SerializeField] int damagePoints = 30;
    private bool explosionSpawned = false;
    GameObject createEffect;
    private void OnTriggerEnter(Collider other) {

         switch(other.gameObject.tag) {            
            case "enemyTroop":           
                other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(damagePoints);
                
            break;
            
            case "EnemyBase":
                other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(damagePoints);
                
            break;

            case "EBCollector":
                other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(damagePoints);
                
            break;

            case "EMDef":
                other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(damagePoints);
                
            break;
            
            case "EmDefBody":
                other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(damagePoints);
                
            break;

            case "EBWorker":
                other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(damagePoints);
               
            break;

            case "EBBarracks":
                other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(damagePoints);
               
            break;           
            
            
        }
        if ((!(other.gameObject.name.ToLower()).Contains("Jet") && !(other.gameObject.name.ToLower()).Contains("arrow")) && !(other.gameObject.name.ToLower()).Contains("fire"))
            StartCoroutine(createExplosion());
       
    }
    IEnumerator createExplosion()
    {   
        if (!explosionSpawned) 
        {    
            createEffect = Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);
            explosionSpawned = true;
        }
        yield return new WaitForSeconds(0.2f);
        Destroy(mainModel);
        if (explosionSpawned)
        {
            Destroy(createEffect);  
        }      
        Destroy(gameObject);
    }
}
