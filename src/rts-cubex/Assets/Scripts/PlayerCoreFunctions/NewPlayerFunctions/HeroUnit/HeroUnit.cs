using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


//NOTE: Seperate heroUnit script into two scripts: DefenderHero, RogueHero
//NOTE: Create hero spawner on player base
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
    [SerializeField] List<UpgradeTask> upgradeTasks = new List<UpgradeTask>();
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
    [SerializeField] Text heroHealthCharText;
    [SerializeField] Text abilityStatsText;
    // [SerializeField] int hp;
    [Header("Defender cnf param")]
    [SerializeField] int boostShieldPoints = 10;
    //[SerializeField] Vector3 movementVelocity;
    [SerializeField] GameObject heroToolbar;
    [SerializeField] GameObject barracadeWall;
    [SerializeField] Transform wallSpawnPoint;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject coolDownParticles;  
    [SerializeField] int wallBarrackExistTime = 5;  
    //For debuging    
    [Header("Rogue cnf param")]
    [SerializeField] List<Transform> bombSpawnPoints;
    [SerializeField] GameObject bomb;    
    [SerializeField] float fireBombRate = 0.5f;
    [SerializeField] float launchForce = 100f;
    [SerializeField] float errorMessageDelay = 1f;
    private float nextBombFire = 0.0f;
    private bool spawnBombs = false;
    private int nextBombIndex = 0;
    //private List<string> inRangeObjNames = new List<string>();
    // public int Hp { set {hp = value;} get { return hp; }}
    // public int Shield { set {shield = value;} get { return shield; }}
    public string HeroType { set { heroType = value; } get { return heroType; }}
    //private int clickCount = 0;
    private NavMeshAgent movement;
    private TroopHealth thealth;
    private bool barracadeWallSpawned = false;
    private GameObject spawnedBarracdeWall;
    private TaskTimer timer;
    private float coolDownTime = 0;
    private TroopAttack tfire;
    private move troopMoveControl;
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
    private Color originalBombBtnColor;
    
    private Animator heroAnimator;
    public int WallBarrackExistTime {set { wallBarrackExistTime = value; } get { return wallBarrackExistTime; }}
    //Rogue bomb ability 
    public GameObject Bomb {set { bomb = value; } get { return bomb; }}
    
   // public Vector3 MovementVelocity { set {movementVelocity = value;} get { return movementVelocity; }}

    private void Start() {
        thealth = GetComponent<TroopHealth>();
        timer = GetComponent<TaskTimer>();
        movement = GetComponent<NavMeshAgent>();
        tfire = GetComponent<TroopAttack>();
        
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
            heroAnimator = GetComponent<Animator>();
        }
        // else 
        // {
        //     heroCharText.text += ("Barracade wall time:" + wallBarrackExistTime + "s");
        // }
    }
    private void Update() {
        updateDataText();
       //Scaning for near by troops
       detectionTroopSphere(); 

        if (startAbilityColdown)
        {
                coolDownParticles.SetActive(true);
               if (timer.FinishedTask)
               {
                  shieldWallText.text = heroType == "defender" ? "Holo shield" : "Bomb storm";
                  coolDownParticles.SetActive(false);
                  startAbilityColdown = false;
                  shieldWallBtn.GetComponent<Button>().interactable = true;
               }
               else 
               {
                    shieldWallText.text = "Cooldown\n(" + Mathf.Round(timer.TimeStart) + ")";
               }
        }

       switch (heroType) 
       {
           case "defender":
                
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
                        //barracadeWall.SetActive(false);
                        //barracadeBuild.SetBool("ButtonClicked", false);
                       
                        barracadeWallSpawned = false;
                        tfire.LockFire = false; 
                        var coolDownOffset = (wallBarrackExistTime / 4);
                        coolDownTime = wallBarrackExistTime - coolDownOffset + diffCooldown;
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
            case "rogue" :
                 if (spawnBombs)
                 {
                     shieldWallText.text = "Spawning bombs\n(" + Mathf.Round(timer.TimeStart) + ")";
                     if(timer.FinishedTask)
                     {
                         shieldWallText.text = "Bomb storm";
                         spawnBombs = false;
                         //shieldWallBtn.GetComponent<Button>().interactable = true;
                         heroAnimator.SetBool("ready", false);
                         startAbilityColdown = true;
                         coolDownTime = (taskTime * 2) - diffCooldown;
                         startAbilityColdown = true; 
                         timer.startTimer(coolDownTime); 
                     }
                   
                 }
            break;
       }
    }
    IEnumerator spawnExplosion()
    {
        Destroy(spawnedBarracdeWall);
        var e = Instantiate(explosion, wallSpawnPoint.position, transform.rotation);
        yield return new WaitForSeconds(explosionDelay);       
        Destroy(e);
    }
    public void spawnBarracde()
    {    
        var spawnBarracde = Instantiate(barracadeWall, wallSpawnPoint.position, transform.rotation);  
        spawnBarracde.transform.parent = transform;  
        //spawnBarracde.transform.position = new Vector3(0f, 0f, 10f);
        //spawnBarracde.SetActive(true);
        spawnedBarracdeWall = spawnBarracde;
        //barracadeWall.SetActive(true);
        //barracadeBuild.SetBool("ButtonClicked", true);
        barracadeWallSpawned = true;
        shieldWallBtn.GetComponent<Button>().interactable = false;
        //print(wallBarrackExistTime);
        timer.startTimer(wallBarrackExistTime);
        //After timer is finished reset animation parameter
       //  barracadeBuild.SetBool("ButtonClicked", false);
       
    }
    public void doBombardment()
    {
        if (tfire.SpottedEnemy)
        {
            spawnBombs = true;
            shieldWallBtn.GetComponent<Button>().interactable = false;
            //timer.startTimer(taskTime);
            heroAnimator.SetBool("ready", true);
            timer.startTimer(taskTime);
        }
        else
        {
            //print("No enemy spotted!");
            StartCoroutine(displayErrorMessageBomb());
            
        }
    }
    IEnumerator displayErrorMessageBomb()
    {
        shieldWallText.text = "No enemy spotted";
        originalBombBtnColor = shieldWallBtn.GetComponent<Image>().color;
        shieldWallBtn.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(errorMessageDelay);
        shieldWallText.text = "Bomb storm";
        shieldWallBtn.GetComponent<Image>().color = originalBombBtnColor;
    }
    public void spawnBombsOnEvent()
    {
        if (tfire.SpottedEnemy)
        {
            if (nextBombIndex >= bombSpawnPoints.Count)
            {
                nextBombIndex = 0;
            }
            var spawnedBomb = Instantiate(bomb, bombSpawnPoints[nextBombIndex].position, bombSpawnPoints[nextBombIndex].rotation);           
            var bombDirection = tfire.SpottedEnemy.transform.position - transform.position;
            spawnedBomb.GetComponent<Rigidbody>().AddForce(bombDirection.normalized * launchForce, ForceMode.Impulse);
            //spawnedBomb.GetComponent<Rigidbody>().AddForce((Vector3.forward + Vector3.up) * launchForce);
            nextBombIndex++;
        }
        else
        {
            nextBombIndex = 0;
            heroAnimator.SetBool("ready", false);
            timer.FinishedTask = true;
            timer.StartCountdown = false;
        }
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
    private void updateDataText()
    {
          heroHealthCharText.text = ("\nHealth:" +  thealth.UnitHP + ", Shield(SH):" + thealth.ShieldHealth + 
                             "\nSH regeneration time:" + (Mathf.Round(thealth.MaxShield * thealth.ShieldRegTime)) + " s\nMovement speed:" + movement.speed);
          abilityStatsText.text = "Ability\ncharacteristics:\n" + (heroType == "rogue" ? ("Bomb damage:" +  bomb.GetComponent<Bomb>().DamagePoints) : ("Holo Shield time:" + wallBarrackExistTime + " s"));
    }
    private void OnDestroy() {
        var shrine = FindObjectOfType<Shrine>();
        if (shrine)
        {
            var heroList = shrine.HToTrain;
            if (heroList.Count > 0)
            {
                foreach(var hero in heroList)
                {
                    if (gameObject.name.Contains(hero.Hero.name))
                    {
                        hero.ReadyToTrain = false;
                        hero.SpawnButton.interactable = true;
                        break;
                    }
                }
            }
        }
    }
}
