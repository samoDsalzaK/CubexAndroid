using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningStationBuild : MonoBehaviour
{
    //Button for building a barracks millitary structure...

    //NOTE: When you implement the barracks spawn system, then use this variable! :)
    //[SerializeField] GameObject barrack;
    [Header("Main configuration parameters")]
	
    [SerializeField] Text creditsLeft;
    [SerializeField] Text energonLeft;
    //Button variable, which will used for disablying when the user clicked on the barracks construction button
    [SerializeField] Button buildMiningStationBtn;
    [SerializeField] Text buttonText;
    [SerializeField] Text availableCreditsMiningStationText;
	[SerializeField] GameObject errorForWorker; // panel will pop up if there are no active workers on the map
    [SerializeField] GameObject errorWorkerIssue2;
    //boolean variable for indicating when the user can build a barracks structure
    [SerializeField] bool canBuildMiningStation = false;
     //boolean variable for indicating if the barracks structure is built
    [SerializeField] bool structureBuilt = false;
    private Base playerbase;
    [SerializeField] int minNeededEnergonAmountForMiningStation;//50
    [SerializeField] int minNeededCreditsAmountForMiningStation;//100
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
    	//    clickUndo.SetActive(false);
       	buttonText.text = "Build Credits\n mining station\n" + "(" + minNeededEnergonAmountForMiningStation + " energon & " + minNeededCreditsAmountForMiningStation  + " credits)";
        availableCreditsMiningStationText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerCreditsMiningStationAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerCreditsMiningStationAmountInLevel;
	    creditsLeft.text = "Credits left : " + playerbase.getCreditsAmount();
        energonLeft.text = "Energon left : " + playerbase.getEnergonAmount();
    }  
    private void Update() {
        // check for current build button state and apply text changes
        if (!playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(8)){
            structureBuilt = true;
        }
        //Checks if structure is built in the base
        if (structureBuilt)
        {
            if (playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerCreditsMiningStationAmountInLevel >= playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerCreditsMiningStationAmountInLevel){
                buttonText.text = "Credits Mining Station\n" + "Max amount reached";
                availableCreditsMiningStationText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerCreditsMiningStationAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerCreditsMiningStationAmountInLevel;
                buildMiningStationBtn.interactable = false;
                canBuildMiningStation = false;
                structureBuilt = false;
            }   
            else{
                buildMiningStationBtn.interactable = true;
                buttonText.text = "Build Credits\n mining station\n" + "(" + minNeededEnergonAmountForMiningStation + " energon & " + minNeededCreditsAmountForMiningStation  + " credits)";
                availableCreditsMiningStationText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerCreditsMiningStationAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerCreditsMiningStationAmountInLevel;
                canBuildMiningStation = false;
                structureBuilt = false;
            }
        }
        creditsLeft.text = "Credits left : " + playerbase.getCreditsAmount();
        energonLeft.text = "Energon left : " + playerbase.getEnergonAmount(); 
    }
    //When you've clicked on the button, this method will be invoked in the Unity ClickOn() section
    public void buildMiningStationAction()
    {
        // find playerbase object on the game map
        playerbase = FindObjectOfType<Base>();
        // checks for workers on the map
        if (playerbase.getworkersAmount() <= 0){
            //Debug.Log("Build worker first"); 
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
            errorWorkerIssue2.SetActive(true);
        }
        // check available resources
        if (playerbase.getEnergonAmount() < minNeededEnergonAmountForMiningStation || playerbase.getCreditsAmount() < minNeededCreditsAmountForMiningStation) // patikrina esamus zaidejo resursus
        {
            playerbase.GetComponent<LocalPanelManager>().deactivatePanels();
            playerbase.setResourceAMountScreenState(true);    
            return; 
        }
        // change button activity
        playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(8);
        if(playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(8)){
            playerbase.setBuildingArea(true);
            //State variable is setted to true, which means that the button is clicked
            canBuildMiningStation = true;
            //buildArmyCampBtn.interactable = false;
            buttonText.text = "Select Place";  
        }
        else{
            canBuildMiningStation = false;
            structureBuilt = true; 
            playerbase.setBuildingArea(false);
        }
    }

    public bool buildMiningStation()
    {
        return canBuildMiningStation;
    }
    public void canBuildAgain(bool state) 
    {
        structureBuilt = state;
    }
    public int getminNeededEnergonAmountForMiningStation()
    {
      return minNeededEnergonAmountForMiningStation;
    } 
    public int getminNeededCreditsAmountForMiningStation()
    {
      return minNeededCreditsAmountForMiningStation;
    } 
}
