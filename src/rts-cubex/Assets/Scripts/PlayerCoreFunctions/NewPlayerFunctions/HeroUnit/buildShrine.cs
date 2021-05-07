using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buildShrine : MonoBehaviour
{
    [Header("Main configuration parameters")]

    [SerializeField] GameObject errorForWorker;
    [SerializeField] GameObject errorForWorker2;
    //boolean variable for indicating when the user can build a barracks structure
    [SerializeField] bool canBuildShrine = false;
     //boolean variable for indicating if the barracks structure is built
    [SerializeField] bool structureBuilt = false;
    //Button variable, which will used for disablying when the user clicked on the barracks construction button
    [SerializeField] Button buildShrineBtn;
    [SerializeField] Text buttonText;
    [SerializeField] Text availableShrineText;
    private Base playerbase;
    [SerializeField] int minNeededEnergonAmount;
    [SerializeField] int minNeededCreditsAmount;

    [SerializeField] Text creditsLeft;
    [SerializeField] Text energonLeft;
    // Start is called before the first frame update
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
        buttonText.text = "Create Hero Shrine\n" + "(" + minNeededEnergonAmount + " energon & " +  minNeededCreditsAmount + " credits)";
        availableShrineText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerShrineAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerShrineAmountInLevel;
        creditsLeft.text = "Credits left : " + playerbase.getCreditsAmount();
        energonLeft.text = "Energon left : " + playerbase.getEnergonAmount();
    }

    // Update is called once per frame
    void Update()
    {
        if (structureBuilt)
        {
            if (playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerShrineAmountInLevel >= playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerShrineAmountInLevel){
                availableShrineText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerShrineAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerShrineAmountInLevel;
                buttonText.text = "Shrine\n" + "Max amount reached";
                buildShrineBtn.interactable = false;
                canBuildShrine = false;
                structureBuilt = false;  
            }
            else{
                buildShrineBtn.interactable = true;
                availableShrineText.text = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerShrineAmountInLevel + " / " + playerbase.GetComponent<setFidexAmountOfStructures>().getMaxPlayerShrineAmountInLevel;
                buttonText.text = "Create Hero Shrine\n" + "(" + minNeededEnergonAmount + " energon & " +  minNeededCreditsAmount + " credits)";
                canBuildShrine = false;
                structureBuilt = false;  
            }
            
        }
    }

     public void buildShrineAction()
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
        playerbase.setBuildingArea(true);
        //State variable is setted to true, which means that the button is clicked
        canBuildShrine = true;
        //Button interaction state is setted to false
        buildShrineBtn.interactable = false;
        buttonText.text = "Select Build Site";  
        //Add button locking system...
        //Like showing the text which says place the barracks object in the base area
        //Debug.Log("Select a place where to build a barrack.");
    }

    public bool shrineBuildState()
    {
        return canBuildShrine;
    }
    public void canBuildAgain(bool state)
    {
        structureBuilt = state;
    }
    public int getMinNeededEnergonAmountForShrine()
    {
      return minNeededEnergonAmount;
    } 
    public int getMinNeededCreditsAmountForShrine()
    {
      return minNeededCreditsAmount;
    } 
}
