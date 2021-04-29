﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopAttack : MonoBehaviour
{
    [SerializeField] Transform fireGun;
    [SerializeField] float scannerRadius = 20f;
    [SerializeField] GameObject projectile;
    [SerializeField] float launchForce = 700f;
    [SerializeField] bool startAttacking = false;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] string targetName = "enemy";
    private bool lockFire = false;
    private float nextFire = 0.0f;
    private GameObject spottedEnemy;
    public bool LockFire { set { lockFire = value; } get { return lockFire; } }
    SniperMagazine magazine;

    void Start()
    {
        magazine = GetComponent<SniperMagazine>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!lockFire)
        {
            scanAreaForEnemies(); //Scan enemies

            //Main attack logic
            if (startAttacking)
            {
                if (spottedEnemy)
                    transform.LookAt(spottedEnemy.transform);

                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    attack();
                }
            }
        }
    }
    private void scanAreaForEnemies()
    {
        //Check if enemy in range
        // if (spottedEnemy)
        checkIfEnemyInFireRange();
        // else
        //Check if enemy our of range
        checkIfEnemyOutOfFireRange();
    }
    private void checkIfEnemyInFireRange()
    {
        if (!spottedEnemy)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, scannerRadius);
            foreach (var hitCollider in hitColliders)
            {
                if ((hitCollider.tag.ToLower()).Contains(targetName) && !(hitCollider.tag.ToLower()).Contains("arrow"))
                {
                    print("Enemy troop spotted!");
                    spottedEnemy = hitCollider.gameObject;
                    startAttacking = true;
                }
            }
        }
    }
    private void checkIfEnemyOutOfFireRange()
    {
        if (spottedEnemy)
        {
            var troopPos = transform.position;
            var spottedEnemyPos = spottedEnemy.transform.position;
            var distance = Vector3.Distance(spottedEnemyPos, troopPos);

            if (distance > scannerRadius)
            {
                spottedEnemy = null;
                return; //If enemy is out of range, then stop tracking it
            }
        }
    }
    private void attack()
    {
        if (spottedEnemy)
        {
            if (magazine)
            {
                if (magazine.getAmmo() > 0 && magazine.getFullMag())
                {
                    magazine.decreaseAmmo();
                    var sProjectile = Instantiate(projectile, fireGun.position, fireGun.rotation);
                    sProjectile.GetComponent<Rigidbody>().AddForce(fireGun.right * launchForce);
                }
            }
            else
            {
                var sProjectile = Instantiate(projectile, fireGun.position, fireGun.rotation);
                sProjectile.GetComponent<Rigidbody>().AddForce(fireGun.right * launchForce);
            }
        }
        else
        {
            startAttacking = false;
        }
    }
}