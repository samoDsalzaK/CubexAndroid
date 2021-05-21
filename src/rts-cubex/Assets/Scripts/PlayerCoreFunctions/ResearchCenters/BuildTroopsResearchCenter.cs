using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildTroopsResearchCenter : MonoBehaviour
{
    //Button for building a barracks millitary structure...

    //NOTE: When you implement the barracks spawn system, then use this variable! :)
    //[SerializeField] GameObject barrack;
    [Header("Main configuration parameters")]

    [SerializeField] GameObject errorForWorker;
    [SerializeField] GameObject errorForWorker2;
    //boolean variable for indicating when the user can build a barracks structure
    [SerializeField] bool canBuildTroopsResearchCentre = false;
     //boolean variable for indicating if the barracks structure is built
    [SerializeField] bool structureBuilt = false;
    //Button variable, which will used for disablying when the user clicked on the barracks construction button
    [SerializeField] Button buildTroopsResearchCentreBtn;
    [SerializeField] Text buttonText;
    [SerializeField] Text troopsResearchAmountText;
    private Base playerbase;
    [SerializeField] int minNeededEnergonAmountForResearchCentre;
    [SerializeField] int minNeededCreditsAmountForResearchCentre;
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
        buttonText.text = "Build Troops Research Center (" + minNeededCreditsAmountForResearchCentre + " credits & " + minNeededEnergonAmountForResearchCentre  + " energon)\n";
        troopsResearchAmountText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTroopsResearchAmountInLevel  + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerTroopsResearchAmountInLevel;
    }
    private void Update() {
        //Checks if the barracks structure is built in the base
        // check for current build button state and apply text changes
        if (!playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(4)){
            structureBuilt = true;
        }
        if (structureBuilt)
        {
            if (playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTroopsResearchAmountInLevel >= playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerTroopsResearchAmountInLevel){
                troopsResearchAmountText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTroopsResearchAmountInLevel  + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerTroopsResearchAmountInLevel;
                buttonText.text = "Troops Research Center\n" + "Max amount reached";
                buildTroopsResearchCentreBtn.interactable = false;
                canBuildTroopsResearchCentre = false; 
                structureBuilt = false;
            }
            else{
                troopsResearchAmountText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTroopsResearchAmountInLevel  + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerTroopsResearchAmountInLevel;
                buttonText.text = "Build Troops Research Center (" + minNeededCreditsAmountForResearchCentre + " credits & " + minNeededEnergonAmountForResearchCentre  + " energon)\n";
                buildTroopsResearchCentreBtn.interactable = true;
                canBuildTroopsResearchCentre = false; 
                structureBuilt = false;
            }
           
        }
    }
    //When you've clicked on the button, this method will be invoked in the Unity ClickOn() section
    public void buildResearchCentreAction()
    {
        // checks for workers on the map
        if (playerbase.getworkersAmount() <= 0){
            Debug.Log("Build worker first"); 
            playerbase.GetComponent<LocalPanelManager>().deactivatePanels();
		   errorForWorker.SetActive(true);  
            return;
        }
        // check if there are free workers on the map with at least 2/3 of life cycle bar
        var Workers = FindObjectsOfType<Worker>(); // find all the workers on the map
        var count = 0;
		for (int y = 0; y < Workers.Length; y++)
			{
				if(!Workers[y].isWorkerAssigned()){ 
               	count ++;
            }
         }
        if (count == 0){
            playerbase.GetComponent<LocalPanelManager>().deactivatePanels();
            errorForWorker2.SetActive(true);
        }
        // check for available resources
        if (playerbase.getEnergonAmount() < minNeededEnergonAmountForResearchCentre || playerbase.getCreditsAmount() < minNeededCreditsAmountForResearchCentre) // patikrina esamus zaidejo resursus
        {
            playerbase.GetComponent<LocalPanelManager>().deactivatePanels();
            playerbase.setResourceAMountScreenState(true);    
            return; 
        }
        // change button activity
        playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(4);
        if(playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(4)){
            playerbase.setBuildingArea(true);
            //State variable is setted to true, which means that the button is clicked
            canBuildTroopsResearchCentre = true;
            //buildArmyCampBtn.interactable = false;
            buttonText.text = "Select Place";  
        }
        else{
            canBuildTroopsResearchCentre = false;
            structureBuilt = true; 
            playerbase.setBuildingArea(false);
        }
    }

    public bool buildResearchCentre()
    {
        return canBuildTroopsResearchCentre;
    }
    public void canBuildAgain(bool state) 
    {
        structureBuilt = state;
    }
    public int getMinNeededEnergonAmountForTroopsResearchCentre()
    {
      return minNeededEnergonAmountForResearchCentre;
    } 
    public int getMinNeededCreditsAmountForTroopsResearchCentre()
    {
      return minNeededCreditsAmountForResearchCentre;
    } 
}
