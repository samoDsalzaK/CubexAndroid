using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrackBuild : MonoBehaviour
{
   //Button for building a barracks millitary structure...

    //NOTE: When you implement the barracks spawn system, then use this variable! :)
    //[SerializeField] GameObject barrack;
    [Header("Main configuration parameters")]

    [SerializeField] GameObject errorForWorker;
    [SerializeField] GameObject errorForWorker2;
    //boolean variable for indicating when the user can build a barracks structure
    [SerializeField] bool canBuildBarrack = false;
     //boolean variable for indicating if the barracks structure is built
    [SerializeField] bool structureBuilt = false;
    //Button variable, which will used for disablying when the user clicked on the barracks construction button
    [SerializeField] Button buildBarrackBtn;
    [SerializeField] Text buttonText;
    [SerializeField] Text availableBarracksText;
    private Base playerbase;
    [SerializeField] int minNeededEnergonAmount;
    [SerializeField] int minNeededCreditsAmount;
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
       buttonText.text = "Create Troops Barrack\n" + "(" + minNeededEnergonAmount + " energon & " +  minNeededCreditsAmount + " credits)";
       availableBarracksText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBarrackAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerBarrackAmountInLevel;
    }  
    private void Update() {
        // check for current build button state and apply text changes
        if (!playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(1)){
            structureBuilt = true;
        }
        //Checks if the barracks structure is built in the base
        if (structureBuilt)
        {
            if (playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBarrackAmountInLevel >= playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerBarrackAmountInLevel){
                buttonText.text = "Troops Barrack\n" + "Max amount reached";
                availableBarracksText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBarrackAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerBarrackAmountInLevel;
                buildBarrackBtn.interactable = false;
                canBuildBarrack = false;
                structureBuilt = false;
            }
            else{
                buildBarrackBtn.interactable = true;
                availableBarracksText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBarrackAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerBarrackAmountInLevel;
                buttonText.text = "Create Troops Barrack\n" + "(" + minNeededEnergonAmount + " energon & " +  minNeededCreditsAmount + " credits)";
                canBuildBarrack = false;
                structureBuilt = false;
            }
        }
    }
    //When you've clicked on the button, this method will be invoked in the Unity ClickOn() section
    public void buildBarrackAction()
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
        if (playerbase.getEnergonAmount() < minNeededEnergonAmount || playerbase.getCreditsAmount() < minNeededCreditsAmount) // patikrina esamus zaidejo resursus
        {
        playerbase.GetComponent<LocalPanelManager>().deactivatePanels();
        playerbase.setResourceAMountScreenState(true);    
        return; 
        }
        // change button activity
        playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(1);
        if(playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(1)){
            playerbase.setBuildingArea(true);
            //State variable is setted to true, which means that the button is clicked
            canBuildBarrack = true;
            //buildArmyCampBtn.interactable = false;
            buttonText.text = "Select Place";  
        }
        else{
            //State variable is setted to true, which means that the button is clicked
            canBuildBarrack = false;
            structureBuilt = true; 
            playerbase.setBuildingArea(false);
        }
    }

    public bool buildBarrack()
    {
        return canBuildBarrack;
    }
    public void canBuildAgain(bool state)
    {
        structureBuilt = state;
    }
    public int getMinNeededEnergonAmountForTroopsBarrack()
    {
      return minNeededEnergonAmount;
    } 
    public int getMinNeededCreditsAmountForTroopsBarrack()
    {
      return minNeededCreditsAmount;
    } 
   
}
