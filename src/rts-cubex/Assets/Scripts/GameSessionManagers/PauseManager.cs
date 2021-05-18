using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class PauseManager : MonoBehaviour
{
    PanelManager panelManager;
    //private TMP_Text m_TextComponent;
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
        //m_TextComponent = GetComponent<TMP_Text>();
 
        // Change the text on the text component.
        //m_TextComponent.text = "Some new line of text.";
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
