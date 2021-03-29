using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] Text shieldText;
    [SerializeField] string playerTroopTag = "Unit";
    [SerializeField] string heroType;
    // [SerializeField] int hp;
    [SerializeField] int boostShieldPoints = 10;
    [SerializeField] int boostedShTroopsCount = 0;
    // [SerializeField] float movementSpeed;
    [SerializeField] GameObject heroToolbar;
    [SerializeField] GameObject barracadeWall;
    //For debuging
    [SerializeField] List<UpgradeTask> upgradeTasks = new List<UpgradeTask>();
    //private List<string> inRangeObjNames = new List<string>();
    // public int Hp { set {hp = value;} get { return hp; }}
    // public int Shield { set {shield = value;} get { return shield; }}
    public string HeroType { set { heroType = value; } get { return heroType; }}
    private int clickCount = 0;
    private Animator barracadeBuild;
    // public float MovementSpeed { set {movementSpeed = value;} get { return movementSpeed; }}

    private void Start() {
        barracadeBuild = barracadeWall.GetComponent<Animator>();
    }
    private void Update() {
        shieldText.text = "Auto boost shield by\n(+" + boostShieldPoints + ")";

        // Checking barracade animation build
        // if (barracadeWall.active)
        // {
        //     if (barracadeWall.transform.localScale.x >= 20)
        //     {
        //        barracadeBuild.SetBool("ButtonClicked", false);
        //     }
        // }
        //Scanner to see near by player troops
        detectionSphere(); 
        
       
    }
    public void spawnBarracde()
    {
        barracadeWall.SetActive(true);
        barracadeBuild.SetBool("ButtonClicked", true);
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
                   if (cShieldAmount == 0)
                   {
                       tHealth.ShieldHealth += boostShieldPoints;
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
