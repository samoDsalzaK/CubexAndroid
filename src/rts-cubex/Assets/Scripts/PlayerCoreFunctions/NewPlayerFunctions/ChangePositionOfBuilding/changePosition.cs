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
    bool isPressed = false;

    public bool canChange {get{return isPressed;}set{isPressed = value;}}

    public int returnBtnIndex {get{return btnIndex;}}

    private Base playerbase;
   
    void Start (){
        //
    }


    void Update (){
        //
    }
    
    
    public void changePosAction(){
        // find playerbase object on the game map
        playerbase = FindObjectOfType<Base>();
        // checks for workers on the map
        if (playerbase.getworkersAmount() <= 0){
            //Debug.Log("Build worker first"); 
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
            errorWorkerIssue2.SetActive(true);
        }
        // checks if there are other buildings on the map in position change mode
        var changePosBuildings = FindObjectsOfType<changePosition>();
        if(changePosBuildings != null){
            for (int i = 0; i < changePosBuildings.Length; i++){
                if(changePosBuildings[i].canChange){
                    Debug.Log("Can not change, because other building is in change position mode");
                    errorForChangingPos.SetActive(true);
                    return;
                }
            }
        }
        isPressed = true;
        playerbase.setBuildingArea(true);
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
        changePos.interactable = false;
        changePosBtnText.text = "Select place";
    }
    

    public void destroyGameObject(){
        Destroy(gameObject);
    }

    /*public void changePositionOfBuilding(Vector3 pos){
        building.transform.position = pos;
        playerbase.setBuildingArea(false);
        changePos.interactable = true;
        changePosBtnText.text = "Change position";
        isPressed = false;
    }*/

    public void activateButton(){
        changePos.interactable = true;
        changePosBtnText.text = "Change position";
    }
}
