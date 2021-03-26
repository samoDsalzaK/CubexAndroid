using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Barrack_menu : MonoBehaviour {
    [SerializeField] GameObject menu;
   // [SerializeField] PanelCode code;
    [SerializeField] GameObject barracks;
    [SerializeField] bool isTutorialChecked=false;
    private HealthOfRegBuilding troopsResearchHealth;
    [SerializeField] ResearchConf oBGResearch;
    PanelManager panelManager;
    private void Start() {
        troopsResearchHealth = GetComponent<HealthOfRegBuilding>();
        troopsResearchHealth.setHealthOfStructureOriginal(oBGResearch.getBarrackHealth());
        troopsResearchHealth.setHealth(oBGResearch.getBarrackHealth());
        panelManager = GetComponent<PanelManager>();
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