using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buildArmyCamp : MonoBehaviour
{
    [Header("Main configuration parameters")]

    [SerializeField] GameObject errorForWorker;
    [SerializeField] GameObject errorForWorker2;

    //boolean variable for indicating when the user can build a barracks structure
    [SerializeField] bool canBuildArmyCamp = false;
     //boolean variable for indicating if the barracks structure is built
    [SerializeField] bool structureBuilt = false;
    //Button variable, which will used for disablying when the user clicked on the barracks construction button
    [SerializeField] Button buildArmyCampBtn;
    [SerializeField] Text buttonText;
    private Base playerbase;
    [SerializeField] int minNeededEnergonAmount;
    [SerializeField] int minNeededCreditsAmount;
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
       buttonText.text = "Create Army Camp\n" + "(" + minNeededEnergonAmount + " energon & " +  minNeededCreditsAmount + " credits)";
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the barracks structure is built in the base
        if (structureBuilt)
        {
            buildArmyCampBtn.interactable = true;
            buttonText.text = "Create Army Camp\n" + "(" + minNeededEnergonAmount + " energon & " +  minNeededCreditsAmount + " credits)";
            canBuildArmyCamp = false;
            structureBuilt = false;
        }
    }

    public void buildArmyCampAction()
    {
        // checks for workers on the map
        if (playerbase.getworkersAmount() <= 0){
            Debug.Log("Build worker first"); 
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
            errorForWorker2.SetActive(true);
        }
        // check for available resources
        if (playerbase.getEnergonAmount() < minNeededEnergonAmount || playerbase.getCreditsAmount() < minNeededCreditsAmount) // patikrina esamus zaidejo resursus
        {
        playerbase.setResourceAMountScreenState(true);    
        return; 
        }
        playerbase.setBuildingArea(true);
        //State variable is setted to true, which means that the button is clicked
        canBuildArmyCamp = true;
        //Button interaction state is setted to false
        buildArmyCampBtn.interactable = false;
        buttonText.text = "Select Build Site";  
    }

    public bool armyCampBuildState()
    {
        return canBuildArmyCamp;
    }
    public void canBuildAgain(bool state)
    {
        structureBuilt = state;
    }
    public int getMinNeededEnergonAmountForArmyCamp()
    {
      return minNeededEnergonAmount;
    } 
    public int getMinNeededCreditsAmountForArmyCamp()
    {
      return minNeededCreditsAmount;
    } 
}
