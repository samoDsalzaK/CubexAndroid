using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Barrack_menu : MonoBehaviour {
    [SerializeField] GameObject menu;
   // [SerializeField] PanelCode code;
    [SerializeField] GameObject barracks;
    [SerializeField] bool isTutorialChecked=false;
    private HealthOfRegBuilding barrackHealth;
    [SerializeField] ResearchConf oBGResearch;
    PanelManager panelManager;
    [SerializeField] GameObject selectionCanvas;
    Base playerbase;
    private void Start() {
        barrackHealth = GetComponent<HealthOfRegBuilding>();
        barrackHealth.setHealthOfStructureOriginal(oBGResearch.getBarrackHealth());
        barrackHealth.setHealth(oBGResearch.getBarrackHealth());
        panelManager = GetComponent<PanelManager>();
        playerbase = FindObjectOfType<Base>(); 
    }
    void Update(){
        if(barrackHealth.getHealth() <= 0)
        {
			playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBarrackAmountInLevel = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerBarrackAmountInLevel - 1;
			//Debug.Log(playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerCreditsMiningStationAmountInLevel);
            playerbase.GetComponent<setFidexAmountOfStructures>().changeBuildStructureButton(1);
            Destroy(gameObject);
            //Debug.Log("Destroyed");
        }
    }
    void OnMouseDown () {
        // check for active panels in this building hierarchy if yes do not trigger on mouse click
        var status = panelManager.checkForActivePanels();
        if (status){
            return;
        }  
        else{
            // set main window
            openMenu ();
            selectionCanvas.SetActive(true);
            // deactivate other building panels
            panelManager.changeStatusOfAllPanels();
        }
        var GameTutorialManager = FindObjectOfType<GameTutorialManager>();
        if(GameTutorialManager != null && !isTutorialChecked) {
            GameTutorialManager.tutorialPanelTroopAt(4, true);
            isTutorialChecked = true;
        }
    }
    public void openMenu () {
        menu.SetActive (true);
    }
    public void destroyBarracks () {
        Destroy(barracks);
    }

}