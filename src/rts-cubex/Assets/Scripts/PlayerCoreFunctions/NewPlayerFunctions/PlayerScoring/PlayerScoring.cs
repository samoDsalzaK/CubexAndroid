using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoring : MonoBehaviour
{
    [SerializeField] int[] pointsEarnedForWorkerCreation;
    [SerializeField] int[] pointsEarnedForPlayerBaseUpgrade;
    [SerializeField] int[] pointsEarnedTurretPlayerBaseUpgrade;
    [SerializeField] int[] pointsEarnedForMiningStationUpgrade;
    [SerializeField] int[] pointsEarnedForBuildingResearchUpgrade;
    [SerializeField] int[] pointsEarnedForTroopsResearchUpgrade;
    [SerializeField] int[] pointsEarnedForShrineUpgrade;
    [SerializeField] int pointsEarnedForBuildingCreation;
    [SerializeField] int pointsEarnedForEnemyDestoyment;
    [SerializeField] int pointsEarnedForEnemyBaseDestoyment;
    [SerializeField] int pointsEarnedForHeroCreation;
    [SerializeField] int pointsEarnedForCreditsReedem; // will depend on mined credits amount
    [SerializeField] int[] pointsEarnedForAdditionalWorker;
    [SerializeField] int pointsEarnedForLootBox;

    GameSession gameSession;

    createAnimatedPopUp animatedPopUps;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        animatedPopUps = GetComponent<createAnimatedPopUp>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //func for adding score after worker creation
    public void addScoreAfterWorkerCreation(int index){ // index - current amout of build workers
        if (index >=4){
            index = 4;
        }
        gameSession.AddPlayerScorePoints(pointsEarnedForWorkerCreation[index-1]);
        animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForWorkerCreation[index-1]);
        gameSession.addPlayerWorkersAmount(1);
    }

    // func for adding score after building addional worker
    public void addScoreAferAdditionalWorker(int index){
        if (index >=4){
            index = 4;
        }
        gameSession.AddPlayerScorePoints(pointsEarnedForAdditionalWorker[index-1]);
        animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForAdditionalWorker[index-1]);
    }

    // func for adding points after base upgrade
    public void addScoreAfterBaseUpgrade(int index){
        if (index >= 5){
            index = 5;
        }
        else if (index <= 1){
            index = 2;
        }
        gameSession.AddPlayerScorePoints(pointsEarnedForPlayerBaseUpgrade[index-2]);
        animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForPlayerBaseUpgrade[index-2]);
    }

    // func for adding player score after mined credits reedem
    public void addScoreAfterCreditsReedem(int points){
        if (points == 1){
            pointsEarnedForCreditsReedem = 1;
        }
        else{
            pointsEarnedForCreditsReedem = points / 2;
        }
        gameSession.AddPlayerScorePoints(pointsEarnedForCreditsReedem);
        animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForCreditsReedem);
    }

    // func for adding score points after structure build
    public void addScoreAfterStructureBuild(string type){
        switch(type){
            case "barrack":
                gameSession.addPlayerBarrackAmount(1);
                gameSession.AddPlayerScorePoints(pointsEarnedForBuildingCreation);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForBuildingCreation);
            break;
            case "turret":
                gameSession.addPlayerTurretAmount(1);
                gameSession.AddPlayerScorePoints(pointsEarnedForBuildingCreation);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForBuildingCreation);
            break;
            case "creditsMiningStation":
                gameSession.addPlayerMiningStationAmount(1);
                gameSession.AddPlayerScorePoints(pointsEarnedForBuildingCreation);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForBuildingCreation);
            break;
            case "armyCamp":
                gameSession.addPlayerArmyCampAmount(1);
                gameSession.AddPlayerScorePoints(pointsEarnedForBuildingCreation);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForBuildingCreation);
            break;
            case "buildingResearch":
                gameSession.addPlayerResearchAmount(1);
                gameSession.AddPlayerScorePoints(pointsEarnedForBuildingCreation);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForBuildingCreation);
            break;
            case "troopsResearch":
                gameSession.addPlayerResearchAmount(1);
                gameSession.AddPlayerScorePoints(pointsEarnedForBuildingCreation);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForBuildingCreation);
            break;
            case "shrine":
                gameSession.addPlayerShrineAmount(1);
                gameSession.AddPlayerScorePoints(pointsEarnedForBuildingCreation);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForBuildingCreation);
            break;
            case "energonCollector":
                gameSession.addPlayerCollectorAmount(1);
                gameSession.AddPlayerScorePoints(pointsEarnedForBuildingCreation);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForBuildingCreation);
            break;
            default:
            break;
        }
    }

    // func for adding score after structure upgrade
    public void addScoreAfterStructureUpgrade(string type, int index){
        switch(type){
            case "turret":
                if (index >= 5){
                    index = 5;
                }
                else if (index <= 1){
                    index = 2;
                }
                gameSession.AddPlayerScorePoints(pointsEarnedTurretPlayerBaseUpgrade[index-2]);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedTurretPlayerBaseUpgrade[index-2]);
            break;
            case "buildingResearch":
                if (index >= 5){
                    index = 5;
                }
                else if (index <= 1){
                    index = 2;
                }
                gameSession.AddPlayerScorePoints(pointsEarnedForBuildingResearchUpgrade[index-2]);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForBuildingResearchUpgrade[index-2]);
            break;
            case "troopsResearch":
                if (index >= 5){
                    index = 4;
                }
                else if (index <= 1){
                    index = 1;
                }
                gameSession.AddPlayerScorePoints(pointsEarnedForTroopsResearchUpgrade[index-1]);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForTroopsResearchUpgrade[index-1]);
            break;
            case "creditsMiningStation":
                if (index >= 5){
                    index = 5;
                }
                else if (index <= 1){
                    index = 2;
                }
                gameSession.AddPlayerScorePoints(pointsEarnedForMiningStationUpgrade[index-2]);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForMiningStationUpgrade[index-2]);
            break;
            case "shrine":
                if (index <= 1){
                    index = 1;
                }
                else if (index > 2){
                    index = 2;
                }
                gameSession.AddPlayerScorePoints(pointsEarnedForShrineUpgrade[index-1]);
                animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForShrineUpgrade[index-1]);
            break;
            default:
            break;
        }
    }

    // func for adding score points after enemy destroyment
    public void addScoreAfterEnemyDestoy(){
        if (gameSession)
        {
            gameSession.AddPlayerScorePoints(pointsEarnedForEnemyDestoyment);
            animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForEnemyDestoyment);
        }
    }

    // func for adding score points after enemy destroyment
    public void addScoreAfterEnemyBaseDestoy(){
        if (gameSession)
        {
            gameSession.AddPlayerScorePoints(pointsEarnedForEnemyBaseDestoyment);
            animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForEnemyBaseDestoyment);
        }
    }

    // func for adding score after hero unit creation
    public void addScoreAfterHeroCreation(){
        if (gameSession)
        {
            gameSession.AddPlayerScorePoints(pointsEarnedForHeroCreation);
            animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForHeroCreation);
        }
    }

    // func for addding score after opened loot box
    public void addScoreAfterOpenedLootBox(){
        if (gameSession)
        {
            gameSession.AddPlayerScorePoints(pointsEarnedForLootBox);
            animatedPopUps.createAddPlayerScorePointsPopUp(pointsEarnedForLootBox);
        }
    }
}
