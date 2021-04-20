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
    InGameMarketManager marketManager;

    // hash table for storing button activity info (pressed or not)
    Hashtable buttonActivityHashMap = new Hashtable();
    // hash table for button unlock tracking
    Hashtable buttonUnlockTracker = new Hashtable();
    // hast table for in game market offers
    Hashtable buttonInGameMarketOffersHashMap = new Hashtable();
    // playerbase type variable
    private Base playerbase;

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
        for (int i = 1; i < 4; i ++){
            buttonInGameMarketOffersHashMap.Add(i, false);
        }
        setButtonText(1);
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
        else if (buttonPressed == refreshMarketItemsList){
            // refresh list, generate new items
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
                else{
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

    public void offerButtonTracker(){
        // will make button set active false
    }
}
