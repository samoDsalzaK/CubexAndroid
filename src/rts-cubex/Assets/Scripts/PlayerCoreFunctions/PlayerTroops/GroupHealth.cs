using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupHealth : MonoBehaviour
{
    [SerializeField] GameObject troop1;
    [SerializeField] GameObject troop2;
    [SerializeField] int groupHealth;
    private TroopHealth troopHealth1;
    private TroopHealth troopHealth2;
    [SerializeField] Image healthBarForeground;
    [SerializeField] Image healthBarBackground;
    [SerializeField] ResearchConf upgrade;
    private float maxHealth;

    void Start()
    {
        troopHealth1 = troop1.GetComponent<TroopHealth>();
        troopHealth2 = troop2.GetComponent<TroopHealth>();
        maxHealth = 2 * upgrade.getLightTroopScalingCoef();
    }

    void Update()
    {
        groupHealth = troopHealth1.UnitHP + troopHealth2.UnitHP;
        if(groupHealth<=0){
            Destroy(gameObject);
        }
        healthBarForeground.fillAmount = (float)groupHealth / 200f;
    }
}
