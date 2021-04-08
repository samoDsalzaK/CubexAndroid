using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RogueHero : MonoBehaviour
{
    [SerializeField] float scannerRadius;
    [SerializeField] string troopTag = "Unit";
    private Hero heroMain = new Hero();
    [SerializeField] List<Hero.UpgradeTask> uTasks;
    [SerializeField] Text heroHealthCharText;
    [SerializeField] Text abilityStatsText;
    [SerializeField] float taskTime = 5f;    
    
    [SerializeField] Text fireBoostText;
    [SerializeField] GameObject bombBtn;
    [SerializeField] Text bombBtnText;

    [Header("Rogue cnf param")]
    [SerializeField] float oldTroopFireRate = 1f;
    [SerializeField] float fireRateOffset = 0.25f;
    [SerializeField] int heroDmgPoints = 0;
    [SerializeField] int dmgOffset =  10;
    [SerializeField] List<Transform> bombSpawnPoints;
    [SerializeField] GameObject bomb; 
    [SerializeField] GameObject coolDownParticles;   
    [SerializeField] float fireBombRate = 0.5f;
    [SerializeField] float launchForce = 100f;
    [SerializeField] float errorMessageDelay = 1f;
    [SerializeField] float diffCooldown = 2f;
     private UnityEngine.AI.NavMeshAgent movement;
    private TroopHealth thealth;
    private TaskTimer timer;
    private float nextBombFire = 0.0f;
    private bool spawnBombs = false;
    private int nextBombIndex = 0;
    private Animator heroAnimator;
    public GameObject Bomb {set { bomb = value; } get { return bomb; }}
    private Color originalBombBtnColor;
    private TroopAttack tfire;
    private float coolDownTime = 0;
    private bool startAbilityColdown = false;
    void Start()
    {
        // heroMain.FireRateOffet = 0.25f;
        // heroMain.DmgOffset = 10;
        tfire = GetComponent<TroopAttack>();
        thealth = GetComponent<TroopHealth>();
        timer = GetComponent<TaskTimer>();
        movement = GetComponent<UnityEngine.AI.NavMeshAgent>();
        uTasks = heroMain.UpgradeTasks;
        heroMain.HeroPos = transform;

        var projectileData =  gameObject
                              .GetComponent<TroopAttack>().Projectile
                              .GetComponent<TroopLaserMovement>();
        projectileData.HeroFire = true;
        var troopData = projectileData.OBGResearch;
            
        //Setting initial data for damage upgrades
        heroDmgPoints = troopData.HeroMakeDamagePoints;   
        fireRateOffset = (gameObject.GetComponent<TroopAttack>().FireRate / 2);  
        dmgOffset = (heroDmgPoints / 2);         
        fireBoostText.text = "Auto boost firerate(+" + heroMain.FireRateOffet + ")\ndamage (+" + heroMain.DmgOffset + ")";
        heroMain.OldFireRate = oldTroopFireRate;
        heroAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         detectionTroopSphere();
         updateDataText();
         //Check if needed to boost attack fire rate and damage points
         autoBoostAttack();

        if (startAbilityColdown)
        {
                coolDownParticles.SetActive(true);
               if (timer.FinishedTask)
               {
                  bombBtnText.text =  "Bomb storm";
                  coolDownParticles.SetActive(false);
                  startAbilityColdown = false;
                  bombBtn.GetComponent<Button>().interactable = true;
               }
               else 
               {
                    bombBtnText.text = "Cooldown\n(" + Mathf.Round(timer.TimeStart) + ")";
               }
        }
        if (spawnBombs)
        {
            bombBtnText.text = "Spawning bombs\n(" + Mathf.Round(timer.TimeStart) + ")";
            if(timer.FinishedTask)
            {
                bombBtnText.text = "Bomb storm";
                spawnBombs = false;
                //shieldWallBtn.GetComponent<Button>().interactable = true;
                heroAnimator.SetBool("ready", false);
                startAbilityColdown = true;
                coolDownTime = (taskTime * 2) - diffCooldown;
                startAbilityColdown = true; 
                timer.startTimer(coolDownTime); 
            }
                   
        }
    }
     private void detectionTroopSphere()
    {
        //To check if player troops are in range
        heroMain.detectIfObjectsInRange(scannerRadius, troopTag);

        //To check if player troops are out of range
        heroMain.detectIfObjectsOutOfRange(scannerRadius, troopTag, "rogue");

        // //Boost shields to near by troops
        // if (heroType == "defender")
        //     autoBoostTroopShields();
        // else if (heroType == "rogue")
        //     autoBoostAttack();
    }
     private void updateDataText()
    {
          heroHealthCharText.text = ("\nHealth:" +  thealth.UnitHP + ", Shield(SH):" + thealth.ShieldHealth + 
                             "\nSH regeneration time:" + (Mathf.Round(thealth.MaxShield * thealth.ShieldRegTime)) + " s\nMovement speed:" + movement.speed);
          abilityStatsText.text = ("Ability\ncharacteristics:\nBomb damage: " +  bomb.GetComponent<Bomb>().DamagePoints);
    }
    private void autoBoostAttack() //For boosting auto attack
    {
        if (uTasks == null || uTasks.Count > 0)
        {
            foreach (var utask in uTasks)
            {
                if (!utask.IsStatsBoosted)
                {
                    var troopAttackController = utask.Unit.GetComponent<TroopAttack>();
                    //Boosting fire rate  
                    // if (heroMain.OldFireRate <= 0)
                    //     heroMain.OldFireRate = troopAttackController.FireRate;                 
                    heroMain.NewFireRate = troopAttackController.FireRate - fireRateOffset;
                    
                    troopAttackController.FireRate = heroMain.NewFireRate;
                   
                    //Boosing projectile damage points
                    var ctProjectile = troopAttackController.Projectile;
                    var projectileController = ctProjectile.GetComponent<TroopLaserMovement>();
                    var proData = projectileController.OBGResearch;
                    heroMain.OldDamagePoints = proData.SDamage;                    
                    heroMain.NewDamagePoints = proData.SDamage + dmgOffset;
                    proData.SDamage = heroMain.NewDamagePoints;
                    
                   
                    utask.IsStatsBoosted = true;
                }
            }
        }
    }
    //Bomb thoring logic
    public void doBombardment()
    {
        if (tfire.SpottedEnemy)
        {
            spawnBombs = true;
            bombBtn.GetComponent<Button>().interactable = false;
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
        bombBtnText.text = "No enemy spotted";
        originalBombBtnColor = bombBtn.GetComponent<Image>().color;
        bombBtn.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(errorMessageDelay);
        bombBtnText.text = "Bomb storm";
        bombBtn.GetComponent<Image>().color = originalBombBtnColor;
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
