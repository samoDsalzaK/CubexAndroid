using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class HeroUnitToTrain  : System.Object
{    
    [SerializeField] int energonPrice = 0;
    [SerializeField] int creditsPrice = 0;
    [SerializeField] int timeToTrain = 5;
    [SerializeField] bool readyToTrain = false;
    [SerializeField] GameObject hero;
    [SerializeField] Button spawnButton;
    [SerializeField] Text buttonText;
    private string oldButtonText;
    public int EnergonPrice { get {return energonPrice;}}
    public int CreditsPrice { get {return creditsPrice;}}    
    public int TimeToTrain { get { return timeToTrain; }}
    public bool ReadyToTrain { set {readyToTrain = value; } get {return readyToTrain; }} 
    public GameObject Hero { set {hero = value; } get {return hero; }}
    public Button SpawnButton { get {return spawnButton; }}
    public Text ButtonText { get {return buttonText;}}
    public string OldButtonText { set {oldButtonText = value; } get {return oldButtonText; }} 
}

public class Shrine : MonoBehaviour
{
    [SerializeField] GameObject mainUIWindow;
    [SerializeField] int researchLevel = 1; //Use player military research building    
    [SerializeField] List<HeroUnitToTrain> hToTrain = new List<HeroUnitToTrain>();
    [SerializeField] Transform spawnPoint;
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
    enum HeroClasses { Defender = 0, Rogue = 1}
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
        if (trainingHero)
        {
           heroToTrain.ButtonText.text = "Training\n("+Mathf.Round(timer.TimeStart)+")";
           if (timer.FinishedTask) 
           {
                var spawnedHero = Instantiate(heroToTrain.Hero, spawnPoint.position, heroToTrain.Hero.transform.rotation);
                trainingHero = false;
                
                //Lock button 
                 print((cIndex > 1 ? "Rogue" : "Defender") + " spawned!");
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
}
