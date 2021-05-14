using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserMovment : MonoBehaviour
{
    [SerializeField] float speed = 5f; 
    [SerializeField] int damagePoints = 25; 
    [SerializeField] float lifeTimeTime = 7f;
    // [SerializeField] string playerUnitTag = "Unit";
    // [SerializeField] string playerBaseTag = "PlayerBase";
    Rigidbody laserMov;
    private TaskTimer tt;
    void Start()
    {
        laserMov = GetComponent<Rigidbody>();
        tt = GetComponent<TaskTimer>();
        tt.startTimer(lifeTimeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (tt.FinishedTask)
        {
            Destroy(gameObject);
        }

        laserMov.velocity = transform.TransformDirection(new Vector3(0f, 0f, speed));
    }
    private void OnTriggerEnter(Collider other) {
        switch(other.gameObject.tag)
        {
            case "Unit":
            if(other.gameObject.GetComponent<TroopHealth>()) {
            other.gameObject.GetComponent<TroopHealth>().decreaseHealth(damagePoints);
            }
            else if (other.gameObject.GetComponent<HeavyHealth>())
            {                
                other.gameObject.GetComponent<HeavyHealth>().decreaseHealth(damagePoints);
            }
            Destroy(gameObject);
            break;
            
            case "shrine":
            if (other.gameObject.GetComponent<HealthOfRegBuilding>())
            {
                other.gameObject.GetComponent<HealthOfRegBuilding>().decreaseHealth(damagePoints);
            }
            Destroy(gameObject);
          
            break;

            case "PlayerBase":
            if (other.gameObject.GetComponent<HealthOfRegBuilding>())
                 other.gameObject.GetComponent<HealthOfRegBuilding>().decreaseHealth(damagePoints);
            Destroy(gameObject);
            break;

            case "energonFactory":
            if (other.gameObject.GetComponent<HealthOfRegBuilding>())
                other.gameObject.GetComponent<HealthOfRegBuilding>().decreaseHealth(damagePoints);
            Destroy(gameObject);
            break;

            case "Worker":
            if (other.gameObject.GetComponent<HealthOfRegWorker>())
                other.gameObject.GetComponent<HealthOfRegWorker>().decreaseHealth(damagePoints);
            Destroy(gameObject);
            break;

            case "LvlMap":
                Destroy(gameObject);
            break;

            case "Arrow":
                Destroy(gameObject);
            break;
        }

         
    }
    public void setMaxDamagePoints(int mDmgPoints)
    {
        this.damagePoints = mDmgPoints;
    }
    public int getMaxDamagePoints()
    {
        return damagePoints;
    }
}
