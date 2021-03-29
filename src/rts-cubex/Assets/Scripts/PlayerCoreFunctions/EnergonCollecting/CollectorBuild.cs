using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CollectorBuild : MonoBehaviour
{
   //This script is attached on the build collector button 
    //************************UPDATE****************************************** */
    [Header("Main collector button configuration parameters ")]

    [SerializeField] GameObject errorForWorker;
    [SerializeField] GameObject errorForWorker2;
    //State variable for checking when the user clicked on the button
    [SerializeField] bool btnCollectorClicked = false;
     //State variable for checking if the user's chosen collector object is constructed
    [SerializeField] bool collectorStructureBuilt = false;
    //Variable for knowing which button could be disabled
    [SerializeField] Button buildButton;
    //Variable for checking the button's text
    [SerializeField] Text buttonText;
    //Variable for remembering the button's text
    private Base playerbase;
    [SerializeField] int minNeededEnergonAmount;
    [SerializeField] int minNeededCreditsAmount;
    [SerializeField] float neededWorkerJobAmountForStrucuture = 0.66f;
    //remembering the button's text
    private void Start() {
        
         if(FindObjectOfType<Base>() == null)
         {
           return;
         }
         else{
           playerbase = FindObjectOfType<Base>();
         }
         
         buttonText.text = "Create Energon Station\n" + "(" + minNeededEnergonAmount + " energon & " +  minNeededCreditsAmount + " credits)";
    }
    private void Update() {
        //Checks if the collector structure is built, and if it is built, then this button is unlocked
        if (collectorStructureBuilt) 
        {
            //Sets the button click variable to false
           btnCollectorClicked = false;
           buildButton.interactable = true; 
           //Displays the previous button's text
           buttonText.text = "Create Energon Station\n" + "(" + minNeededEnergonAmount + " energon & " +  minNeededCreditsAmount + " credits)";
           //sets this variable to false because the collector structure is already constructed
           collectorStructureBuilt = false;
           return;
        }
    }
    //When the user clicked on the button, the player need's to select the deposit, which has the class BuildMiningStation
    //This method used on the button's OnClick() section
    public void clickedOnBuildCollectorBtn()
    {       
		// checks for workers on the map
        if (playerbase.getworkersAmount() <= 0){
            Debug.Log("Build worker first"); 
		   	errorForWorker.SetActive(true);  
            return;
        }
        // check if there are free workers on the map with at least 2/3 of life cycle bar
        var Workers = FindObjectsOfType<Worker>(); // find all the workers on the map
        var count = 0;
		for (int y = 0; y < Workers.Length; y++)
			{
				if(!Workers[y].isWorkerAssigned() && Math.Round(Workers[y].GetComponent<WorkerLifeCycle>().getWorkerJobLeft(),2) >= Math.Round(neededWorkerJobAmountForStrucuture,2)){ 
               	count ++;
            }
         }
        if (count == 0){
            errorForWorker2.SetActive(true);
        }
        // check for available resources
        if (playerbase.getEnergonAmount() < minNeededEnergonAmount || playerbase.getCreditsAmount() < minNeededCreditsAmount) // patikrina esamus zaidejo resursus
        {
            playerbase.setResourceAMountScreenState(true);
            return;
        }
        var playerWorkers = GameObject.FindGameObjectsWithTag("Worker");
        for(int i = 0; i < playerWorkers.Length; i++)
        {
           // Debug.Log(Math.Round(playerWorkers[i].GetComponent<WorkerLifeCycle>().getWorkerJobLeft(),2));
            if(!playerWorkers[i].GetComponent<Worker>().isWorkerAssigned() &&  Math.Round(playerWorkers[i].GetComponent<WorkerLifeCycle>().getWorkerJobLeft(),2) >= Math.Round(neededWorkerJobAmountForStrucuture,2))
            {
              //Debug.Log(playerWorkers[i].GetComponent<WorkerLifeCycle>().getWorkerJobLeft());
              btnCollectorClicked = true;
              buildButton.interactable = false;
              buttonText.text = "Select a deposit";  
              return;      
            }
        }
    }   
    //Method for getting the state of the button, is it clicked - true or not - false
    public bool getClickedBuildCollectorBtnState()
    {
        return btnCollectorClicked;
    }
    public void setClickedBuildCollectorBtnState(bool newState)
    {
        btnCollectorClicked = newState;
    }
    //Method for sending a message to the collector build button that the collector structure is constructed..
    public void setCollectorStructureBuilt(bool state/*, string name*/)
    {
        collectorStructureBuilt = state;
        //originalBtnText = name;
    }
    public int getMinNeededEnergonAmountForEnergonCollector()
    {
      return minNeededEnergonAmount;
    } 
    public int getMinNeededCreditsAmountForCreditsCollector()
    {
      return minNeededCreditsAmount;
    } 
    public float getNeededWorkerJobAmountForStrucuture()
    {
      return neededWorkerJobAmountForStrucuture;
    }
}
