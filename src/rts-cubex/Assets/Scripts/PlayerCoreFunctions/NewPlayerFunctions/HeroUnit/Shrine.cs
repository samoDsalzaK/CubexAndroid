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
    //HeroClasses hClass = HeroClasses.Defender;

    private void OnMouseDown() {
        mainUIWindow.SetActive(true);
    }

    void Start()
    {
        timer = GetComponent<TaskTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update Shrine data GUI text
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
                var heroData = spawnedHero.GetComponent<HeroUnit>();
                if (heroData.HeroType == "defender")
                {                    
                    spawnedHero.GetComponent<TroopHealth>().ShieldRegTime -= heroToTrain.BoostShieldRegTime;
                    heroData.WallBarrackExistTime += heroToTrain.WallTimeBoost;
                }
                else if (heroData.HeroType == "rogue")
                {
                    spawnedHero.GetComponent<NavMeshAgent>().speed += heroToTrain.MovementSpeedBoost;
                    heroData.Bomb.GetComponent<Bomb>().DamagePoints += heroToTrain.BombDamagePoints;
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
        switch(unitClassIndex)
        {
            case 1:
                if (isTesting)
                {
                    var heroToTrainUnit = hToTrain[(int)HeroClasses.Defender];
                    if (credits < heroToTrainUnit.CreditsPrice || 
                        energon < heroToTrainUnit.EnergonPrice)
                    {
                        //Print error msg
                        printError(heroToTrainUnit);                        
                    }
                    else
                    {
                        // Start training hero
                        //timer.startTimer(heroToTrain.TimeToTrain);
                        credits -= heroToTrainUnit.CreditsPrice;
                        energon -= heroToTrainUnit.EnergonPrice;
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
                        
                        
                    }
                }
                else
                {
                    //Add player base resources checking logic
                    break;
                }
            break;

            case 2:
                if (isTesting)
                {                    
                    var heroToTrainUnit = hToTrain[(int)HeroClasses.Rogue];
                    if (credits < heroToTrainUnit.CreditsPrice || 
                        energon < heroToTrainUnit.EnergonPrice)
                    {
                        //Print error msg
                        printError(heroToTrainUnit);                        
                    }
                    else
                    {
                        // Start training hero
                        //timer.startTimer(heroToTrain.TimeToTrain);
                        credits -= heroToTrainUnit.CreditsPrice;
                        energon -= heroToTrainUnit.EnergonPrice;
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
                        
                        
                    }
                }
                else
                {
                    //Add player base resources checking logic
                    break;
                }
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
    }
    private void updateText()
    {
        mrlevel.text = "Military Research Level(MRLevel) " + researchLevel;
    }
   
}
