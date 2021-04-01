using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
// class Hero{
//     private string heroType;
//     private int hp;
//     private int shield;
//     private float movementSpeed;
//     public int Hp { set {hp = value;} get { return hp; }}
//     public int Shield { set {shield = value;} get { return shield; }}
//     public string HeroType { set { heroType = value; } get { return heroType; }}

// }
// class Defender : Hero{

// }

//FIX Troop fire, create new system
//Create Rogue
//IsShieldBoosted
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
public class HeroUnit : MonoBehaviour
{
    [Header("Object detection")]
    [SerializeField] float scannerRadius = 10f;
    //[SerializeField] Image detectionRange;
    [Header("General param")]
    [SerializeField] float taskTime = 5f;    
    [SerializeField] float diffCooldown = 2f; //For hero cooldown variation
    [SerializeField] float explosionDelay = 2f; 
    [SerializeField] Text shieldText;
    [SerializeField] GameObject shieldWallBtn;
    [SerializeField] Text shieldWallText;
    [SerializeField] string playerTroopTag = "Unit";
    [SerializeField] string heroType;
    [SerializeField] bool startAbilityColdown = false;
    // [SerializeField] int hp;
    [SerializeField] int boostShieldPoints = 10;
    //[SerializeField] Vector3 movementVelocity;
    [SerializeField] GameObject heroToolbar;
    [SerializeField] GameObject barracadeWall;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject coolDownParticles;
    //For debuging
    [SerializeField] List<UpgradeTask> upgradeTasks = new List<UpgradeTask>();
    //private List<string> inRangeObjNames = new List<string>();
    // public int Hp { set {hp = value;} get { return hp; }}
    // public int Shield { set {shield = value;} get { return shield; }}
    public string HeroType { set { heroType = value; } get { return heroType; }}
    //private int clickCount = 0;
    private Animator barracadeBuild;
    private NavMeshAgent movement;
    private bool barracadeWallSpawned = false;
    private TaskTimer timer;
    private float coolDownTime = 0;
    private TroopAttack tfire;
    private move troopMoveControl;
    //For firing upgrades
    private float newFireRate = 0f;
    private float oldFireRate = 0f;
    private float fireRateOffet = 0f;
    private int heroDmgPoints = 0;
    private int newDamagePoints = 0;
    private int oldDamagePoints = 0;
    private int dmgOffset = 0;
   // public Vector3 MovementVelocity { set {movementVelocity = value;} get { return movementVelocity; }}

    private void Start() {
        timer = GetComponent<TaskTimer>();
        movement = GetComponent<NavMeshAgent>();
        tfire = GetComponent<TroopAttack>();
        if (heroType == "defender")
            barracadeBuild = barracadeWall.GetComponent<Animator>();
        troopMoveControl = GetComponent<move>();
        
        if (heroType == "rogue")
        {
            var projectileData =  gameObject
                                 .GetComponent<TroopAttack>().Projectile
                                 .GetComponent<TroopLaserMovement>();
            projectileData.HeroFire = true;
            var troopData = projectileData.OBGResearch;
            
            //Setting initial data for damage upgrades
            heroDmgPoints = troopData.HeroMakeDamagePoints;   
            fireRateOffet = (gameObject.GetComponent<TroopAttack>().FireRate / 2);  
            dmgOffset = (heroDmgPoints / 2);         
            shieldText.text = "Auto boost firerate(+" + fireRateOffet + ")\ndamage (+" + dmgOffset + ")";
        }
        
    }
    private void Update() {
       //Scaning for near by troops
       detectionTroopSphere(); 
       switch (heroType) 
       {
           case "defender":
                if (startAbilityColdown)
                {
                    coolDownParticles.SetActive(true);
                    if (timer.FinishedTask)
                    {
                        shieldWallText.text = "Holo shield";
                        coolDownParticles.SetActive(false);
                        startAbilityColdown = false;
                        shieldWallBtn.GetComponent<Button>().interactable = true;
                    }
                    else 
                    {
                        shieldWallText.text = "Cooldown\nHolo shield\n(" + Mathf.Round(timer.TimeStart) + ")";
                    }
                }
                shieldText.text = "Auto boost shield by\n(+" + boostShieldPoints + ")";        
            

                // Handle second defender ability
                if(barracadeWallSpawned)
                {
                    
                    movement.isStopped = true;
                    troopMoveControl.LockMove = true;
                    tfire.LockFire = true;
                    //movement.velocity = Vector3.zero;
                    if (timer.FinishedTask)
                    {
                        StartCoroutine(spawnExplosion());              
                        movement.isStopped = false;
                        troopMoveControl.LockMove = false;
                        //movement.velocity = MovementVelocity;
                        barracadeWall.SetActive(false);
                        barracadeBuild.SetBool("ButtonClicked", false);
                        barracadeWallSpawned = false;
                        tfire.LockFire = false; 
                        coolDownTime = (taskTime * 2) - diffCooldown;
                        startAbilityColdown = true; 
                        timer.startTimer(coolDownTime); 
                        //shieldWallText.text = "Holo shield";
                    }
                    else
                    {
                        shieldWallText.text = "Holo shield\n(" + Mathf.Round(timer.TimeStart) + ")";
                    }
                }
                //Scanner to see near by player troops               
            break;
            // case "rogue" :
                 
            // break;
       }
    }
    IEnumerator spawnExplosion()
    {
        var e = Instantiate(explosion, barracadeWall.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(explosionDelay);
        Destroy(e);
    }
    public void spawnBarracde()
    {        
        barracadeWall.SetActive(true);
        barracadeBuild.SetBool("ButtonClicked", true);
        barracadeWallSpawned = true;
        shieldWallBtn.GetComponent<Button>().interactable = false;
        
        timer.startTimer(taskTime);
        //After timer is finished reset animation parameter
       //  barracadeBuild.SetBool("ButtonClicked", false);
       
    }
   
    private void detectionTroopSphere()
    {
        //To check if player troops are in range
        detectIfObjectsInRange();

        //To check if player troops are out of range
        detectIfObjectsOutOfRange();

        //Boost shields to near by troops
        if (heroType == "defender")
            autoBoostTroopShields();
        else if (heroType == "rogue")
            autoBoostAttack();
    }
    private void autoBoostAttack() //For boosting auto attack
    {
        if (upgradeTasks == null || upgradeTasks.Count > 0)
        {
            foreach (var utask in upgradeTasks)
            {
                if (!utask.IsStatsBoosted)
                {
                    var troopAttackController = utask.Unit.GetComponent<TroopAttack>();
                    //Boosting fire rate                   
                    newFireRate = troopAttackController.FireRate - fireRateOffet;
                    oldFireRate = troopAttackController.FireRate;
                    troopAttackController.FireRate = newFireRate;
                   
                    //Boosing projectile damage points
                    var ctProjectile = troopAttackController.Projectile;
                    var projectileController = ctProjectile.GetComponent<TroopLaserMovement>();
                    var proData = projectileController.OBGResearch;
                    oldDamagePoints = proData.SDamage;                    
                    newDamagePoints = proData.SDamage + dmgOffset;
                    proData.SDamage = newDamagePoints;

                     utask.IsStatsBoosted = true;
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
    //Shield boosting method, which gives starting shield if it is 0 value
    private void autoBoostTroopShields()
    {
        if (upgradeTasks.Count > 0)
        {
            foreach(var uitem in upgradeTasks)
            {
                if (!uitem.IsStatsBoosted)
                {
                   var tHealth = uitem.Unit.GetComponent<TroopHealth>();
                   var cShieldAmount = tHealth.ShieldHealth;
                   var mShieldAmount = tHealth.MaxShield;
                   boostShieldPoints = mShieldAmount / 2;
                   if (cShieldAmount < boostShieldPoints)
                   {
                       tHealth.ShieldHealth = boostShieldPoints;
                       uitem.IsStatsBoosted = true;             
                   }
                }
            }
        }
    }
    // To check if player troops are out of range
    private void detectIfObjectsOutOfRange()
    {
        if (upgradeTasks.Count > 0)
        {
            foreach (var uitem in upgradeTasks)
            {
                var currentDistance = Mathf.Round(Vector3.Distance(uitem.Unit.transform.position, transform.position));
                if (currentDistance > scannerRadius)
                {
                    if (heroType == "defender")
                    {
                        var troopHealth = uitem.Unit.GetComponent<TroopHealth>();
                        if (troopHealth)
                            troopHealth.NearHero = false;
                    }
                    else if (heroType == "rogue")
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
     // To check if player troops are in range
    private void detectIfObjectsInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, scannerRadius);
        foreach (var hitCollider in hitColliders)
        {  
           var detectedDis = Vector3.Distance(hitCollider.gameObject.transform.position, transform.position);
      
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
   
    
}
