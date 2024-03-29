﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopLaserMovement : MonoBehaviour {
    Rigidbody laserMove;
    private Research damage;
    [SerializeField] float thrust = 2f;
    [SerializeField] float lifeTime = 7f;
    [SerializeField] ResearchConf oBGResearch;
    [SerializeField] string [] enemyTags;
    [SerializeField] bool heroFire = false;
    [SerializeField] int currentDamagePoints = 0;
    public bool HeroFire { set {heroFire = value; } get { return heroFire; }}
    public ResearchConf OBGResearch { get { return oBGResearch; }}
    private TaskTimer tt;
    void Start () {
        laserMove = GetComponent<Rigidbody> ();
        tt = GetComponent<TaskTimer>();
        tt.startTimer(lifeTime);
        //if (FindObjectOfType<Research> () != null) {
        //damage = FindObjectOfType<Research> ();}
    }
    void Update () {
        if (tt.FinishedTask)
        {
            Destroy(gameObject);
        }

        laserMove.velocity = transform.TransformDirection (new Vector3 (0f, 0f, thrust));
        var giveDamagePoints = HeroFire ? oBGResearch.HeroMakeDamagePoints : oBGResearch.getDamage();
        currentDamagePoints = giveDamagePoints;
    }
    private void OnTriggerEnter (Collider other) {
        /*if (isEnemy(other.gameObject.tag)){
            other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth (oBGResearch.getDamage());
            Destroy (gameObject);
        }
        else {
            Destroy (gameObject);
        }*/
        
        
        switch(other.gameObject.tag) {            
            case "enemyTroop":           
            other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(currentDamagePoints);
            Destroy(gameObject);
            break;
            
            case "EnemyBase":
            other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(currentDamagePoints);
            Destroy(gameObject);
            break;

            case "EBCollector":
            other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(currentDamagePoints);
            Destroy(gameObject);
            break;

            case "EMDef":
            other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(currentDamagePoints);
            Destroy(gameObject);
            break;
            
            case "EmDefBody":
            other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(currentDamagePoints);
            Destroy(gameObject);
            break;

            case "EBWorker":
            other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(currentDamagePoints);
            Destroy(gameObject);
            break;

            case "EBBarracks":
            other.gameObject.GetComponent<HealthEnemyAI>().decreaseHealth(currentDamagePoints);
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
    private bool isEnemy(string tag){
        foreach(var i in enemyTags){
            if(tag==i){
                return true;
            }
        }
        return false;
    }
    /* if the object (bullet) hits an enemy, it decreases their health the bullet is destroyed */
}