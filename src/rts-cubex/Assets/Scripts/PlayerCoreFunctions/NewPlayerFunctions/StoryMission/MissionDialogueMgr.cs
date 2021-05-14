using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionDialogueMgr : MonoBehaviour
{
    [Header("UI configuration")]
    [Header("Dialogue config:")]
    [SerializeField] GameObject proPotraint;
    [SerializeField] GameObject troopPotraint;
    [Header("Other UI config:")]
    [Header("Dialogue info")]
    [SerializeField] string heroineRank = "(Captain)";
    [SerializeField] GameObject buttonNext;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject menuBtn;
    [SerializeField] GameObject dWindow;
    [SerializeField] Text speakerName;
    [SerializeField] Text displayText;
    [SerializeField] StoryData sdata;
    [SerializeField] bool startSpeaking = false;
    [SerializeField] int startIndex = 0;
    [SerializeField] int dMax = 1;
    [SerializeField] float minRequiredDistance = 7f;
    private SLevelManager slmgr;
    private GameObject spawnedHero;
    private Vector3 arrivalPoint;
    private bool act1Open = true; //On start open by default
    private bool act2Open = false; //Bonus act
    private bool act3Open = false;
    private bool act4Open = false;
    private bool act5Open = false;

    public bool Act2Open { set {act2Open = value; } get { return act2Open; }}
    public bool Act3Open { set {act3Open = value; } get { return act3Open; }}
    public bool Act4Open { set {act4Open = value; } get { return act4Open; }}
    public bool Act5Open { set {act5Open = value; } get { return act5Open; }}

    void Start()
    {
        slmgr = GetComponent<SLevelManager>();
        arrivalPoint = slmgr.MSquadStartArrivalPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnedHero)
        {
            spawnedHero = slmgr.SpawnedHero;
        }
        if (act1Open)
        {
            var distance = Vector3.Distance(spawnedHero.transform.position, arrivalPoint);
            //print("Distance to point: " + distance);
            if (distance < minRequiredDistance)
            {
                //play dialogue
                dWindow.SetActive(true);
                
                Time.timeScale = 0f;
                dMax = 7;

                //Assigning first dialogue script
                var dataText = sdata.DialogueLines[startIndex];
                if (dataText != null)
                {
                    
                    if (dataText.Contains("[H]") )
                    {

                        spawnedHero.GetComponent<ClickOn>().HeroToolbar.SetActive(false);

                        speakerName.text = spawnedHero.name.Substring(0, 3) + heroineRank;
                        proPotraint.SetActive(true);
                        troopPotraint.SetActive(false);
                    }
                    
                    displayText.text = dataText.Remove(0, 3);   
                   
                }

                act1Open = false;
            }
        }
        if (act2Open) //Troop saving bonus act
        {
            //Play dialogue
            dWindow.SetActive(true);
            //For overlapping purpose hero
            //spawnedHero.GetComponent<ClickOn>().IsSelected = false;
            Time.timeScale = 0f;

            //Switching to bonus dialogue
            startIndex = 7;
            dMax = 10;

            var dataText = sdata.DialogueLines[startIndex];
            if (dataText != null)
            {
                   
                if (dataText.Contains("[TS]") )
                {
                    spawnedHero.GetComponent<ClickOn>().HeroToolbar.SetActive(false);

                    speakerName.text = "Surviving troop(Leader)";
                    proPotraint.SetActive(false);
                    troopPotraint.SetActive(true);
                }
                    
                displayText.text = dataText.Remove(0, 3);   
                   
            }

            //Closing act 2 bonus
            act2Open = false;
        }
        if (act3Open)
        {
             //Play dialogue
            dWindow.SetActive(true);
            //For overlapping purpose hero
            //spawnedHero.GetComponent<ClickOn>().IsSelected = false;
            Time.timeScale = 0f;

            //Switching to main act dialogue
            startIndex = 10;
            dMax = 11;

           
            var dataText = sdata.DialogueLines[startIndex];
            if (dataText != null)
            {
                   
                if (dataText.Contains("[H]") )
                {
                    spawnedHero.GetComponent<ClickOn>().HeroToolbar.SetActive(false);

                    speakerName.text = spawnedHero.name.Substring(0, 3) + heroineRank;
                    proPotraint.SetActive(true);
                    troopPotraint.SetActive(false);
                }
                    
                displayText.text = dataText.Remove(0, 3);   
                   
            }

            act3Open = false;
        }
        if (act4Open) //After bringing resources to base print next dialogue
        {
             //Play dialogue
            dWindow.SetActive(true);
            //For overlapping purpose hero
            //spawnedHero.GetComponent<ClickOn>().IsSelected = false;
            Time.timeScale = 0f;

            //Switching to main act dialogue
            startIndex = 11;
            dMax = 14;

           
            var dataText = sdata.DialogueLines[startIndex];
            if (dataText != null)
            {
                   
                if (dataText.Contains("[H]") )
                {
                    spawnedHero.GetComponent<ClickOn>().HeroToolbar.SetActive(false);

                    speakerName.text = spawnedHero.name.Substring(0, 3) + heroineRank;
                    proPotraint.SetActive(true);
                    troopPotraint.SetActive(false);
                }
                    
                displayText.text = dataText.Remove(0, 3);   
                   
            }
            act4Open = false;
        }
        if (act5Open) //After surviving the approching enemy wave encounter print last dialogue
        {
             //Play dialogue
            dWindow.SetActive(true);
            //Configuring required buttons
            exitButton.SetActive(true);
            restartButton.SetActive(true);
            buttonNext.SetActive(false);
            menuBtn.SetActive(false);
            //For overlapping purpose hero
            //spawnedHero.GetComponent<ClickOn>().IsSelected = false;
            Time.timeScale = 0f;

            //Switching to main act dialogue
            startIndex = 14;
            dMax = 15;

           
            var dataText = sdata.DialogueLines[startIndex];
            if (dataText != null)
            {
                   
                if (dataText.Contains("[H]") )
                {
                    spawnedHero.GetComponent<ClickOn>().HeroToolbar.SetActive(false);

                    speakerName.text = spawnedHero.name.Substring(0, 3) + heroineRank;
                    proPotraint.SetActive(true);
                    troopPotraint.SetActive(false);
                }
                    
                displayText.text = dataText.Remove(0, 3);   
                   
            }
            act5Open = false;
        }
    }
    //Method to play the dialogue
    public void goToNextText()
    {         
        startIndex++;
         //Checking if eneding of the dialogue is reached
        if (startIndex >= dMax)
        {
            dWindow.SetActive(false);
            Time.timeScale = 1f;
            return;
        }      

       var dataText = sdata.DialogueLines[startIndex];

       if (dataText.Contains("[H]") )
       {
            speakerName.text = spawnedHero.name.Substring(0, 3) + heroineRank;
            proPotraint.SetActive(true);
            troopPotraint.SetActive(false);
       }
       if (dataText.Contains("[T]") )
       {
            speakerName.text = "Nearby trooper(Squad member)";
            proPotraint.SetActive(false);
            troopPotraint.SetActive(true);
       }
       if (dataText.Contains("[TS]") )
       {
            speakerName.text = "Surviving troop(Leader)";
            proPotraint.SetActive(false);
            troopPotraint.SetActive(true);
       }  

       displayText.text = dataText.Remove(0, 3);

    }
}
