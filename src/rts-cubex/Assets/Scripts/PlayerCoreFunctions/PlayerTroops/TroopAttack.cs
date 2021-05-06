using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TroopAttack : MonoBehaviour
{
    [Header("For enemy units")]
    [SerializeField] bool followPlayerInCombat = false;
    [SerializeField] Transform fireGun;
    [SerializeField] Transform secondFireGun;
    [SerializeField] float scannerRadius = 20f;    
    [SerializeField] GameObject projectile;
    [SerializeField] float launchForce = 700f;
    [SerializeField] bool startAttacking = false;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] string targetName = "enemy";
    [SerializeField] List<string> targetTagNames;
    private bool lockFire = false;
    private float nextFire = 0.0f;
    [SerializeField] GameObject spottedEnemy;
    public float FireRate { set { fireRate = value; } get { return fireRate; }}
    public bool LockFire { set {lockFire = value; } get { return lockFire; }}
    public GameObject Projectile { get { return projectile; }}
    public GameObject SpottedEnemy { get { return spottedEnemy; }}
    private float distanceToEnemy = 0f;
    // Update is called once per frame
    void Update()
    {
        //For aggresive combat enemy mode
        if (followPlayerInCombat)
        {
            if (spottedEnemy)
            {
                var moveCtrl = GetComponent<NavMeshAgent>();
                if (moveCtrl)
                {
                    if (distanceToEnemy > scannerRadius / 2)
                        moveCtrl.destination = spottedEnemy.transform.position;
                }
            }
        }
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
                if (matchingTarget(hitCollider.tag.ToLower()))
                {
                    print("Enemy troop spotted!");
                    spottedEnemy = hitCollider.gameObject;
                    startAttacking = true;
                }
            }
        }
    }
    private void  checkIfEnemyOutOfFireRange()
    {
        if (spottedEnemy)
        {
            var troopPos = transform.position;
            var spottedEnemyPos = spottedEnemy.transform.position;
            distanceToEnemy = Vector3.Distance(spottedEnemyPos, troopPos);

            if (distanceToEnemy > scannerRadius)
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
            var sProjectile = Instantiate(projectile, fireGun.position, fireGun.rotation);
            sProjectile.GetComponent<Rigidbody>().AddForce (fireGun.right * launchForce);

            if (secondFireGun)
            {
                 var _sProjectile = Instantiate(projectile, secondFireGun.position, secondFireGun.rotation);
                 _sProjectile.GetComponent<Rigidbody>().AddForce (secondFireGun.right * launchForce);
            }
        }
        else
        {
            startAttacking = false;
        }
    }
    private bool matchingTarget(string spottedTargetTag)
    {
        foreach(var tag in targetTagNames)
        {
            if ((spottedTargetTag).Contains(tag))
            {
                return true;
            }
        }
        return false;
    }
}
