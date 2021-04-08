using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private Transform heroPos;
    //For Rogue fire upgrades to troops
    private float newFireRate = 0f;
    private float oldFireRate = 0f;
    private float fireRateOffet = 0f;
    private int heroDmgPoints = 0;
    //Troop damage boosting
    private int newDamagePoints = 0;
    private int oldDamagePoints = 0;
    private int dmgOffset = 0;
    private Helper tool = new Helper();
    List<UpgradeTask> upgradeTasks = new List<UpgradeTask>();
    public List<UpgradeTask> UpgradeTasks { get {return upgradeTasks; }}
    public Transform HeroPos {set {heroPos = value; } get { return heroPos; }}
    public float NewFireRate {set {newFireRate = value; } get { return newFireRate; }}
    public float OldFireRate {set {oldFireRate = value; } get { return oldFireRate; }}
    public float FireRateOffet {set {fireRateOffet = value; } get { return fireRateOffet; }}
    public int HeroDmgPoints {set {heroDmgPoints = value; } get { return heroDmgPoints; }}
    public int DmgOffset {set {dmgOffset = value; } get { return dmgOffset; }}
    public int NewDamagePoints {set {newDamagePoints = value; } get { return newDamagePoints; }}
    public int OldDamagePoints {set {oldDamagePoints = value; } get { return oldDamagePoints; }}
    [System.Serializable]
    public class UpgradeTask : System.Object //Required for managing current upgraded units
    {
        [SerializeField] GameObject unit;
        [SerializeField] string uType = "none";
        [SerializeField] bool isStatsBoosted;

        public UpgradeTask(GameObject g)
        {
            this.unit = g;
        }
        public string UType { set { uType = value; } get { return uType; }}
        public GameObject Unit { set { unit = value; } get {return unit;}}
        public bool IsStatsBoosted { set { isStatsBoosted = value; } get {return isStatsBoosted;}}
    }
    public void detectIfObjectsInRange(float scannerRadius, string playerTroopTag)
    {
        Collider[] hitColliders = Physics.OverlapSphere(heroPos.position, scannerRadius);
        foreach (var hitCollider in hitColliders)
        {  
           var detectedDis = Vector3.Distance(hitCollider.gameObject.transform.position, heroPos.position);
      
           //Adding player troops if detected in the scanner range    
           var playerTroopInRange = !isInUList(hitCollider.gameObject.name) && (hitCollider.tag == playerTroopTag && (hitCollider.gameObject.name.ToLower()).Contains("troop"));
           if (playerTroopInRange)
           {               
               //print("Detected: " + hitCollider.gameObject.name + " at distance: " + detectedDis);
            
               var detectedTroop = hitCollider.gameObject;
               //Turining on troop health bars near hero    
               var troopHealth = detectedTroop.GetComponent<TroopHealth>();
               if (troopHealth)
               {
                   troopHealth.NearHero = true;
               }

               upgradeTasks.Add(new UpgradeTask(hitCollider.gameObject));
           }
        }
    }
     public void detectIfObjectsOutOfRange(float scannerRadius, string playerTroopTag, string heroType)
    {
        if (upgradeTasks.Count > 0)
        {
            foreach (var uitem in upgradeTasks)
            {
                var currentDistance = Mathf.Round(Vector3.Distance(uitem.Unit.transform.position, heroPos.position));
                if (currentDistance > scannerRadius)
                {
                   
                    var troopHealth = uitem.Unit.GetComponent<TroopHealth>();
                    if (troopHealth)
                          troopHealth.NearHero = false;

                    if (heroType == "rogue")
                    {
                        var troopAttackController = uitem.Unit.GetComponent<TroopAttack>();
                        troopAttackController.FireRate = oldFireRate; 
                        troopAttackController.Projectile.GetComponent<TroopLaserMovement>().OBGResearch.SDamage = oldDamagePoints;
                        
                    }
                    upgradeTasks.Remove(uitem);
                    break;
                }
            }
        }
    }
      //To check if player troop is in upgrade list
    private bool isInUList(string gname)
    {
        if (upgradeTasks == null)
            return false;

        foreach(var uItem in upgradeTasks)
        {
            if (uItem.Unit.name == gname)
                return true;
        }

        return false;
    }
  
}
