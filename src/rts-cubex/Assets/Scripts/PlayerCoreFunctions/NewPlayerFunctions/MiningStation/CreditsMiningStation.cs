﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreditsMiningStation : MonoBehaviour
{
    [Header("Configuration parameters Mining Station")]
    //[SerializeField] GameStorageConf userAccount; // user account data
    [SerializeField] GameObject MainMiningStationPanel;
    [SerializeField] GameObject errorForMining1; // error panel if user do not have min needed energon amount to mine
    [SerializeField] GameObject errorForMining2; // error panel if storage is full and user has to redeem mined credits
    [SerializeField] GameObject selectionCanvas;
    [SerializeField] Text playerMinedCreditsTextField;
    [SerializeField] Text upgradePaneCredits; // upgrade panel credits amount
    [SerializeField] Text upgradePaneEnergon; // upgrade panel energon amount
    [SerializeField] Text slotsPanelEnergonTextField;

    // playerbase type variable
    private Base playerbase;

    // panel Manager type variable
    PanelManager panelManager;

    // variables needed for slots defition
    [SerializeField] int[] minNeededEnergonAmount; // the price in energon needed to mine specific amount of credits
    [SerializeField] float[] timeNeedForMining;
    [SerializeField] int[] rewards; // every slot credits reward
    [SerializeField] float[] timeNeedForSlotCoolDown; // slot cool Down time
 
    [SerializeField] int minedCreditsAmount; // current mined Credits value
    [SerializeField] private int maxMinedCreditsAmount; // capasity limit

    createAnimatedPopUp animatedPopUps;

    public int[] changeMinNeededEnergonAmount
    {
        get
        {
            return minNeededEnergonAmount;
        }
        set
        {
            minNeededEnergonAmount = value;
        }
    }

    public float[] changeTimeNeedForMining
    {
        get
        {
            return timeNeedForMining;
        }
        set
        {
            timeNeedForMining = value;
        }
    }

    public int[] changeRewards
    {
        get
        {
            return rewards;
        }
        set
        {
            rewards = value;
        }
    }

    public float[] changeTimeNeedForSlotCoolDown {get{return timeNeedForSlotCoolDown;} set{timeNeedForSlotCoolDown = value;}}

    public int changeMinedCreditsAmount {set {minedCreditsAmount=value;} get {return minedCreditsAmount;}}

    TimerForMining timer; // creating Timer type variable

    changePosition changePos;

    // Start is called before the first frame update
    void Start()
    {
        MainMiningStationPanel.SetActive(false);
        if(FindObjectOfType<Base>() == null)
        {
          return;
        }
        else{
            playerbase = FindObjectOfType<Base>();
        }

        panelManager = GetComponent<PanelManager>();
        animatedPopUps = playerbase.GetComponent<createAnimatedPopUp>();
        changePos = GetComponent<changePosition>();
    }
    // Update is called once per frame
    void Update()
    {
        upgradePaneCredits.text = "Credits left : " + playerbase.getCreditsAmount(); 
        upgradePaneEnergon.text = "Energon left : " + playerbase.getEnergonAmount(); 
        slotsPanelEnergonTextField.text =  "Energon left : " + playerbase.getEnergonAmount(); 
        playerMinedCreditsTextField.text = "Mined Credits : " + minedCreditsAmount  + " / "  + maxMinedCreditsAmount;
    }

    void OnMouseDown()
    {
        // check for active panels in this building hierarchy if yes do not trigger on mouse click
        var status = panelManager.checkForActivePanels();
        if (status){
            return;
        }  
        else{
            // set main window
            MainMiningStationPanel.SetActive(true);
            selectionCanvas.SetActive(true);
            // deactivate other building panels
            panelManager.changeStatusOfAllPanels();
            changePos.setDefaultValues();
        }
        // check for active panels in this building hierarchy and deactivate them
        //panelManager.deactivateParentPanels(MainMiningStationPanel)
    }

    public void miningAction(int slotIndex)
    {
        // check if player has enough starting energon to mine credits
        if (playerbase.getEnergonAmount() < minNeededEnergonAmount[slotIndex-1]) // patikrina esamus zaidejo resursus
        {
        errorForMining1.SetActive(true);  
        Debug.Log("Insuffiecient energon balance"); 
        return; 
        }
        // check if storage of mined Credits can handle new amount of mined Credits
        if (minedCreditsAmount + rewards[slotIndex-1] > maxMinedCreditsAmount){
        errorForMining2.SetActive(true);  
        Debug.Log("First redeem and then mine again");
        return;
        }
        animatedPopUps.createDecreaseEnergonPopUp(minNeededEnergonAmount[slotIndex-1]);
        // set new value for energon
        playerbase.setEnergonAmount(playerbase.getEnergonAmount() - minNeededEnergonAmount[slotIndex-1]);
        // addition check for resources
        timer = FindObjectOfType<TimerForMining>();
        timer.startTimer(timeNeedForMining[slotIndex-1], slotIndex);
    }


    public void reedemCreditsAction(){
        animatedPopUps.createAddCreditsPopUp(minedCreditsAmount);
        playerbase.setCreditsAmount(playerbase.getCreditsAmount() + minedCreditsAmount);
        if(minedCreditsAmount > 0){
            playerbase.GetComponent<PlayerScoring>().addScoreAfterCreditsReedem(minedCreditsAmount);
        }
        minedCreditsAmount = 0;
    }
}
