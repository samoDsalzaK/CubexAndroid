using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopHealth : MonoBehaviour
{
    [SerializeField]  int unitHP;
    public GameObject health;
    [SerializeField] Image healthBarForeground;
    [SerializeField] Image healthBarBackground;
    [SerializeField] ResearchConf upgrade;
    private bool isShot = false;
    [SerializeField] int scoreForEnemy = 10;
    [SerializeField] int regenerationAmount = 1;
    private bool oneTimeGetHP = false;
    private Base playerBase;
    [SerializeField] int TroopWeight = 1;
    [SerializeField] int maxShield = 100;
    [SerializeField] int shieldHealth;
    [SerializeField] Image shieldForeground;
    [SerializeField] Image shieldBackground;
    [SerializeField] bool canShield;
    [SerializeField] bool isHero = false;
    [SerializeField] bool nearHero = false;
    [SerializeField] GameObject heroEffectParticles;
    [SerializeField] float shieldRegTime = 0.3f;
    [SerializeField] Text heroCharText;
    public bool NearHero { set { nearHero = value; } get { return nearHero; }}
    public int UnitHP { get { return unitHP; }}
    public int ShieldHealth { set { shieldHealth = value; } get { return shieldHealth; }}
    public int MaxShield { set { maxShield = value; } get { return maxShield; }}
    public float ShieldRegTime { set {shieldRegTime = value; } get {return shieldRegTime;}}
    void Start()
    {
        if (isHero)
        {
            shieldHealth = maxShield;
            
        }
        else
        {
            shieldHealth = 0;
            
        }
        health.SetActive(false);
        if (FindObjectOfType<Base>() == null)
        {
            return;
        }
        else
        {
            playerBase = FindObjectOfType<Base>();
        }
        //upgrade = FindObjectOfType<Research> ();
        if (isHero)
            unitHP = upgrade.SHeroMaxHP;
        else
            unitHP = upgrade.getMaxHP();
        //healthBar.sizeDelta = new Vector2 (unitHP * scalingCoef, healthBar.sizeDelta.y);
        healthBarForeground.fillAmount = unitHP / upgrade.getLightTroopScalingCoef();
    }
    void Update()
    {
        if(nearHero && !isHero)
        {            
            if (heroEffectParticles)
                heroEffectParticles.SetActive(true);
            health.SetActive(true);
        }
        else
        {
             if (heroEffectParticles)
                heroEffectParticles.SetActive(false);
            health.SetActive(false);
        }
        if ((FindObjectOfType<Research>() != null) && !isHero)
        {
            if (!oneTimeGetHP)
            {
                oneTimeGetHP = true;
                unitHP = upgrade.getMaxHP();
            }
            if (unitHP < upgrade.getMaxHP())
            {
                health.SetActive(true);
                StartCoroutine(RegenerateHealth());
            }
            if (unitHP >= upgrade.getMaxHP())
            {
                health.SetActive(false);
            }
            if (shieldHealth <= 0) //For hero. If there are problems pls contact me!!:) Pls check if it works with troop
            {
                shieldHealth = 0;
                canShield = false;
            }
            if (shieldHealth < maxShield && canShield)
            {
                health.SetActive(true);
                StartCoroutine(GenerateShield());
            }
            healthBarForeground.fillAmount = unitHP / upgrade.getLightTroopScalingCoef();
            shieldForeground.fillAmount = shieldHealth / upgrade.getLightTroopShieldScalingCoef();
            if (unitHP <= 0)
            {
                Destroy(gameObject);
                playerBase.addPlayerTroopsAmount(-TroopWeight);
                var var = upgrade.getTroopLevel();
                if (var == 0)
                {
                    FindObjectOfType<GameSession>().AddEnemyScorePoints(scoreForEnemy);
                }
                else if (var == 1)
                {
                    FindObjectOfType<GameSession>().AddEnemyScorePoints(2 * scoreForEnemy);
                }
                else
                {
                    FindObjectOfType<GameSession>().AddEnemyScorePoints(3 * scoreForEnemy);
                }
            }
        }
        else
        {
            if (unitHP < (isHero ? upgrade.SHeroMaxHP : upgrade.getMaxHP()))
            {
                if (!nearHero)
                    health.SetActive(true);
                StartCoroutine(RegenerateHealth());
            }
            if (unitHP >=  (isHero ? upgrade.SHeroMaxHP : upgrade.getMaxHP()))
            {
                if (!nearHero)
                     health.SetActive(false);
            }
            if (shieldHealth < maxShield && canShield)
            {
                health.SetActive(true);
                StartCoroutine(GenerateShield());
            }
            healthBarForeground.fillAmount = unitHP / upgrade.getLightTroopScalingCoef();
            shieldForeground.fillAmount = shieldHealth / upgrade.getLightTroopShieldScalingCoef();
            if (unitHP <= 0)
            {
                Destroy(gameObject);
                if (playerBase)
                    playerBase.addPlayerTroopsAmount(-TroopWeight);
            }
        }
    }
    /* Health bar is hidden by default, it only appears when the units, structures HP is not full and after some time, if not attacked again,
    Health starts to regenerate slowly and when HP is full, the Health Bar again hides itself. Also, if HP decreases down to 0, the Health holder
    is destroyed (death) */
    public void decreaseHealth(int damage)
    {
        if (shieldHealth <= 0)
        {
            unitHP -= damage;
        }
        else
        {
            shieldHealth -= damage;
        }
        if (FindObjectOfType<Research>() != null)
        {
            healthBarForeground.fillAmount = unitHP / upgrade.getLightTroopScalingCoef();
            shieldForeground.fillAmount = shieldHealth / upgrade.getLightTroopShieldScalingCoef();
        }
        else
        {
            healthBarForeground.fillAmount = unitHP / upgrade.getDamage();
            shieldForeground.fillAmount = shieldHealth / upgrade.getDamage();
        }
        isShot = true;
    }
    public void setHP(int HP)
    {
        unitHP += HP;
    }
    public void setCanShield(bool can)
    {
        canShield = can;
    }
    IEnumerator RegenerateHealth()
    {
        int x = unitHP;
        yield return new WaitForSeconds(15);
        if (x == unitHP)
        {
            isShot = false;
            if (FindObjectOfType<Research>() != null)
            {
                while (unitHP < (isHero ? upgrade.SHeroMaxHP : upgrade.getMaxHP()) && !isShot)
                {
                    unitHP += regenerationAmount;
                    yield return new WaitForSeconds(0.3f);
                }
            }
            else
            {
                while (unitHP < (isHero ? upgrade.SHeroMaxHP : upgrade.getMaxHP()) && !isShot)
                {
                    unitHP += regenerationAmount;
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }
    }

    IEnumerator GenerateShield()
    {
        int x = shieldHealth;
        yield return new WaitForSeconds(2);
        if (x == shieldHealth)
        {
            isShot = false;
            while (shieldHealth < maxShield && !isShot && canShield)
            {
                shieldHealth += regenerationAmount;
                yield return new WaitForSeconds(shieldRegTime);
            }
        }
    }
}