using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DefenderHero : MonoBehaviour
{
    [SerializeField] float scannerRadius;
    [SerializeField] string troopTag = "Unit";
    private Hero heroMain = new Hero();
    [SerializeField] List<Hero.UpgradeTask> uTasks;
    [SerializeField] int boostShieldPoints = 50;

    [Header("General param")]
    [SerializeField] Text heroHealthCharText;
    [SerializeField] Text abilityStatsText;
    [SerializeField] float taskTime = 5f;    
    [SerializeField] float diffCooldown = 2f; //For hero cooldown variation
    [SerializeField] float explosionDelay = 2f; 
    [SerializeField] Text shieldText;
    [SerializeField] GameObject shieldWallBtn;
    [SerializeField] Text shieldWallText;
    [SerializeField] GameObject barracadeWall;
    [SerializeField] Transform wallSpawnPoint;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject coolDownParticles;  
    [SerializeField] int wallBarrackExistTime = 5;
    [SerializeField] bool startAbilityColdown = false;  
    private UnityEngine.AI.NavMeshAgent movement;
    private TroopHealth thealth;
    private bool barracadeWallSpawned = false;
    private GameObject spawnedBarracdeWall;
    private TaskTimer timer;
    private float coolDownTime = 0;
    private TroopAttack tfire;
    private move troopMoveControl;
    public int WallBarrackExistTime {set { wallBarrackExistTime = value; } get { return wallBarrackExistTime; }}
    //Rogue bomb ability 
    
    void Start()
    {
        thealth = GetComponent<TroopHealth>();
        timer = GetComponent<TaskTimer>();
        movement = GetComponent<UnityEngine.AI.NavMeshAgent>();
        tfire = GetComponent<TroopAttack>();
        troopMoveControl = GetComponent<move>();

        uTasks = heroMain.UpgradeTasks;
        heroMain.HeroPos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        detectionTroopSphere();
        updateDataText();

        //Chck if boost shields to troops
        autoBoostTroopShields();

        shieldText.text = "Auto boost shield by\n(+" + boostShieldPoints + ")";
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
                    shieldWallText.text = "Cooldown\n(" + Mathf.Round(timer.TimeStart) + ")";
               }
        }
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
    }
    private void detectionTroopSphere()
    {
        //To check if player troops are in range
        heroMain.detectIfObjectsInRange(scannerRadius, troopTag);

        //To check if player troops are out of range
        heroMain.detectIfObjectsOutOfRange(scannerRadius, troopTag, "defender");

        // //Boost shields to near by troops
        // if (heroType == "defender")
        //     autoBoostTroopShields();
        // else if (heroType == "rogue")
        //     autoBoostAttack();
    }
    private void autoBoostTroopShields()
    {
        if (uTasks.Count > 0)
        {
            foreach(var uitem in uTasks)
            {
                if (!uitem.IsStatsBoosted)
                {
                   var tHealth = uitem.Unit.GetComponent<TroopHealth>();
                   var cShieldAmount = tHealth.ShieldHealth;
                   var mShieldAmount = tHealth.MaxShield;
                   boostShieldPoints = mShieldAmount / 2;
                   //print(boostShieldPoints);
                   if (cShieldAmount < boostShieldPoints)
                   {
                       tHealth.ShieldHealth = boostShieldPoints;
                       uitem.IsStatsBoosted = true;             
                   }
                }
            }
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
    private void updateDataText()
    {
          heroHealthCharText.text = ("\nHealth:" +  thealth.UnitHP + ", Shield(SH):" + thealth.ShieldHealth + 
                             "\nSH regeneration time:" + (Mathf.Round(thealth.MaxShield * thealth.ShieldRegTime)) + " s\nMovement speed:" + movement.speed);
          abilityStatsText.text = ("Ability\ncharacteristics:\n Holo Shield time:" + wallBarrackExistTime + " s");
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
