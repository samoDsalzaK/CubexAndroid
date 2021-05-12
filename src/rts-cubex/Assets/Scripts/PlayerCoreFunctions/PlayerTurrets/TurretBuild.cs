using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurretBuild : MonoBehaviour
{
     //Button for building a barracks millitary structure...

    //NOTE: When you implement the barracks spawn system, then use this variable! :)
    //[SerializeField] GameObject barrack;
    [Header("Main configuration parameters")]

    [SerializeField] GameObject errorForWorker;
    [SerializeField] GameObject errorForWorker2;
    //boolean variable for indicating when the user can build a barracks structure
    [SerializeField] bool canBuildTurret = false;
     //boolean variable for indicating if the barracks structure is built
    [SerializeField] bool structureBuilt = false;
    //Button variable, which will used for disablying when the user clicked on the barracks construction button
    [SerializeField] Button buildTurretBtn;
    [SerializeField] Text buttonText;
    [SerializeField] Text availableTurretsText;
    private Base playerbase;
    [SerializeField] int minNeededEnergonAmountForTurret;//5
    [SerializeField] int minNeededCreditsAmountForTurret;//50
   // [SerializeField] GameObject clickUndo;
    private void Start() 
    {
      if(FindObjectOfType<Base>() == null)
        {
           return;
        }
        else
        {
           playerbase = FindObjectOfType<Base>();
        }
    //  clickUndo.SetActive(false);
        buttonText.text = "Create Turret\n" + "(" + minNeededEnergonAmountForTurret + " energon & " + minNeededCreditsAmountForTurret  + " credits)";
		//Debug.Log(playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTurretAmountInLevel);
		availableTurretsText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTurretAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerTurretAmountInLevel;
    }  
    private void Update() {
		// check for current build button state and apply text changes
        if (!playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(6)){
            structureBuilt = true;
        }
        //Checks if structure is built in the base
        if (structureBuilt)
        {
			if (playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTurretAmountInLevel >= playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerTurretAmountInLevel){
				availableTurretsText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTurretAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerTurretAmountInLevel;
				buttonText.text = "Turret\n" + "Max amount reached";
				buildTurretBtn.interactable = false;
				canBuildTurret = false;
				structureBuilt = false;
			}
			else{
				buildTurretBtn.interactable = true;
				availableTurretsText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTurretAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerTurretAmountInLevel;
				buttonText.text = "Create Turret\n" + "(" + minNeededEnergonAmountForTurret + " energon & " + minNeededCreditsAmountForTurret  + " credits)";
				canBuildTurret = false;
				structureBuilt = false;
			}
        }
    }
    //When you've clicked on the button, this method will be invoked in the Unity ClickOn() section
    public void buildTurretAction()
    {
		// checks for workers on the map
		if (playerbase.getworkersAmount() <= 0){
			Debug.Log("Build worker first"); 
			playerbase.GetComponent<LocalPanelManager>().deactivatePanels();
			errorForWorker.SetActive(true);  
			return;
		}
		// check if there are free workers on the map
		var playerWorkers = FindObjectsOfType<Worker>(); // find all the workers on the map
		var count = 0;
		for (int y = 0; y < playerWorkers.Length; y++)
			{
				if(!playerWorkers[y].isWorkerAssigned()){ // find first free worker on the map
            	count ++;
            }
        }
      	if (count == 0){
         	playerbase.GetComponent<LocalPanelManager>().deactivatePanels();
         	errorForWorker2.SetActive(true);
      	}
      	// check for available resources
      	if (playerbase.getEnergonAmount() < minNeededEnergonAmountForTurret || playerbase.getCreditsAmount() < minNeededCreditsAmountForTurret) // patikrina esamus zaidejo resursus
        {
            playerbase.GetComponent<LocalPanelManager>().deactivatePanels();
            playerbase.setResourceAMountScreenState(true);    
            return; 
        }
      
	  	  // change button activity
        playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(6);
        if(playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(6)){
            playerbase.setBuildingArea(true);
            //State variable is setted to true, which means that the button is clicked
            canBuildTurret = true;
            //buildArmyCampBtn.interactable = false;
            buttonText.text = "Select Place";  
        }
        else{
            canBuildTurret = false;
            structureBuilt = true; 
            playerbase.setBuildingArea(false);
        }
    }

    public bool buildTurret()
    {
        return canBuildTurret;
    }
    public void canBuildAgain(bool state) 
    {
        structureBuilt = state;
    }
    public int getMinNeededEnergonAmountForTurret()
    {
      return minNeededEnergonAmountForTurret;
    } 
    public int getMinNeededCreditsAmountForTurret()
    {
      return minNeededCreditsAmountForTurret;
    } 
}
