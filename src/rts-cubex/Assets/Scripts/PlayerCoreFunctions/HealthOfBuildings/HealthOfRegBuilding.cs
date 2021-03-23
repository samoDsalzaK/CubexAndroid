using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthOfRegBuilding : MonoBehaviour
{
    [SerializeField] int healthOfStructureOriginal; // variable which always holds the max value
    [SerializeField] int health; // variable which will be changed if player gets damage from enemy troops
    public GameObject healthOfBuilding;
    [SerializeField] Image healthBarForeground;
    [SerializeField] Image healthBarBackground;
    private bool isShot = false;
    float descreaseHealthBarOfStructure;
    float increaseHealthBarOfStructure;
    [SerializeField] int regenerateHpAmount;
    [SerializeField] int maxShield = 1000;
    [SerializeField] int shieldHealth = 0;
    [SerializeField] Image shieldForeground;
    [SerializeField] Image shieldBackground;
    //int damagePoints = 15;
    void Start()
    {
        healthOfBuilding.SetActive(true);
        descreaseHealthBarOfStructure = 0;
        increaseHealthBarOfStructure = 0;

    }
    void Update()
    {
        if (health < healthOfStructureOriginal)
        {
            StartCoroutine(RegenerateHealth());
        }
        if (shieldHealth < maxShield)
        {
            StartCoroutine(RegenerateShield());
        }
        healthBarForeground.fillAmount = (float)health / (float)healthOfStructureOriginal;
        if (shieldBackground && shieldForeground)
        {
            shieldForeground.fillAmount = (float)shieldHealth / (float)maxShield;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void decreaseHealth(int damagePoints)
    {
        // is pradziu mes turime surasi koks yra tas procentas damage nuo kazkokio pastato hp
        descreaseHealthBarOfStructure = (((float)damagePoints * 100) / (float)healthOfStructureOriginal) / 100;

        if (shieldHealth <= 0)
        {
            health -= damagePoints;
        }
        else
        {
            shieldHealth -= damagePoints;
        }

        //  int 
        // health2 -= (float)damagePoints; // 265 // float
        healthBarForeground.fillAmount -= descreaseHealthBarOfStructure;  // 264.9

        isShot = true;

        descreaseHealthBarOfStructure = 0;

        // damagePoints += 20;
    }
    IEnumerator RegenerateHealth()
    {
        int x = health;
        yield return new WaitForSeconds(15);
        if (x == health)
        {
            isShot = false;
            while (health < healthOfStructureOriginal && !isShot)
            {
                health += regenerateHpAmount; // 300 // int
                increaseHealthBarOfStructure = (((float)regenerateHpAmount * 100) / (float)healthOfStructureOriginal) / 100;
                healthBarForeground.fillAmount += increaseHealthBarOfStructure; // 299.7
                increaseHealthBarOfStructure = 0;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
    IEnumerator RegenerateShield()
    {
        int x = shieldHealth;
        yield return new WaitForSeconds(15);
        if (x == shieldHealth)
        {
            isShot = false;
            while (shieldHealth < maxShield && !isShot)
            {
                shieldHealth += regenerateHpAmount;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
    public int getHealth()
    {
        return health;
    }
    public int getHealthOfStructureOriginal()
    {
        return healthOfStructureOriginal;
    }
    public void setHealthOfStructureOriginal(int newHealth)
    {
        healthOfStructureOriginal = newHealth;
    }
    public void setHealth(int newHealth)
    {
        health = newHealth;
    }
}
