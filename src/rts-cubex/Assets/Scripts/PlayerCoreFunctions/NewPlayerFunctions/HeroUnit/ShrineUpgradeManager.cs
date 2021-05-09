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
    [SerializeField] List<GameObject> infoPurchaseBtn = new List<GameObject>();
    [SerializeField] GameObject errorWindow;
    [SerializeField] Text errorText;
    //[SerializeField] List<Button> addButtons = new List<Button>();
    private Shrine shrine;
    private List<HeroUnitToTrain> heroChars = new List<HeroUnitToTrain>();
    private bool displayUpgrades = false;
    private createAnimatedPopUp animatedPopUps;
    void Start()
    {
        shrine = GetComponent<Shrine>();
        heroChars = shrine.HToTrain;
        if (!shrine.IsTesting)
            animatedPopUps = GameObject.Find("PlayerBase").GetComponent<createAnimatedPopUp>();
        //print(shrine.ResearchLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (shrine.ResearchLevel >= 5)
        {
            foreach(var b in level01btns)
            {
                b.GetComponent<Button>().interactable = true;
            }
        }
        if (shrine.ResearchLevel >= 10)
        {
            foreach(var b in level02btns)
            {
                b.GetComponent<Button>().interactable = true;
            }
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
                    //var heroCont = h.GetComponent<HeroUnit>();
                    // if (heroCont)
                    // {
                        if (h.GetComponent<DefenderHero>())
                        {
                            defenderHero = h;
                        }
                        else if (h.GetComponent<RogueHero>())
                        {
                            rogueHero = h;
                        }
                   // }
                }
            }
            //Level 01 upgrades
            if (uIndex == 1)
            {
                var resourcePrice = level01btns[uIndex - 1].GetComponent<UpgradePrice>();
                if(!canDoTask(resourcePrice.Energon, resourcePrice.Credits, uIndex))
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
                {
                    print("Upgrade task("+uIndex+") done!");
                    infoPurchaseBtn[uIndex - 1].SetActive(true);    
                    level01btns[0].SetActive(false);
                }

                //Apply upgrade to exising spawnedHero
                if (defenderHero)
                {
                    print("Current shield: " + defenderHero.GetComponent<TroopHealth>().ShieldRegTime);
                    defenderHero.GetComponent<TroopHealth>().ShieldRegTime -= defenderData.BoostShieldRegTime; //Bug with shield reg
                }


            }

           else if (uIndex == 2) //Rogue speed increase
           {
                
                var resourcePrice = level01btns[uIndex - 1].GetComponent<UpgradePrice>();
                if(!canDoTask(resourcePrice.Energon, resourcePrice.Credits, uIndex))
                {
                    return;
                }
                var rogueData = heroChars[1];
                var rogueModel = rogueData.Hero;

                var cMovementSpeedBoost = rogueData.Hero.GetComponent<NavMeshAgent>().speed;
                rogueData.MovementSpeedBoost = (cMovementSpeedBoost * 2) /4;

                if (rogueData.MovementSpeedBoost > 0)
                {
                    print("Upgrade task("+uIndex+") done!");
                    infoPurchaseBtn[uIndex - 1].SetActive(true); 
                    level01btns[1].SetActive(false);
                }
                if(rogueHero)
                    rogueHero.GetComponent<NavMeshAgent>().speed += rogueData.MovementSpeedBoost;
            }

            //Level 02 upgrades
            else if (uIndex == 3)
            {
                //Boost wall time               
                var resourcePrice = level02btns[uIndex - uIndex].GetComponent<UpgradePrice>();
                if(!canDoTask(resourcePrice.Energon, resourcePrice.Credits, uIndex))
                {
                    return;
                }
                var defenderData = heroChars[0];
                var defenderModel = defenderData.Hero;

                var cWallTime = defenderData.Hero.GetComponent<DefenderHero>().WallBarrackExistTime;
                defenderData.WallTimeBoost = cWallTime * 2;

                 if (defenderData.WallTimeBoost > 0)  //Fix it later
                 {
                     print("Upgrade task("+uIndex+") done!");
                    infoPurchaseBtn[uIndex - 1].SetActive(true); 
                    level02btns[0].SetActive(false);
                 }

                if (defenderHero)
                    defenderHero.GetComponent<DefenderHero>().WallBarrackExistTime += defenderData.WallTimeBoost;
            }

          

            else if (uIndex == 4) //Stronger bombs upgrades
            {
                
                var resourcePrice = level02btns[(uIndex - uIndex) + 1].GetComponent<UpgradePrice>();
                if(!canDoTask(resourcePrice.Energon, resourcePrice.Credits, uIndex))
                {
                    return;
                }
                var rogueData = heroChars[1];
                var rogueModel = rogueData.Hero;

                var cDamagePoints = rogueData
                                    .Hero.GetComponent<RogueHero>()
                                    .Bomb.GetComponent<Bomb>().DamagePoints;
                rogueData.BombDamagePoints = (cDamagePoints * 2) / 4;

                if (rogueData.BombDamagePoints > 0)
                {
                    print("Upgrade task("+uIndex+") done!");
                    infoPurchaseBtn[uIndex - 1].SetActive(true); 
                    level02btns[1].SetActive(false);
                }

                if (rogueHero)
                    rogueHero.GetComponent<RogueHero>().Bomb.GetComponent<Bomb>().DamagePoints += rogueData.BombDamagePoints;
            }
        
    }
    private bool canDoTask(int eprice, int cprice, int uindex)
    {
        if (shrine.IsTesting)
        {
            if (shrine.Energon < eprice || shrine.Credits < cprice)
            {
                errorWindow.SetActive(true);
                errorText.text = "\nNot enough funds for this upgrade!\nRequired:" + eprice + " e, " + cprice + " c";
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
        else
        {
            if (shrine.PlayerBase.getEnergonAmount() < eprice || shrine.PlayerBase.getCreditsAmount() < cprice)
            {
                errorWindow.SetActive(true);
                errorText.text = "\nNot enough funds for this upgrade!\nRequired:" + eprice + " e, " + cprice + " c";
                print("ERROR not enough funds for this upgrade!");
                return false;
            }
            else
            {
                shrine.PlayerBase.setEnergonAmount(shrine.PlayerBase.getEnergonAmount() - eprice);
                animatedPopUps.createDecreaseEnergonPopUp(eprice);
                shrine.PlayerBase.setCreditsAmount(shrine.PlayerBase.getCreditsAmount() -  cprice);
                animatedPopUps.createDecreaseCreditsPopUp(cprice);
                shrine.PlayerBase.GetComponent<PlayerScoring>().addScoreAfterStructureUpgrade("shrine",uindex);
                Debug.Log(uindex);
                print("Doing upgrade task(" + uindex + ")");
                return true;
            }
        }
    }
}
