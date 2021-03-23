using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /*public void deactivateParentPanels(GameObject mainPanel){
        foreach(GameObject panel in panels){
            if(panel != mainPanel){
                panel.SetActive(false);
            }
        }
    }*/

    public bool checkForActivePanels(){
        foreach(GameObject panel in panels){
            if(panel.activeInHierarchy){
                return true;
            }
        }
        return false;
    }
}
