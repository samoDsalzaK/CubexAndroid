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

    public void changeStatusOfAllPanels(){
        var activePanels = FindObjectsOfType<PanelManager>();
		if (activePanels != null){  
			for (int i = 0; i < activePanels.Length; i++){
				if(activePanels[i].changeStatus){
					activePanels[i].changeStatus = false;
				}
			}
		}
        isActive = true;
        for (int i = 0; i < activePanels.Length; i++){
            if(!activePanels[i].changeStatus){
                activePanels[i].deactivatePanels();
            }
        }
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

    // this function will be used for pause manager
    public void onGamePauseDisableAllPanels(){
        var activePanels = FindObjectsOfType<PanelManager>();
        if (activePanels != null){  
            for (int i = 0; i < activePanels.Length; i++){
                if(activePanels[i].changeStatus){
                    activePanels[i].changeStatus = false;
                }
                activePanels[i].deactivatePanels();
            }
        }
        else{
            return;
        }
    }
}
