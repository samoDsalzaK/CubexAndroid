using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unselectBuildGameStructure : MonoBehaviour
{
    // button ids
    // 1 - barrack
    // 2 - army camp
    // 3 - research b
    // 4 - research t
    // 5 - shrine
    // 6 - turret
    // 7 - energon collector
    // 8 - credits mining station
    // 9 - player worker
    Hashtable buildStrucutureButtonActivity  = new Hashtable();
    [SerializeField] int amountOfAvailableGameStructures;
    Base playerbase;
    
    // Start is called before the first frame update
    void Start()
    {
        playerbase = GetComponent<Base>();
        for(int i = 1; i <= amountOfAvailableGameStructures; i++){
            buildStrucutureButtonActivity.Add(i, false); // this means that all buttons are currently inactive
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeBuildStructureButtonActivity(int buttonID){
        if(buildStrucutureButtonActivity.ContainsKey(buttonID))
        {
            if ((bool)buildStrucutureButtonActivity[buttonID]){ // if returned true, that means button is activity and was pressed once again, thus we have to deactivate it
                buildStrucutureButtonActivity[buttonID] = false;
            }
            else{
                for (int j = 1; j < buildStrucutureButtonActivity.Count + 1; j++){
                    buildStrucutureButtonActivity[j] = false; 
                }
                buildStrucutureButtonActivity[buttonID] = true; 
            }
        }
    }

    //
    public bool checkForCurrentButtonState(int buttonID){
        if(buildStrucutureButtonActivity.ContainsKey(buttonID))
        {
            return (bool)buildStrucutureButtonActivity[buttonID];
        }
        else{
            return false;
        }
    }

    // on exit change states of all activity build buttons
    public void onExitChangeAllBuildButtonActivity(){
        /*var changePosBuildings = FindObjectsOfType<changePosition>();
        for (int i = 0; i < changePosBuildings.Length; i++){
		    if(changePosBuildings[i].canChange){ // check again if button is already pressed
				changePosBuildings[i].setDefaultColor();
			}
		}*/
        for (int j = 1; j < buildStrucutureButtonActivity.Count; j++){
            buildStrucutureButtonActivity[j] = false; 
        }
        playerbase.setBuildingArea(false);
    }
}
 