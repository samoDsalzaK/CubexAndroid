﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    PanelManager panelManager;
    private void Start() {
        
    }
    // Update is called once per frame
    void Update()
    {

    }

    // game pause 
    public void pauseGame()
    {
        panelManager = GetComponent<PanelManager>();
        panelManager.changeStatusOfAllPanels();
        Time.timeScale = 0;
        //diabling all active panels when game is paused
        //float seconds = (int)(Time.timeSinceLevelLoad % 60f);
        //float minutes = (int)(Time.timeSinceLevelLoad / 60f) % 60;
        //Debug.Log(minutes.ToString("00") + ":" + seconds.ToString("00"));
    }
    // game resume
    public void unpauseGame()
    {
        Time.timeScale = 1;
    }
}
