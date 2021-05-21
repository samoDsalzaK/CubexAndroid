using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setFidexAmountOfStructures : MonoBehaviour
{
    [Header("Configuration script to set fixed amount of structures in game level")]

    // button's components
    [SerializeField] TurretBuild turretBtn;
    [SerializeField] BarrackBuild barrackBtn; 
    [SerializeField] ResearchCentreBuild researchCenterBtn;
    [SerializeField] BuildTroopsResearchCenter troopsCenterBtn;
    [SerializeField] MiningStationBuild creditsMiningStationBtn; // for credits mining station
	[SerializeField] buildArmyCamp armyCampBuildBtn;
	[SerializeField] buildShrine shrineBuildBtn;

    // current amount of structures in game level
    [SerializeField] int playerBarracksAmountInLevel;
    [SerializeField] int playerArmyCampAmountInLevel;
    [SerializeField] int playerTurretAmountInLevel;
    [SerializeField] int playerCreditsMiningStationAmountInLevel;
    [SerializeField] int playerShrineAmountInLevel;
    [SerializeField] int playerWallsAmountInLevel; 
    [SerializeField] int playerBuildingResearchAmountInLevel;
    [SerializeField] int playerTroopsResearchAmountInLevel;

    // max available amount of structures in game level
    [SerializeField] int playerMaxBarrackAmountInLevel;
    [SerializeField] int playerMaxArmyCampAmountInLevel;
    [SerializeField] int playerMaxTurretAmountInLevel;
    [SerializeField] int playerMaxCreditsMiningStationAmountInLevel;
    [SerializeField] int playerMaxShrineAmountInLevel;
    [SerializeField] int playerMaxWallsAmountInLevel;
    [SerializeField] int playerMaxBuildingResearchAmountInLevel;
    [SerializeField] int playerMaxTroopsResearchAmountInLevel;  

    // getter and setter for config params - actual amount in game level
    public int changePlayerBarrackAmountInLevel {set {playerBarracksAmountInLevel = value;} get {return playerBarracksAmountInLevel;}}
    public int changePlayerArmyCampAmountInLevel {set {playerArmyCampAmountInLevel = value;} get {return playerArmyCampAmountInLevel;}}
    public int changePlayerTurretAmountInLevel {set {playerTurretAmountInLevel = value;} get {return playerTurretAmountInLevel;}}
    public int changePlayerCreditsMiningStationAmountInLevel {set {playerCreditsMiningStationAmountInLevel = value;} get {return playerCreditsMiningStationAmountInLevel;}}
    public int changePlayerShrineAmountInLevel {set {playerShrineAmountInLevel = value;} get {return playerShrineAmountInLevel;}}
    public int changePlayerWallsAmountInLevel {set {playerWallsAmountInLevel = value;} get {return playerWallsAmountInLevel;}}
    public int changePlayerBuildingResearchAmountInLevel {set {playerBuildingResearchAmountInLevel = value;} get {return playerBuildingResearchAmountInLevel;}}
    public int changePlayerTroopsResearchAmountInLevel {set {playerTroopsResearchAmountInLevel = value;} get {return playerTroopsResearchAmountInLevel;}}

    // getter and setter for config params - max available
    public int getMaxPlayerBarrackAmountInLevel {get {return playerMaxBarrackAmountInLevel;}}
    public int getMaxPlayerArmyCampAmountInLevel {get {return playerMaxArmyCampAmountInLevel;}}
    public int getMaxPlayerTurretAmountInLevel {get {return playerMaxTurretAmountInLevel;}}
    public int getMaxPlayerCreditsMiningStationAmountInLevel {get {return playerMaxCreditsMiningStationAmountInLevel;}}
    public int getMaxPlayerShrineAmountInLevel {get {return playerMaxShrineAmountInLevel;}}
    public int getMaxPlayerWallsAmountInLevel {get {return playerMaxWallsAmountInLevel;}}
    public int getMaxPlayerBuildingResearchAmountInLevel {get {return playerMaxBuildingResearchAmountInLevel;}}
    public int getMaxPlayerTroopsResearchAmountInLevel {get {return playerMaxTroopsResearchAmountInLevel;}}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeBuildStructureButton(int index){
        switch(index){
            // player troops barrack
            case 1: 
                barrackBtn.canBuildAgain(true);
            break;
            // player army camp
            case 2:
                armyCampBuildBtn.canBuildAgain(true);
            break;
            // player shrine 
            case 3:
                shrineBuildBtn.canBuildAgain(true);
            break;
            // player turret
            case 4:
                turretBtn.canBuildAgain(true);
            break;
            // player walls
            case 5:
            break;
            // player building research
            case 6:
                researchCenterBtn.canBuildAgain(true);
            break;
            // player troops research
            case 7:
                troopsCenterBtn.canBuildAgain(true);
            break;
            // player credits mining station
            case 8:
                creditsMiningStationBtn.canBuildAgain(true);
            break;
            default:
            break;
        }
    }
}
