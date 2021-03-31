using System;
using UnityEngine;

public class TroopFire : MonoBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private Rigidbody projectilePrefab;
    [SerializeField] private float launchForce = 700f;
    [SerializeField] bool lockFire = false;
    private LookAtEnemy inRange;
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    public bool LockFire { set { lockFire = value; } get { return lockFire; }}
    private void Start () {
        inRange = FindObjectOfType<LookAtEnemy> ();
    }
    public void Update () {
        if (!LockFire)
        {
            if (inRange.getIsEnemyNear ()) {
                if (Time.time > nextFire) {
                    nextFire = Time.time + fireRate;
                    Shoot ();
                }
            }
        }
    }
    private void Shoot () {
        var projectileInstance = Instantiate (projectilePrefab, firePoint.position, firePoint.rotation);
        projectileInstance.AddForce (firePoint.forward * launchForce);
    }
    public Vector3 getUnitPosition () {
        return transform.position;
    }
}