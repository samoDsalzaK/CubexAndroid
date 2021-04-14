using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelManager : MonoBehaviour
{
    [Header("Configuration parameters panel manager")]
    [SerializeField] GameObject[] panels;

    bool isActive = false;

    public bool changeStatus {get {return isActive;} set {isActive = value;}}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkIfClickedOnTheMap();
    }

    public void deactivatePanels(){
        foreach(GameObject panel in panels){
            panel.SetActive(false);
        }
    }

    // this function is used by every single building in the map, here is not good to enter with state as true, because that could couse loop and your panels will be dactivate every time you click on structure 
    public void changeStatusOfAllPanels(){
        var activePanels = FindObjectsOfType<PanelManager>();
		if (activePanels != null){  
			for (int i = 0; i < activePanels.Length; i++){
				if(activePanels[i].changeStatus){
					activePanels[i].changeStatus = false;
                    activePanels[i].deactivatePanels();
				}
			}
		}
        // make yourself to be visible
        isActive = true;
    }

    // this function ignores player click on the object if another panel of this object is currently active
    public bool checkForActivePanels(){
        foreach(GameObject panel in panels){
            if(panel.activeInHierarchy){
                return true;
            }
        }
        return false;
    }

    // this function will deactive active panel on the building if player has clicked elsewhere on the map
    public void checkIfClickedOnTheMap(){
        if (Input.GetMouseButtonDown(0))
        {
            // checks mouse click is over UI panel, if not will process futher and deactivate panels
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") || hit.transform.gameObject.layer == LayerMask.NameToLayer("LvlMap"))
                    {
                        isActive = false;
                        deactivatePanels();
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }

    // this function will be useful for objects with big colliders, to ignore clicking
    public bool checkIfWhereAreActivePanelsOnTheMap(){
        var activePanels = FindObjectsOfType<PanelManager>();
        if(activePanels != null){
            for (int i = 0; i < activePanels.Length; i++){ 
                if(activePanels[i].changeStatus){
                   return true;
                }
            }
            return false;
        }
        return false;
    }

    // function will be useful for exit butttons on panels, to fix bug, when building remains to be active in panel usage, and can not be clicked once again 
    public void deactivatePanelsOnExit(){
        var activePanels = FindObjectsOfType<PanelManager>();
        if(activePanels != null){
            for (int i = 0; i < activePanels.Length; i++){ 
                activePanels[i].changeStatus = false;
            }
        }
    }

    // function needed to deactivate playerbase panels, because several panel management is on another building and we can not deactivate them as child objects
    public void onExitDeactivatePlayerBasePanels(){
        var playerbase = FindObjectOfType<Base>();
        playerbase.setResourceAMountScreenStateForUpgrade(false); 
        playerbase.setErrorStateToBuildStructure(false);
    }

    public void activatePanel(GameObject panel){
        panel.SetActive(true);
    }

    public void deactivatePanel(GameObject panel){
        panel.SetActive(false);
    }
}
