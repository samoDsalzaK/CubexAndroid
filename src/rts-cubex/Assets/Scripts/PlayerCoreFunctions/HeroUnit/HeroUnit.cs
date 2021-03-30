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
// NOTE: Add index to spawned troop names from barracks
// NOTE:  Fix spherecast detection system
[System.Serializable]
public class UpgradeTask : System.Object //Required for managing current upgraded units
{
    [SerializeField] GameObject unit;
    [SerializeField] string uType = "none";
    [SerializeField] bool isShieldBoosted;

    public UpgradeTask(GameObject g)
    {
        this.unit = g;
    }
    public string UType { set { uType = value; } get { return uType; }}
    public GameObject Unit { set { unit = value; } get {return unit;}}
    public bool IsShieldBoosted { set { isShieldBoosted = value; } get {return isShieldBoosted;}}
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
    [SerializeField] Button shieldWallBtn;
    [SerializeField] Text shieldWallText;
    [SerializeField] string playerTroopTag = "Unit";
    [SerializeField] string heroType;
    [SerializeField] bool startAbilityColdown = false;
    // [SerializeField] int hp;
    [SerializeField] int boostShieldPoints = 10;
    // [SerializeField] float movementSpeed;
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
    private int clickCount = 0;
    private Animator barracadeBuild;
    private NavMeshAgent movement;
    private bool barracadeWallSpawned = false;
    private TaskTimer timer;
    private float coolDownTime = 0;
    // public float MovementSpeed { set {movementSpeed = value;} get { return movementSpeed; }}

    private void Start() {
        timer = GetComponent<TaskTimer>();
        movement = GetComponent<NavMeshAgent>();
        barracadeBuild = barracadeWall.GetComponent<Animator>();
    }
    private void Update() {
        
         if (startAbilityColdown)
         {
            coolDownParticles.SetActive(true);
            if (timer.FinishedTask)
            {
                shieldWallText.text = "Holo shield";
                coolDownParticles.SetActive(false);
                startAbilityColdown = false;
                shieldWallBtn.interactable = true;
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
           if (timer.FinishedTask)
           {
               StartCoroutine(spawnExplosion());              
               movement.isStopped = false;
               barracadeWall.SetActive(false);
               barracadeBuild.SetBool("ButtonClicked", false);
               barracadeWallSpawned = false;

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
        detectionSphere(); 
        
       
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
        shieldWallBtn.interactable = false;
        timer.startTimer(taskTime);
        //After timer is finished reset animation parameter
       //  barracadeBuild.SetBool("ButtonClicked", false);
       
    }
    private void OnMouseDown() {
        
        if (clickCount < 1)
        {
            heroToolbar.SetActive(true);
            clickCount++;
        }
        else
        {
            heroToolbar.SetActive(false);
            clickCount = 0;
        }
    }
    private void detectionSphere()
    {
        //To check if player troops are in range
        detectIfObjectsInRange();

        //To check if player troops are out of range
        detectIfObjectsOutOfRange();

        //Boost shields to near by troops
        autoBoostTroopShields();
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
                if (!uitem.IsShieldBoosted)
                {
                   var tHealth = uitem.Unit.GetComponent<TroopHealth>();
                   var cShieldAmount = tHealth.ShieldHealth;
                   var mShieldAmount = tHealth.MaxShield;
                   boostShieldPoints = mShieldAmount / 2;
                   if (cShieldAmount < boostShieldPoints)
                   {
                       tHealth.ShieldHealth = boostShieldPoints;
                       uitem.IsShieldBoosted = true;             
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
                    var troopHealth = uitem.Unit.GetComponent<TroopHealth>();
                    if (troopHealth)
                        troopHealth.NearHero = false;
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
