using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class ShrineUpgradeManager : MonoBehaviour
{
    //NOTE:
    //FIX Upgrade prices, make sure not to used hardcoded values
    [Header("Upgrade UI configuration")]
    [SerializeField] List<GameObject> level01btns = new List<GameObject>();
    [SerializeField] List<GameObject> level02btns = new List<GameObject>();
    //[SerializeField] List<Button> addButtons = new List<Button>();
    private Shrine shrine;
    private List<HeroUnitToTrain> heroChars = new List<HeroUnitToTrain>();
    private bool displayUpgrades = false;

    void Start()
    {
        shrine = GetComponent<Shrine>();
        heroChars = shrine.HToTrain;
        //print(shrine.ResearchLevel);
    }

    // Update is called once per frame
    void Update()
    {
        switch(shrine.ResearchLevel)
        {
            case 5:
                //Unlocks Level 01 upgrades
                
                // if (!displayedUpgrades)
                // {
                    // foreach (var b in addButtons)
                    // {
                    //     b.interactable = true;
                    // }
                    foreach(var b in level01btns)
                    {
                        b.GetComponent<Button>().interactable = true;
                    }
                    //displayedUpgrades = true;
               // }
            break;

            case 10: 
                //Unlocks Level 02 upgrades 
                // if (!displayedUpgrades)
                // {
                    // foreach (var b in addButtons)
                    // {
                    //     b.interactable = true;
                    // }              
                    foreach(var b in level02btns)
                    {
                        b.GetComponent<Button>().interactable = true;
                    }
                  //  displayedUpgrades = true;
              //  }
            break;
        }
    }
    public void applyUpgrade(int uIndex)
    {
            GameObject defenderHero = null;
            GameObject rogueHero = null;

            var existingHeroes = GameObject.FindGameObjectsWithTag("Unit");
            if (existingHeroes.Length > 0)
            {
                foreach(var h in existingHeroes)
                {
                    var heroCont = h.GetComponent<HeroUnit>();
                    if (heroCont)
                    {
                        if (heroCont.HeroType == "defender")
                        {
                            defenderHero = h;
                        }
                        else if (heroCont.HeroType == "rogue")
                        {
                            rogueHero = h;
                        }
                    }
                }
            }
            //Level 01 upgrades
            if (uIndex == 1)
            {
                if(!canDoTask(50, 60, uIndex))
                {
                    return;
                }
                //reduce shield regeneration
                var defenderData = heroChars[0];
                var defenderModel = defenderData.Hero;

                //Current shield reg time
                var cRegTime = defenderData.Hero.GetComponent<TroopHealth>().ShieldRegTime;
                defenderData.BoostShieldRegTime = (cRegTime * 2) / 3; 

                if (defenderData.BoostShieldRegTime > 0) //Fix it later
                    level01btns[0].SetActive(false);


                //Apply upgrade to exising spawnedHero
                if (defenderHero)
                {
                    defenderHero.GetComponent<TroopHealth>().ShieldRegTime -= Mathf.Abs(defenderData.BoostShieldRegTime); //Bug with shield reg
                }


            }

           else if (uIndex == 2) //Rogue speed increase
           {
                
                if(!canDoTask(60, 70, uIndex))
                {
                    return;
                }
                var rogueData = heroChars[1];
                var rogueModel = rogueData.Hero;

                var cMovementSpeedBoost = rogueData.Hero.GetComponent<NavMeshAgent>().speed;
                rogueData.MovementSpeedBoost = (cMovementSpeedBoost * 2) /4;

                if (rogueData.MovementSpeedBoost > 0)
                    level01btns[1].SetActive(false);

                if(rogueHero)
                    rogueHero.GetComponent<NavMeshAgent>().speed += rogueData.MovementSpeedBoost;
            }

            //Level 02 upgrades
            else if (uIndex == 3)
            {
                //Boost wall time               
                if(!canDoTask(80, 50, uIndex))
                {
                    return;
                }
                var defenderData = heroChars[0];
                var defenderModel = defenderData.Hero;

                var cWallTime = defenderData.Hero.GetComponent<HeroUnit>().WallBarrackExistTime;
                defenderData.WallTimeBoost = cWallTime * 2;

                 if (defenderData.WallTimeBoost > 0)  //Fix it later
                    level02btns[0].SetActive(false);

                if (defenderHero)
                    defenderHero.GetComponent<HeroUnit>().WallBarrackExistTime += defenderData.WallTimeBoost;
            }

          

            else if (uIndex == 4) //Stronger bombs upgrades
            {
                
                if(!canDoTask(90, 80, uIndex))
                {
                    return;
                }
                var rogueData = heroChars[1];
                var rogueModel = rogueData.Hero;

                var cDamagePoints = rogueData
                                    .Hero.GetComponent<HeroUnit>()
                                    .Bomb.GetComponent<Bomb>().DamagePoints;
                rogueData.BombDamagePoints = (cDamagePoints * 2) / 4;

                if (rogueData.BombDamagePoints > 0)
                    level02btns[1].SetActive(false);

                if (rogueHero)
                    rogueHero.GetComponent<HeroUnit>().Bomb.GetComponent<Bomb>().DamagePoints += rogueData.BombDamagePoints;
            }
        
    }
    private bool canDoTask(int eprice, int cprice, int uindex)
    {
        if (shrine.Energon < eprice || shrine.Credits < cprice)
        {
            print("ERROR not enough funds for this upgrade!");
            return false;
        }
        else
        {
            shrine.Energon -= eprice;
            shrine.Credits -= cprice;
            print("Doing upgrade task(" + uindex + ")");
            return true;
        }
    }
}
