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
       	buttonText.text = "Build Crypto Energon\n mining station\n" + "(" + minNeededEnergonAmountForMiningStation + " energon & " + minNeededCreditsAmountForMiningStation  + " credits)";
	    creditsLeft.text = "Credits left : " + playerbase.getCreditsAmount();
        energonLeft.text = "Energon left : " + playerbase.getEnergonAmount();
    }  
    private void Update() {

        //Checks if structure is built in the base
        if (structureBuilt)
        {
             buildMiningStationBtn.interactable = true;
             buttonText.text = "Build Crypto Energon\n mining station\n" + "(" + minNeededEnergonAmountForMiningStation + " energon & " + minNeededCreditsAmountForMiningStation  + " credits)";
             canBuildMiningStation = false;
             structureBuilt = false;
             creditsLeft.text = "Credits left : " + playerbase.getCreditsAmount();
             energonLeft.text = "Energon left : " + playerbase.getEnergonAmount();
        }
    }
    //When you've clicked on the button, this method will be invoked in the Unity ClickOn() section
    public void buildMiningStationAction()
    {
        // find playerbase object on the game map
        playerbase = FindObjectOfType<Base>();
        // checks for workers on the map
        if (playerbase.getworkersAmount() <= 0){
            //Debug.Log("Build worker first"); 
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
            errorWorkerIssue2.SetActive(true);
        }
        // check available resources
        if (playerbase.getEnergonAmount() < minNeededEnergonAmountForMiningStation || playerbase.getCreditsAmount() < minNeededCreditsAmountForMiningStation) // patikrina esamus zaidejo resursus
        {
            playerbase.setResourceAMountScreenState(true);    
            return; 
        }
        playerbase.setBuildingArea(true);
     //   clickUndo.SetActive(true);
        //State variable is setted to true, which means that the button is clicked
        canBuildMiningStation = true;
        //Button interaction state is setted to false
        buildMiningStationBtn.interactable = false;
        buttonText.text = "Select Place";  
        //Add button locking system...
        //Like showing the text which says place the barracks object in the base area
        //Debug.Log("Select a place where to build a barrack.");
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
