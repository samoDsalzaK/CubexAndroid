using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
//NOTE: add mini upgrade system
//Add upgrade system values to class
[System.Serializable]
public class HeroUnitToTrain  : System.Object
{    
    [SerializeField] int energonPrice = 0;
    [SerializeField] int creditsPrice = 0;
    [SerializeField] int timeToTrain = 5;
    [SerializeField] bool readyToTrain = false;
    [SerializeField] GameObject hero; //Main hero prefab object
    [SerializeField] Button spawnButton;
    [SerializeField] Text buttonText;
    [SerializeField] bool trainingLocked = false;
    [Header("Upgrade data")] //For hero not to lose upgrades
    [SerializeField] float boostShieldRegTime = 0f; // Amount to give
    [SerializeField] int wallTimeBoost = 0;
    [SerializeField] float movementSpeedBoost = 0; 
    [SerializeField] int bombDamagePoints = 0;
    //[SerializeField] bool avaialbleInMenu = false;
    private string oldButtonText;
    public int EnergonPrice { get {return energonPrice;}}
    public int CreditsPrice { get {return creditsPrice;}}    
    public int TimeToTrain { get { return timeToTrain; }}

    public bool ReadyToTrain { set {readyToTrain = value; } get {return readyToTrain; }} 
    public GameObject Hero { set {hero = value; } get {return hero; }}
    public Button SpawnButton { get {return spawnButton; }}
    public Text ButtonText { get {return buttonText;}}
    public string OldButtonText { set {oldButtonText = value; } get {return oldButtonText; }} 
    public bool TrainingLocked { set {trainingLocked = value; } get {return trainingLocked; }}

    public float BoostShieldRegTime { set {boostShieldRegTime = value; } get {return boostShieldRegTime; }} 
    public int WallTimeBoost { set {wallTimeBoost = value; } get {return wallTimeBoost; }} 
    public float MovementSpeedBoost { set {movementSpeedBoost = value; } get {return movementSpeedBoost; }} 
    public int BombDamagePoints { set {bombDamagePoints = value; } get {return bombDamagePoints; }} 
    //public bool AvaialbleInMenu { set {avaialbleInMenu = value; } get {return avaialbleInMenu; }} 
}

public class Shrine : MonoBehaviour
{
    [SerializeField] GameObject mainUIWindow;
    [SerializeField] int researchLevel = 1; //Use player military research building    
    [SerializeField] List<HeroUnitToTrain> hToTrain = new List<HeroUnitToTrain>();
    [SerializeField] Transform spawnPoint;
    [SerializeField] Text mrlevel;
    [SerializeField] GameObject errorWindow;
    [SerializeField] Text errorText;
    [SerializeField] GameObject selectionCanvas;
    [Header("Testing data")]
    [SerializeField] bool isTesting = false;
    [SerializeField] int credits = 1000;
    [SerializeField] int energon = 1000;

   
    TaskTimer timer;
    [SerializeField] List<int> trainIndexes = new List<int>();
    private int cIndex = 0;
    private bool trainingHero = false;
    private HeroUnitToTrain heroToTrain;
    public List<HeroUnitToTrain> HToTrain {get {return hToTrain;} }
    public int ResearchLevel { set {researchLevel = value;} get {return researchLevel;}}    
    enum HeroClasses { Defender = 0, Rogue = 1}
    private int existingHeroAmount = 0;
    public int Energon { set {energon = value; } get { return energon;}}
    public int Credits { set {credits = value; } get { return credits;}}
    private Base playerBase;
    private Research troopRes;
    public bool IsTesting { get { return isTesting; }}
    public Base PlayerBase { get { return playerBase; }}
    //HeroClasses hClass = HeroClasses.Defender;

    private void OnMouseDown() {
        mainUIWindow.SetActive(true);
        selectionCanvas.SetActive(true);
    }

    void Start()
    {
        timer = GetComponent<TaskTimer>();
        if (!isTesting)
        {
            playerBase = FindObjectOfType<Base>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Update Shrine data GUI text
        if (!isTesting)
        {
            var builtResearch = FindObjectOfType<Research>();
            if (builtResearch)
            {
                researchLevel = builtResearch.OBGResearch.ResearchLevel;
               // print(researchLevel);
            }
        }
        updateText();
        //Check existing heroes in scene
        //manageHeroUpgradesInScene();
        //Checking hero availability
        if (researchLevel < 3)
        {
            var currentBtnText = hToTrain[(int)HeroClasses.Rogue].ButtonText.text;
            if (!hToTrain[(int)HeroClasses.Rogue].TrainingLocked)
            {
                hToTrain[(int)HeroClasses.Rogue].OldButtonText = currentBtnText;
                hToTrain[(int)HeroClasses.Rogue].ButtonText.text = currentBtnText + "\n(Required MRLevel 3)";
                hToTrain[(int)HeroClasses.Rogue].TrainingLocked = true;
                hToTrain[(int)HeroClasses.Rogue].SpawnButton.interactable = false;
            }
           
        }
        else
        { 
            
            if (hToTrain[(int)HeroClasses.Rogue].TrainingLocked)
                hToTrain[(int)HeroClasses.Rogue].SpawnButton.interactable = true;
            
            hToTrain[(int)HeroClasses.Rogue].TrainingLocked = false;
        }
        //Main hero training logic
        if (trainingHero)
        {
           heroToTrain.ButtonText.text = "Training\n("+Mathf.Round(timer.TimeStart)+")";
           if (timer.FinishedTask) 
           {
               //Add upgrade logic to spawned hero
                var spawnedHero = Instantiate(heroToTrain.Hero, spawnPoint.position, heroToTrain.Hero.transform.rotation);
                //Adding upgrades
               // var heroData = spawnedHero.GetComponent<HeroUnit>();
                if (spawnedHero.GetComponent<DefenderHero>())
                {    
                    var defenderHero = spawnedHero.GetComponent<DefenderHero>();                
                    spawnedHero.GetComponent<TroopHealth>().ShieldRegTime -= heroToTrain.BoostShieldRegTime;
                    defenderHero.WallBarrackExistTime += heroToTrain.WallTimeBoost;
                    //print(heroData.WallBarrackExistTime);
                }
                else if (spawnedHero.GetComponent<RogueHero>())
                {
                    var rogueHero = spawnedHero.GetComponent<RogueHero>();
                    spawnedHero.GetComponent<NavMeshAgent>().speed += heroToTrain.MovementSpeedBoost;
                    rogueHero.Bomb.GetComponent<Bomb>().DamagePoints += heroToTrain.BombDamagePoints;
                }
                trainingHero = false;                
                //Lock button 
                 print((cIndex > 1 ? "Rogue" : "Defender") + " spawned!"); //For debug 
                 heroToTrain.ButtonText.text = heroToTrain.OldButtonText;   
                 return;
           }
        }
        //Starting hero training
        if (trainIndexes.Count > 0 && !trainingHero)
        {
            cIndex = trainIndexes[0];
            trainIndexes.Remove(cIndex);
            //print(cIndex);
            heroToTrain = getAHeroToTrain(cIndex - 1);
            timer.startTimer(heroToTrain.TimeToTrain);
            trainingHero = true;            
            return;
        }
    }
    public void train(int unitClassIndex) // 1 - defender, 2 - rogue, 
    {
        HeroUnitToTrain heroToTrainUnit;
        switch(unitClassIndex)
        {
            case 1:
                // if (isTesting)
                // {
                heroToTrainUnit = hToTrain[(int)HeroClasses.Defender];
                if (isTesting)
                {                    
                    
                    if (credits < heroToTrainUnit.CreditsPrice || 
                        energon < heroToTrainUnit.EnergonPrice)
                    {
                        //Print error msg
                        printError(heroToTrainUnit);   
                        break;                     
                    }
                    else
                    {
                        credits -= heroToTrainUnit.CreditsPrice;
                        energon -= heroToTrainUnit.EnergonPrice;
                    }
                }
                else
                {
                    if (playerBase.getCreditsAmount() < heroToTrainUnit.CreditsPrice || 
                        playerBase.getEnergonAmount() < heroToTrainUnit.EnergonPrice)
                    {
                        //Print error msg
                        printError(heroToTrainUnit);   
                        break;                     
                    }
                    else
                    {
                          playerBase.setEnergonAmount(playerBase.getEnergonAmount() - heroToTrainUnit.EnergonPrice);
                          playerBase.setCreditsAmount(playerBase.getCreditsAmount() -  heroToTrainUnit.CreditsPrice);
                    }
                }
                
                heroToTrainUnit.SpawnButton.interactable = false;
                print("Prearing to train hero unit!");
                heroToTrainUnit.OldButtonText = heroToTrainUnit.ButtonText.text;
                if(trainingHero)
                {
                   print("Waiting to train!");
                   heroToTrainUnit.ButtonText.text = "Waiting to\ntrain!";
                }

                trainIndexes.Add((int)HeroClasses.Defender + 1);
                heroToTrainUnit.ReadyToTrain = true;                        
                
            break;

            case 2:
                heroToTrainUnit = hToTrain[(int)HeroClasses.Rogue];
                if (isTesting)
                {                    
                    
                    if (credits < heroToTrainUnit.CreditsPrice || 
                        energon < heroToTrainUnit.EnergonPrice)
                    {
                        //Print error msg
                        printError(heroToTrainUnit);   
                        break;                     
                    }
                    else
                    {
                        credits -= heroToTrainUnit.CreditsPrice;
                        energon -= heroToTrainUnit.EnergonPrice;
                    }
                }
                else
                {
                    if (playerBase.getCreditsAmount() < heroToTrainUnit.CreditsPrice || 
                        playerBase.getEnergonAmount() < heroToTrainUnit.EnergonPrice)
                    {
                        //Print error msg
                        printError(heroToTrainUnit);   
                        break;                     
                    }
                    else
                    {
                          playerBase.setEnergonAmount(playerBase.getEnergonAmount() - heroToTrainUnit.EnergonPrice);
                          playerBase.setCreditsAmount(playerBase.getCreditsAmount() -  heroToTrainUnit.CreditsPrice);
                    }
                }
                                            
                        heroToTrainUnit.SpawnButton.interactable = false;
                        print("Prearing to train hero unit!");
                        heroToTrainUnit.OldButtonText = heroToTrainUnit.ButtonText.text;
                        if(trainingHero)
                        {
                            print("Waiting to train!");
                            heroToTrainUnit.ButtonText.text = "Waiting to train!";
                        }

                        trainIndexes.Add((int)HeroClasses.Rogue + 1);
                        heroToTrainUnit.ReadyToTrain = true;
                        
                        
              
            break;
        }
    }
    HeroUnitToTrain getAHeroToTrain(int index)
    {
        if (hToTrain.Count <= 0)
            return null;

        
        return hToTrain[index];
    }
    private void printError(HeroUnitToTrain unit)
    {
        print("Error: Not enough energon and credits!");
        errorWindow.SetActive(true);
        errorText.text = "Not enough energon and credits!\nRequired: " + unit.EnergonPrice + " e, " + unit.CreditsPrice + " c";
    }
    private void updateText()
    {
        mrlevel.text = "Military Research Level(MRLevel) " + researchLevel;
    }
    private void OnDestroy() {
        var existingHeroUnits = GameObject.FindGameObjectsWithTag("Unit");
        if (existingHeroUnits.Length > 0)
        {
            foreach (var unit in existingHeroUnits)
            {
                // if (unit.GetComponent<HeroUnit>())
                // {
                    //var heroUnit = unit.GetComponent<HeroUnit>();
                    if (unit.GetComponent<DefenderHero>())
                    {    
                        var dH = unit.GetComponent<DefenderHero>();                  
                        var originalHero = hToTrain[0].Hero;
                        unit.GetComponent<TroopHealth>().ShieldRegTime = originalHero.GetComponent<TroopHealth>().ShieldRegTime;
                        dH.WallBarrackExistTime = originalHero.GetComponent<DefenderHero>().WallBarrackExistTime;
                    }
                    if (unit.GetComponent<RogueHero>())
                    {
                        var rH = unit.GetComponent<RogueHero>();
                        var originalHero = hToTrain[1].Hero;
                        unit.GetComponent<NavMeshAgent>().speed = originalHero.GetComponent<NavMeshAgent>().speed;
                        rH.Bomb.GetComponent<Bomb>().DamagePoints = originalHero.GetComponent<RogueHero>().Bomb.GetComponent<Bomb>().OriginalDP;
                    }
               // }
            }
        }
    }
}
