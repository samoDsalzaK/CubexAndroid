﻿using System.Collections;
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
    private void Start() {
        troopsResearchHealth = GetComponent<HealthOfRegBuilding>();
        troopsResearchHealth.setHealthOfStructureOriginal(oBGResearch.getBarrackHealth());
        troopsResearchHealth.setHealth(oBGResearch.getBarrackHealth());
    }
    void OnMouseDown () {
        var GameTutorialManager = FindObjectOfType<GameTutorialManager>();
        if(GameTutorialManager != null && !isTutorialChecked) {
            GameTutorialManager.tutorialPanelTroopAt(4, true);
            isTutorialChecked = true;
        }
            openMenu ();
    }
    public void openMenu () {
        menu.SetActive (true);
    }
    public void destroyBarracks () {
        Destroy(barracks);
    }

}