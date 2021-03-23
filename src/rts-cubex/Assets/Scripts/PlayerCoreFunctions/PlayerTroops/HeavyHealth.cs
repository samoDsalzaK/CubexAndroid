using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeavyHealth : MonoBehaviour
{
    [SerializeField] private int unitHP;
    public GameObject health;
    [SerializeField] Image healthBarForeground;
    [SerializeField] Image healthBarBackground;
    [SerializeField] int scoreForEnemy = 20;

    [SerializeField] int regenerationAmount = 3;
    private bool isShot = false;
    private Base playerBase;
    [SerializeField] int TroopWeight = 2;
    [SerializeField] ResearchConf upgrade;
    [SerializeField] int maxShield = 300;
    [SerializeField] int shieldHealth;
    [SerializeField] Image shieldForeground;
    [SerializeField] Image shieldBackground;
    [SerializeField] bool canShield;
    void Start()
    {
        shieldHealth = 0;
        health.SetActive(false);
        if (FindObjectOfType<Base>() == null)
        {
            return;
        }
        else
        {
            playerBase = FindObjectOfType<Base>();
        }
        unitHP = upgrade.getHeavyMaxHP();
        healthBarForeground.fillAmount = unitHP / upgrade.getHeavyTroopScalingCoef();
    }
    void Update()
    {
        if (unitHP < upgrade.getHeavyMaxHP())
        {
            health.SetActive(true);
            StartCoroutine(RegenerateHealth());
        }
        if (unitHP >= upgrade.getHeavyMaxHP())
        {
            health.SetActive(false);
        }
        if (shieldHealth < maxShield && canShield)
        {
            health.SetActive(true);
            StartCoroutine(GenerateShield());
        }
        healthBarForeground.fillAmount = unitHP / upgrade.getHeavyTroopScalingCoef();
        shieldForeground.fillAmount = shieldHealth / upgrade.getHeavyTroopShieldScalingCoef();
        if (unitHP <= 0)
        {
            Destroy(gameObject);
            playerBase.addPlayerTroopsAmount(-TroopWeight);
            var var = upgrade.getHeavyTroopLevel();
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
        healthBarForeground.fillAmount = unitHP / upgrade.getHeavyTroopScalingCoef();
        shieldForeground.fillAmount = shieldHealth / upgrade.getHeavyTroopShieldScalingCoef();
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
            while (unitHP < upgrade.getHeavyMaxHP() && !isShot)
            {
                unitHP += regenerationAmount;
                yield return new WaitForSeconds(0.3f);
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
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}