using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuildArea : MonoBehaviour
{
    private Base playerbase;
    bool canBuild = false;
    [SerializeField] string[] buildingTypes;
    [SerializeField] TurretBuild turretBtn;
    [SerializeField] BarrackBuild barrackBtn;
    [SerializeField] ResearchCentreBuild researchCenterBtn;
    [SerializeField] BuildTroopsResearchCenter troopsCenterBtn;
    [SerializeField] BuildWorker buildWorkerBtn;
    //[SerializeField] BuildPlayerWall buildPlayerWallBtn;
    [SerializeField] WorkerSpawningTimer timerStart;
    [SerializeField] MiningStationBuild creditsMiningStationBtn; // for credits mining station
	[SerializeField] buildArmyCamp armyCampBuildBtn;
	[SerializeField] buildShrine shrineBuildBtn;
    [SerializeField] float buildWorkerStartTime = 5f;
  	//  [SerializeField] GameObject buildRegWorker;
  	//  [SerializeField] GameObject buildDefensive;
	createAnimatedPopUp animatedPopUps;
    //Main build area system
    void Start() 
    {
        if(FindObjectOfType<Base>() == null)
        {
          return;
        }
        else
        {
          playerbase = FindObjectOfType<Base>();
        }
		animatedPopUps =  playerbase.GetComponent<createAnimatedPopUp>();
    }
    void Update()
    {
        //Checks if there aare there any workers in the base area
        if(playerbase.getworkersAmount() > 0) // turiu skailiuka atsakinga uz laisvus workerius
        canBuild = true;

        if(buildWorkerBtn.buildWorker())
        {
        canBuild = true;
        }
    }
    private void OnMouseDown() {
        if(!canBuild) // stai cia mes tikriname ar yra isvis workeriai (nesvarbu ar jie yra laisvi ar ne)
        {
        return; // stai cia as manau nereikia deti panelo, nes vistiek jeigu jam leidzia klikinti reiskes jis jau turi workeri
        }
        //Variable for storing informatino about the intersected object with Raycasthit
        RaycastHit hittedObject; // uzfiksuoja taska kur papaudei
        //variable for finding the barracks build button
       // var barrackBtn = FindObjectOfType<BarrackBuild>();
        // variable for finding the turret build button
       // var turretBtn = FindObjectOfType<TurretBuild>();

        //If statement for outputing the ray beam from the main level camera
        //Camera.main.ScreenPointToRay(Input.mousePosition) method for directing where the user clicked on the build area object plane
        //out hittedObject variable for storing information about the interected object with the ray
        //Mathf.Infinity - for making the ray size
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hittedObject, Mathf.Infinity)) //out nukreipimas /mathf ilgis ray; // jeigu colidinssi su tasku tai reiksme grazina true
        {       
            // if statement is for checking with what type of object did the ray collided with. It simply checks what is the object's layer.
            //If the game object's layer is "Build area" and if the user clicked on the build barracks button(which helps to indicate using buildBarrack()),
            // then the barracks building process can start
            //else: the building process doesn't start and a error message apears...

          	if (( hittedObject.transform.gameObject.layer == LayerMask.NameToLayer("BuildArea")))
          	{ 
              	if((barrackBtn.buildBarrack()) && (canBuild) && (playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(1)))
              	{
					animatedPopUps.createDecreaseEnergonPopUp(barrackBtn.getMinNeededEnergonAmountForTroopsBarrack()); // creating pop up window
            		animatedPopUps.createDecreaseCreditsPopUp(barrackBtn.getMinNeededCreditsAmountForTroopsBarrack()); // creating pop up window
                    playerbase.setCreditsAmount(playerbase.getCreditsAmount()-barrackBtn.getMinNeededCreditsAmountForTroopsBarrack()); // uzsetiname naujas reksmes 
                    playerbase.setEnergonAmount(playerbase.getEnergonAmount()-barrackBtn.getMinNeededEnergonAmountForTroopsBarrack()); // uzsetiname naujas reksmes
                    playerbase.setworkersAmount(playerbase.getworkersAmount()-1); // atimame is skailiuko vieneta
					playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBarrackAmountInLevel = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBarrackAmountInLevel + 1; // change amount of actual barracks on the game map 
                    playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(2); // fasle
					barrackBtn.canBuildAgain(true);
                    playerbase.setBuildingArea(false);

                    var playerWorkers = FindObjectsOfType<Worker>(); 

                    for (int i = 0; i < playerWorkers.Length; i++)
                    {
                      if (!playerWorkers[i].isWorkerAssigned()) // jeigu workeris yra laisvas tai jo reiksme yra false, kitu atveju bus true
                      {
                        //Variable for knowing, what's the current position of this object, which has this class
                        Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
                        playerWorkers[i].SetDestination(buildpozition);
                        playerWorkers[i].SetBuildingOrder(buildingTypes[0]); // nusiunciama buildingo tipa i workerio klase;
                        playerWorkers[i].setWorkerState(true);
                        return;
                      }    
                    }
              	}
				if((turretBtn.buildTurret()) && (canBuild) && (playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(6)))
				{
					animatedPopUps.createDecreaseEnergonPopUp(turretBtn.getMinNeededEnergonAmountForTurret()); // creating pop up window
            		animatedPopUps.createDecreaseCreditsPopUp(turretBtn.getMinNeededCreditsAmountForTurret()); // creating pop up window
					playerbase.setCreditsAmount(playerbase.getCreditsAmount()-turretBtn.getMinNeededCreditsAmountForTurret()); // uzsetiname naujas reksmes 
					playerbase.setEnergonAmount(playerbase.getEnergonAmount()-turretBtn.getMinNeededEnergonAmountForTurret()); // uzsetiname naujas reksmes
					playerbase.setworkersAmount(playerbase.getworkersAmount()-1); // atimame is skailiuko vieneta
					playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTurretAmountInLevel = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTurretAmountInLevel + 1;
					playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(6);
					turretBtn.canBuildAgain(true);
					playerbase.setBuildingArea(false);
					//  buildDefensive.SetActive(false);

					var playerWorkers = FindObjectsOfType<Worker>(); 

					for (int i = 0; i < playerWorkers.Length; i++)
					{
						if (!playerWorkers[i].isWorkerAssigned()) // jeigu workeris yra laisvas tai jo reiksme yra false, kitu atveju bus true
						{
						//Variable for knowing, what's the current position of this object, which has this class
						Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
						playerWorkers[i].SetDestination(buildpozition);
						playerWorkers[i].SetBuildingOrder(buildingTypes[1]); // nusiunciama buildingo tipa i workerio klase;
						playerWorkers[i].setWorkerState(true);
						return;
						}    
					}
				}
				if((researchCenterBtn.buildResearchCentre()) && (canBuild) && (playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(3)))
				{
					animatedPopUps.createDecreaseEnergonPopUp(researchCenterBtn.getMinNeededEnergonAmountForResearchCentre()); // creating pop up window
            		animatedPopUps.createDecreaseCreditsPopUp(researchCenterBtn.getMinNeededCreditsAmountForResearchCentre()); // creating pop up window
					playerbase.setCreditsAmount(playerbase.getCreditsAmount()-researchCenterBtn.getMinNeededCreditsAmountForResearchCentre()); // uzsetiname naujas reksmes 
					playerbase.setEnergonAmount(playerbase.getEnergonAmount()-researchCenterBtn.getMinNeededEnergonAmountForResearchCentre()); // uzsetiname naujas reksmes
					playerbase.setworkersAmount(playerbase.getworkersAmount()-1); // atimame is skailiuko vieneta
					playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBuildingResearchAmountInLevel = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBuildingResearchAmountInLevel + 1;
					playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(3);
					researchCenterBtn.canBuildAgain(true);
					playerbase.setBuildingArea(false);
				
					var playerWorkers = FindObjectsOfType<Worker>(); 

					for (int i = 0; i < playerWorkers.Length; i++)
					{
						if (!playerWorkers[i].isWorkerAssigned()) // jeigu workeris yra laisvas tai jo reiksme yra false, kitu atveju bus true
						{
						//Variable for knowing, what's the current position of this object, which has this class
						Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
						playerWorkers[i].SetDestination(buildpozition);
						playerWorkers[i].SetBuildingOrder(buildingTypes[2]); // nusiunciama buildingo tipa i workerio klase;
						playerWorkers[i].setWorkerState(true);
						return;
						}    
					}
				}
				if((troopsCenterBtn.buildResearchCentre()) && (canBuild) && (playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(4)))
				{
					animatedPopUps.createDecreaseEnergonPopUp(troopsCenterBtn.getMinNeededEnergonAmountForTroopsResearchCentre()); // creating pop up window
            		animatedPopUps.createDecreaseCreditsPopUp(troopsCenterBtn.getMinNeededCreditsAmountForTroopsResearchCentre()); // creating pop up window
					playerbase.setCreditsAmount(playerbase.getCreditsAmount()-troopsCenterBtn.getMinNeededCreditsAmountForTroopsResearchCentre()); // uzsetiname naujas reksmes 
					playerbase.setEnergonAmount(playerbase.getEnergonAmount()-troopsCenterBtn.getMinNeededEnergonAmountForTroopsResearchCentre()); // uzsetiname naujas reksmes
					playerbase.setworkersAmount(playerbase.getworkersAmount()-1); // atimame is skailiuko vieneta
					playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTroopsResearchAmountInLevel = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTroopsResearchAmountInLevel + 1;
					playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(4);
					troopsCenterBtn.canBuildAgain(true);
					playerbase.setBuildingArea(false);
				
					var playerWorkers = FindObjectsOfType<Worker>(); 

					for (int i = 0; i < playerWorkers.Length; i++)
					{
						if (!playerWorkers[i].isWorkerAssigned()) // jeigu workeris yra laisvas tai jo reiksme yra false, kitu atveju bus true
						{
						//Variable for knowing, what's the current position of this object, which has this class
						Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
						playerWorkers[i].SetDestination(buildpozition);
						playerWorkers[i].SetBuildingOrder(buildingTypes[3]); // nusiunciama buildingo tipa i workerio klase;
						playerWorkers[i].setWorkerState(true);
						return;
						}    
					}
				}
				if(buildWorkerBtn.buildWorker() && (playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(9)))
				{
					playerbase.setBuildingArea(false);
					// buildRegWorker.SetActive(false);
					Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
					playerbase.setPosition(buildpozition);
					animatedPopUps.createDecreaseEnergonPopUp(buildWorkerBtn.getMinNeededEnergonAmount());
					playerbase.setCreditsAmount(playerbase.getEnergonAmount()-buildWorkerBtn.getMinNeededEnergonAmount()); // uzsetiname naujas reksmes 
					timerStart.startTimer(buildWorkerStartTime);
					buildWorkerBtn.canBuildAgain(true);
				}
				/*if((buildPlayerWallBtn.buildPlayerWall()) && (canBuild))
				{
					playerbase.setCreditsAmount(playerbase.getCreditsAmount()-buildPlayerWallBtn.getMinNeededCreditsAmountForDefensiveWall()); // uzsetiname naujas reksmes 
					playerbase.setEnergonAmount(playerbase.getEnergonAmount()-buildPlayerWallBtn.getMinNeededEnergonAmountForDefensiveWall()); // uzsetiname naujas reksmes
					playerbase.setworkersAmount(playerbase.getworkersAmount()-1); // atimame is skailiuko vieneta
					buildPlayerWallBtn.canBuildAgain(true);
					playerbase.setBuildingArea(false);
					// buildDefensive.SetActive(false);
					var playerWorkers = FindObjectsOfType<Worker>(); 

					for (int i = 0; i < playerWorkers.Length; i++)
					{
						if (!playerWorkers[i].isWorkerAssigned()) // jeigu workeris yra laisvas tai jo reiksme yra false, kitu atveju bus true
						{
						//Variable for knowing, what's the current position of this object, which has this class
						Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
						playerWorkers[i].SetDestination(buildpozition);
						playerWorkers[i].SetBuildingOrder(buildingTypes[4]); // nusiunciama buildingo tipa i workerio klase;
						playerWorkers[i].setWorkerState(true); // workeris yra uzimtas
						return;
						}    
					}
				}*/
				if((creditsMiningStationBtn.buildMiningStation()) && (canBuild) && (playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(8))){
					animatedPopUps.createDecreaseEnergonPopUp(creditsMiningStationBtn.getminNeededEnergonAmountForMiningStation()); // creating pop up window
            		animatedPopUps.createDecreaseCreditsPopUp(creditsMiningStationBtn.getminNeededCreditsAmountForMiningStation()); // creating pop up window
					playerbase.setCreditsAmount(playerbase.getCreditsAmount()-creditsMiningStationBtn.getminNeededCreditsAmountForMiningStation()); // uzsetiname naujas reksmes 
					playerbase.setEnergonAmount(playerbase.getEnergonAmount()-creditsMiningStationBtn.getminNeededEnergonAmountForMiningStation()); // uzsetiname naujas reksmes
					playerbase.setworkersAmount(playerbase.getworkersAmount()-1); // atimame is skailiuko vieneta
					playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerCreditsMiningStationAmountInLevel = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerCreditsMiningStationAmountInLevel + 1;
					playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(8);
					creditsMiningStationBtn.canBuildAgain(true);
					playerbase.setBuildingArea(false);
					// buildDefensive.SetActive(false);
					var playerWorkers = FindObjectsOfType<Worker>(); 

					for (int i = 0; i < playerWorkers.Length; i++)
					{
						if (!playerWorkers[i].isWorkerAssigned()) // jeigu workeris yra laisvas tai jo reiksme yra false, kitu atveju bus true
						{
						//Variable for knowing, what's the current position of this object, which has this class
						Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
						playerWorkers[i].SetDestination(buildpozition);
						playerWorkers[i].SetBuildingOrder(buildingTypes[5]); // nusiunciama buildingo tipa i workerio klase;
						playerWorkers[i].setWorkerState(true); // workeris yra uzimtas
						return;
						}    
					}
				}
				if((armyCampBuildBtn.armyCampBuildState()) && (canBuild) && (playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(2))){
					animatedPopUps.createDecreaseEnergonPopUp(armyCampBuildBtn.getMinNeededEnergonAmountForArmyCamp()); // creating pop up window
            		animatedPopUps.createDecreaseCreditsPopUp(armyCampBuildBtn.getMinNeededCreditsAmountForArmyCamp()); // creating pop up window
					playerbase.setCreditsAmount(playerbase.getCreditsAmount()-armyCampBuildBtn.getMinNeededCreditsAmountForArmyCamp()); // uzsetiname naujas reksmes 
					playerbase.setEnergonAmount(playerbase.getEnergonAmount()-armyCampBuildBtn.getMinNeededEnergonAmountForArmyCamp()); // uzsetiname naujas reksmes
					playerbase.setworkersAmount(playerbase.getworkersAmount()-1); // atimame is skailiuko vieneta
					playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerArmyCampAmountInLevel = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerArmyCampAmountInLevel + 1;
					playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(2);
					armyCampBuildBtn.canBuildAgain(true);
					playerbase.setBuildingArea(false);
					// buildDefensive.SetActive(false);
					var playerWorkers = FindObjectsOfType<Worker>(); 

					for (int i = 0; i < playerWorkers.Length; i++)
					{
						if (!playerWorkers[i].isWorkerAssigned()) // jeigu workeris yra laisvas tai jo reiksme yra false, kitu atveju bus true
						{
						//Variable for knowing, what's the current position of this object, which has this class
						Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
						playerWorkers[i].SetDestination(buildpozition);
						playerWorkers[i].SetBuildingOrder(buildingTypes[6]); // nusiunciama buildingo tipa i workerio klase;
						playerWorkers[i].setWorkerState(true); // workeris yra uzimtas
						return;
						}    
					}
				}
				if((shrineBuildBtn.shrineBuildState()) && (canBuild) && (playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(5))){
					animatedPopUps.createDecreaseEnergonPopUp(shrineBuildBtn.getMinNeededEnergonAmountForShrine()); // creating pop up window
            		animatedPopUps.createDecreaseCreditsPopUp(shrineBuildBtn.getMinNeededCreditsAmountForShrine()); // creating pop up window
					playerbase.setCreditsAmount(playerbase.getCreditsAmount()-shrineBuildBtn.getMinNeededCreditsAmountForShrine()); // uzsetiname naujas reksmes 
					playerbase.setEnergonAmount(playerbase.getEnergonAmount()-shrineBuildBtn.getMinNeededEnergonAmountForShrine()); // uzsetiname naujas reksmes
					playerbase.setworkersAmount(playerbase.getworkersAmount()-1); // atimame is skailiuko vieneta
					playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerShrineAmountInLevel = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerShrineAmountInLevel + 1;
					playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(5);
					shrineBuildBtn.canBuildAgain(true);
					playerbase.setBuildingArea(false);
					// buildDefensive.SetActive(false);
					var playerWorkers = FindObjectsOfType<Worker>(); 

					for (int i = 0; i < playerWorkers.Length; i++)
					{
						if (!playerWorkers[i].isWorkerAssigned()) // jeigu workeris yra laisvas tai jo reiksme yra false, kitu atveju bus true
						{
						//Variable for knowing, what's the current position of this object, which has this class
						Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
						playerWorkers[i].SetDestination(buildpozition);
						playerWorkers[i].SetBuildingOrder(buildingTypes[7]); // nusiunciama buildingo tipa i workerio klase;
						playerWorkers[i].setWorkerState(true); // workeris yra uzimtas
						return;
						}    
					}
				}
				if ((FindObjectsOfType<changePosition>() != null) && (canBuild)){
					var changePosBuildings = FindObjectsOfType<changePosition>();
					var index = 0; // btn pressed index value
					for (int i = 0; i < changePosBuildings.Length; i++){
						if(changePosBuildings[i].canChange){ // check again if button is already pressed
							index = changePosBuildings[i].returnBtnIndex;
							changePosBuildings[i].isInChangeMode = true;
							//Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
							//changePosBuildings[i].changePositionOfBuilding(buildpozition);
						}
					}
					if (index != 0){ // index as additional check system
						
						var playerWorkers = FindObjectsOfType<Worker>(); // find all the workers on the map
						for (int y = 0; y < playerWorkers.Length; y++)
					   	{
							if(!playerWorkers[y].isWorkerAssigned()){ // find first free worker on the map
								Vector3 buildpozition = new Vector3(hittedObject.point.x, hittedObject.point.y, hittedObject.point.z);
								playerWorkers[y].changePressedBtnIndex = index;
								playerWorkers[y].SetDestination(buildpozition);
								playerWorkers[y].SetBuildingOrder(buildingTypes[index-1]); // nusiunciama buildingo tipa i workerio klase;
								playerWorkers[y].setWorkerState(true); // workeris yra uzimtas
							}
						}
						playerbase.setworkersAmount(playerbase.getworkersAmount()-1); // atimame is skailiuko vieneta
						playerbase.setBuildingArea(false);
						for (int i = 0; i < changePosBuildings.Length; i++){
							if((changePosBuildings[i].returnBtnIndex == index) && (changePosBuildings[i].isInChangeMode)){ // check again if button is already pressed
								changePosBuildings[i].deactivateButton();	
							}
						}
					}
					else {
						for (int i = 0; i < changePosBuildings.Length; i++){
							if(changePosBuildings[i].canChange){ // check again if button is already pressed
								changePosBuildings[i].isInChangeMode = false;
								changePosBuildings[i].activateButton(); // activate and set back button text
							}
						}
						playerbase.setBuildingArea(false);
						return;
					}
				}
            } 
      	} 

    }
}
