using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class changePosition : MonoBehaviour
{
    
    [Header("Change position configuration parameters")]
    [SerializeField] Button changePos;
    [SerializeField] Text changePosBtnText;
    [SerializeField] GameObject errorWorkerIssue1;
    [SerializeField] GameObject errorWorkerIssue2;
    [SerializeField] GameObject errorForChangingPos;
    [SerializeField] int btnIndex;
    [SerializeField] int changePosBuildingIndex;
    bool isPressed = false;

    public bool canChange {get{return isPressed;}set{isPressed = value;}}

    public int returnBtnIndex {get{return btnIndex;}}

    public int returnChangePosBuildingIndex{get{return changePosBuildingIndex;}}

    private Base playerbase;

    private LocalPanelManager localPanelManager;

    private PanelManager panelManager; 

    private int clickCount = 0;

    [SerializeField] Material defaultMaterial;

    bool isInChangePosMode = false; // variable which will say if there is building in change pos mode

    public bool isInChangeMode {get {return isInChangePosMode;} set {isInChangePosMode = value;}}
   
    void Start (){
        if(FindObjectOfType<Base>() == null)
        {
           return;
        }
        else
        {
            playerbase = FindObjectOfType<Base>();
        }
        changePosBtnText.text = "Change Position";
    }


    void Update (){
        //
    }

    public void setDefaultValues(){
        //Debug.Log("Here");
        if (isInChangePosMode){
            return;
        }
        else{
            changePosBtnText.text = "Change Position";
            //playerbase.setBuildingArea(false);
            clickCount = 0;
            isPressed = false;
            //Debug.Log("Here");
            if(gameObject.GetComponent<Renderer>() != null){
                //Debug.Log("Here");
                gameObject.GetComponent<Renderer>().material = defaultMaterial;
            }
            else{
                //Debug.Log("Here");
                Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
                foreach (Transform t in ts) {
                    if(t.gameObject.GetComponent<Renderer>() != null)
                    {
                        t.gameObject.GetComponent<Renderer>().material = defaultMaterial; // setting default building colour
                    }
                } 
            }
        }   
    }
    
    
    public void changePosAction(){
        // find playerbase object on the game map
        playerbase = FindObjectOfType<Base>();
        if(GetComponent<LocalPanelManager>() != null){
            localPanelManager = GetComponent<LocalPanelManager>();
        }
        panelManager = GetComponent<PanelManager>();
        // checks for workers on the map
        if (playerbase.getworkersAmount() <= 0){
            //Debug.Log("Build worker first"); 
            if(localPanelManager != null){
                localPanelManager.deactivatePanels();
            }
            panelManager.onExitDeactivatePlayerBasePanels();
		    errorWorkerIssue1.SetActive(true);
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
            if(localPanelManager != null){
                localPanelManager.deactivatePanels();
            }
            panelManager.onExitDeactivatePlayerBasePanels();
            errorWorkerIssue2.SetActive(true);
        }
        // checks if there are other buildings on the map in position change mode
        var changePosBuildings = FindObjectsOfType<changePosition>();
        if(changePosBuildings != null){
            for (int i = 0; i < changePosBuildings.Length; i++){
                if(changePosBuildings[i].isInChangeMode){
                    Debug.Log("Can not change, because other building is in change position mode");
                    if(localPanelManager != null){
                        localPanelManager.deactivatePanels();
                    }
                    panelManager.onExitDeactivatePlayerBasePanels();
                    errorForChangingPos.SetActive(true);
                    return;
                }
            }
        }
        
        Color changeColour = new Color32(255,0, 13, 100);
        if(gameObject.GetComponent<Renderer>() != null){
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
        }
        else {
            Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Renderer>() != null)
                {
                    t.gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
                }
            }
        }
        // change button activity
        playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(changePosBuildingIndex);
        if(playerbase.GetComponent<unselectBuildGameStructure>().checkForCurrentButtonState(changePosBuildingIndex)){
            playerbase.setBuildingArea(true);
            isPressed = true;
            changePosBtnText.text = "Select place";
            clickCount = 1;
        }
        else{
            isPressed = false;
            changePosBtnText.text = "Change Position";
            playerbase.setBuildingArea(false);
            clickCount = 0;
            if(gameObject.GetComponent<Renderer>() != null){
                gameObject.GetComponent<Renderer>().material = defaultMaterial;
            }
            else{
               Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
                foreach (Transform t in ts) {
                    if(t.gameObject.GetComponent<Renderer>() != null)
                    {
                        t.gameObject.GetComponent<Renderer>().material = defaultMaterial; // setting default building colour
                    }
                } 
            }
        }
    }
    

    public void destroyGameObject(){
        playerbase.GetComponent<unselectBuildGameStructure>().changeBuildStructureButtonActivity(changePosBuildingIndex);
        Destroy(gameObject);
    }

    public void activateButton(){
        changePos.interactable = true;
        changePosBtnText.text = "Change position";
    }

    public void deactivateButton(){
        changePos.interactable = false;
    }
}
