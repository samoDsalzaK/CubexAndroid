using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameMarketButtonHandler : MonoBehaviour
{
    [Header("Configuration parameters for button handler")]
    // in game market UI components
    [SerializeField] Button inGameMarketBtn;
    [SerializeField] Text inGameMarketText;
    [SerializeField] Text inGameMarketLockedText;

    [SerializeField] GameObject inGameMarketPanel;

    [SerializeField] Button refreshMarketItemsList;

    // in game offers buttons
    [SerializeField] Button Offer1;
    [SerializeField] Button Offer2;
    [SerializeField] Button Offer3;

    [SerializeField] int numberOfOffers;

    // value boarders to take random number
    [SerializeField] int minValue; // 1
    [SerializeField] int maxValue; // 5, in order to generate numbers 1,2,3,4

    InGameMarketManager marketManager;

    // hash table for storing button activity info (pressed or not)
    Hashtable buttonActivityHashMap = new Hashtable();
    // hash table for button unlock tracking
    Hashtable buttonUnlockTracker = new Hashtable();
    // hast table for in game market offers
    Hashtable buttonInGameMarketOffersHashMap = new Hashtable();
    // playerbase type variable
    private Base playerbase;

    [SerializeField] int refreshButtonCount; // start from 0

    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectOfType<Base>() == null)
        {
            return;
        }
        else{
            playerbase = FindObjectOfType<Base>();
        }
        marketManager = GetComponent<InGameMarketManager>();
        buttonUnlockTracker.Add(1, false); // button is unlocked on game start
        buttonActivityHashMap.Add(1, false); // button is not pressed on game start
        setButtonText(1);
        for (int i = 1; i < numberOfOffers + 1; i++){
            buttonInGameMarketOffersHashMap.Add(i, false);
        }
        for (int i = 1; i < numberOfOffers + 1; i++){
            setMarketOfferButtonText(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        changeMarketButtonActivity();
    }

    void OnEnable()
    {
        //Register Button Events
        inGameMarketBtn.onClick.AddListener(() => buttonCallBack(inGameMarketBtn));
        refreshMarketItemsList.onClick.AddListener(() => buttonCallBack(refreshMarketItemsList));
    }

    private void buttonCallBack(Button buttonPressed)
    {
        //sint selection;
        if (buttonPressed == inGameMarketBtn)
        {
            if ((bool)buttonActivityHashMap[1]){ // when button will be pressed another time se button state to false (value 0) and deactivate panel
                inGameMarketPanel.SetActive(false);
                changeActivity(1,0); 
            }
            else{ // when firstly pressed market button, activate panel and set button state to true (value 1)
                playerbase.GetComponent<LocalPanelManager>().deactivatePanels();
                inGameMarketPanel.SetActive(true);
                // start card generating
                changeActivity(1,1);
            } 
        }
        else if(buttonPressed == refreshMarketItemsList){
            refreshMarketOffers();
        }
    }

     // function for setting text, when button is locked and unlocked
    public void setButtonText(int buttonID){
        marketManager = GetComponent<InGameMarketManager>();
        Color btnColour;
        switch (buttonID)
        {
            case 1:
                if((bool)buttonUnlockTracker[buttonID]){ // if unlocked 
                    int selectedSkinValue = PlayerPrefs.GetInt("skinSelection"); // grab selected skin value
                    if (selectedSkinValue == 1){
                        btnColour = new Color32(15,41, 243, 255); // default colour
                        inGameMarketBtn.GetComponent<UnityEngine.UI.Image>().color  = btnColour;
                    }
                    else if (selectedSkinValue == 2){
                        btnColour = new Color32(255,181, 0, 231); // orange
                        inGameMarketBtn.GetComponent<UnityEngine.UI.Image>().color  = btnColour;
                    }
                    else if (selectedSkinValue == 3){
                        btnColour = new Color32(0,198, 255, 255); // light blue
                        inGameMarketBtn.GetComponent<UnityEngine.UI.Image>().color  = btnColour;
                    }
                    else if (selectedSkinValue == 4){
                        btnColour = new Color32(34,140, 0, 255); // green
                        inGameMarketBtn.GetComponent<UnityEngine.UI.Image>().color  = btnColour;
                    }
                    inGameMarketBtn.interactable = true;
                    inGameMarketLockedText.gameObject.SetActive(false);
                    inGameMarketText.gameObject.SetActive(true);
                    inGameMarketText.text = "Market";
                }
                else{ // if button is not unlocked
                    inGameMarketBtn.interactable = false;
                    inGameMarketText.gameObject.SetActive(false);
                    inGameMarketLockedText.gameObject.SetActive(true);
                    inGameMarketLockedText.text = "Market Locked" + "\n" + "Base level " + marketManager.getMinLevelNeeded + " needed";
                }
                break;
            default:
                break;
        }
    }

    // function for unlocking in game market button
    public void unlockBtn(int buttonID){ 
        if (buttonUnlockTracker.ContainsKey(buttonID)){
            buttonUnlockTracker[buttonID] = true; // setting value to true, means that button is unlocked
        }
    }

    // function for change button activity info
    public void changeActivity(int buttonID, int state){ 
        if (state == 1){
            if (buttonActivityHashMap.ContainsKey(buttonID)){
                buttonActivityHashMap[buttonID] = true; 
            }
        }
        else if (state == 0){
            if (buttonActivityHashMap.ContainsKey(buttonID)){
                buttonActivityHashMap[buttonID] = false; 
            }
        }
    }

    // function for changing button activity on panel exit
    public void changeActivityOnExit(){
        changeActivity(1,0);
    }

    // change market button activity if player has clicked somewhere on the map
    public void changeMarketButtonActivity(){
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
                        changeActivity(1,0);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }

    // function for changing in game market offer button activity
    public void changeStateOfInGameMarketBtn(int buttonID, int state){
        if (state == 0){
            if (buttonInGameMarketOffersHashMap.ContainsKey(buttonID)){
                buttonInGameMarketOffersHashMap[buttonID] = false; // setting value to false, means that button was not pressed yet after market refresh
            }  
        }
        else if (state == 1){
            if (buttonInGameMarketOffersHashMap.ContainsKey(buttonID)){
                buttonInGameMarketOffersHashMap[buttonID] = true; // setting value to true, means that button was pressed
            }  
        }
    }

    // function for setting text on market offers buttons
    public void setMarketOfferButtonText(int buttonID){
        marketManager = GetComponent<InGameMarketManager>();
        //string buttonText = ""; 
        switch(buttonID){
            case 1:
                if ((bool)buttonInGameMarketOffersHashMap[buttonID]){ // button was alrady pressed
                    Offer1.GetComponentInChildren<Text>().text = "Offer Sold";
                    Offer1.interactable = false;
                }
                else{ // button has not been pressed yet
                    assignRandomParamsToButton(Offer1, buttonID);
                }
                break;
            case 2:
                if ((bool)buttonInGameMarketOffersHashMap[buttonID]){ // button was alrady pressed
                    Offer2.GetComponentInChildren<Text>().text = "Offer Sold";
                    Offer2.interactable = false;
                }
                else{ // button has not been pressed yet
                    assignRandomParamsToButton(Offer2, buttonID);
                }
                break;
            case 3:
                if ((bool)buttonInGameMarketOffersHashMap[buttonID]){ // button was alrady pressed
                    Offer3.GetComponentInChildren<Text>().text = "Offer Sold";
                    Offer3.interactable = false;
                }
                else{ // button has not been pressed yet
                    assignRandomParamsToButton(Offer3, buttonID);
                }
                break;
            default:
                break;
        }
    }

    // function for assigning random params to market offer buttons
    public void assignRandomParamsToButton(Button button, int buttonID){
        int selection = Random.Range(minValue,maxValue);
        switch(selection){
            case 1:
                button.GetComponentInChildren<Text>().text = marketManager.getBoughtEnergonValue + " energon" + "\n" + "Price : " + marketManager.getPriceInCredits + " credits";
                button.onClick.RemoveAllListeners(); // removing all previuos listeners from this button
                button.onClick.AddListener(() => marketManager.buyEnergon(marketManager.getPriceInCredits,marketManager.getBoughtEnergonValue, buttonID));
                button.interactable = true;
                break;
            case 2:
                button.GetComponentInChildren<Text>().text = marketManager.getBoughtCreditsValue + " credits" + "\n" + "Price : " + marketManager.getPriceInEnergon + " energon";
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => marketManager.buyCredits(marketManager.getPriceInEnergon,marketManager.getBoughtCreditsValue, buttonID));
                button.interactable = true;
                break;
            case 3:
                int timeSelection = Random.Range(1,4);
                if (timeSelection == 1){
                    button.GetComponentInChildren<Text>().text = "+ " + marketManager.getLevelTimeValues[timeSelection-1] + " level time minute" + "\n" + "Price : 20 credits";
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => marketManager.inCreaseLevelTime(20,"credits", marketManager.getLevelTimeValues[timeSelection-1], buttonID));
                    button.interactable = true;
                }
                else if (timeSelection == 2)
                {
                    button.GetComponentInChildren<Text>().text = "+ " + marketManager.getLevelTimeValues[timeSelection-1] + " level time minutes" + "\n" + "Price : 50 energon";
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => marketManager.inCreaseLevelTime(50,"energon", marketManager.getLevelTimeValues[timeSelection-1], buttonID));
                    button.interactable = true;
                }
                else if (timeSelection == 3){
                    button.GetComponentInChildren<Text>().text = "+ " + marketManager.getLevelTimeValues[timeSelection-1] + " level time minutes" + "\n" + "Price : 100 credits";
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => marketManager.inCreaseLevelTime(100,"credits", marketManager.getLevelTimeValues[timeSelection-1], buttonID));
                    button.interactable = true;
                }
                break;
            case 4:
                int troopCapacitySelection = Random.Range(1,4);
                if (troopCapacitySelection == 1){
                    button.GetComponentInChildren<Text>().text = "+ " + marketManager.getTroopsCapacityValues[troopCapacitySelection-1] + " troops capacity" + "\n" + "Price : 20 energon";
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => marketManager.inCreaseTroopsCapacity(20,"energon",marketManager.getTroopsCapacityValues[troopCapacitySelection-1], buttonID));
                    button.interactable = true;
                }
                else if (troopCapacitySelection == 2){
                    button.GetComponentInChildren<Text>().text = "+ " + marketManager.getTroopsCapacityValues[troopCapacitySelection-1] + " troops capacity" + "\n" + "Price : 50 credits";
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => marketManager.inCreaseTroopsCapacity(50,"credits",marketManager.getTroopsCapacityValues[troopCapacitySelection-1], buttonID));
                    button.interactable = true;
                }
                else if (troopCapacitySelection == 3){
                    button.GetComponentInChildren<Text>().text = "+ " + marketManager.getTroopsCapacityValues[troopCapacitySelection-1] + " troops capacity" + "\n" + "Price : 100 energon";
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => marketManager.inCreaseTroopsCapacity(100,"energon",marketManager.getTroopsCapacityValues[troopCapacitySelection-1], buttonID));
                    button.interactable = true;
                }
                break;
            default:
                break;
        }
    }

    public void refreshMarketOffers(){
        for (int i = 1; i < numberOfOffers + 1; i++){ // refresh all buttons with new text and items
            buttonInGameMarketOffersHashMap[i] = false;
            setMarketOfferButtonText(i);
        }
        refreshButtonCount++;
        if (refreshButtonCount == 1){
            refreshMarketItemsList.interactable = false;
            marketManager.startCoolDown(0);
        }
        else if (refreshButtonCount == 2){
            refreshMarketItemsList.interactable = false;
            marketManager.startCoolDown(1);
        }
        else if (refreshButtonCount == 3){
            refreshMarketItemsList.interactable = false;
            marketManager.startCoolDown(2);
        }
        else {
            refreshMarketItemsList.interactable = false;
            marketManager.startCoolDown(3);
        }
    }

    public void unlockRefreshBtn(){
        refreshMarketItemsList.GetComponentInChildren<Text>().text = "Refresh";
        refreshMarketItemsList.interactable = true;
    }
}
