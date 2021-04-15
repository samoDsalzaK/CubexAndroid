using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeSkinButtonHandler : MonoBehaviour
{
    // skin ids
    // 1 - Starter
    // 2 - Pyro
    // 3 - Ice
    // 4 - Earth 
    [Header("Configuration parameters for button handler")]
    // change skin buttons
    [SerializeField] Button StarterAssetSkinBtn;
    [SerializeField] Button PyroAssetSkinBtn;
    [SerializeField] Button IceAssetSkinBtn;
    [SerializeField] Button EarthAssetSkinBtn;

    // change skin buttons texts
    [SerializeField] Text StarterAssetSkinBtnText;
    [SerializeField] Text PyroAssetSkinBtnText;
    [SerializeField] Text IceAssetSkinBtnText;
    [SerializeField] Text EarthAssetSkinBtnText;

    // change skin info buttons
    [SerializeField] Button StarterAssetSkinInfoBtn;
    [SerializeField] Button PyroAssetSkinInfoBtn;
    [SerializeField] Button IceAssetSkinInfoBtn;
    [SerializeField] Button EarthAssetSkinInfoBtn;

    // change skin info panels
    [SerializeField] GameObject StarterAssetSkinInfoPanel, PyroAssetSkinInfoPanel, IceAssetSkinInfoPanel, EarthAssetSkinInfoPanel;

    // hash table for storong button activity info (pressed or not)
    Hashtable buttonActivityHashMap = new Hashtable();

    [SerializeField] int btnID; // previously pressed button
    [SerializeField] int numberOfAssets; // in our case it is 4

    [SerializeField] string[] skinNames;

    PanelManager panelManager;
    // Start is called before the first frame update
    void Start()
    {
        // fill in hash table
        // if button is active state is true, otherwise it is false
        int skinSelection = PlayerPrefs.GetInt("skinSelection"); // grab previuosly seleceted skin and set as selected on game start
        //Debug.Log(PlayerPrefs.GetString("skinName"));
        for (int i = 1; i < numberOfAssets + 1; i++){
            if(i == skinSelection){
                buttonActivityHashMap.Add(i, true);
            }
            else{
                buttonActivityHashMap.Add(i, false);
            }
        }
        // setting text fields to buttons
        for(int j = 1; j < buttonActivityHashMap.Count + 1; j++){
            setButtonText(j);
        }
        panelManager = GetComponent<PanelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        //Register Button Events
        StarterAssetSkinBtn.onClick.AddListener(() => buttonCallBack(StarterAssetSkinBtn));
        PyroAssetSkinBtn.onClick.AddListener(() => buttonCallBack(PyroAssetSkinBtn));
        IceAssetSkinBtn.onClick.AddListener(() => buttonCallBack(IceAssetSkinBtn));
        EarthAssetSkinBtn.onClick.AddListener(() => buttonCallBack(EarthAssetSkinBtn));
        StarterAssetSkinInfoBtn.onClick.AddListener(() => buttonCallBack(StarterAssetSkinInfoBtn));
        PyroAssetSkinInfoBtn.onClick.AddListener(() => buttonCallBack(PyroAssetSkinInfoBtn));
        IceAssetSkinInfoBtn.onClick.AddListener(() => buttonCallBack(IceAssetSkinInfoBtn));
        EarthAssetSkinInfoBtn.onClick.AddListener(() => buttonCallBack(EarthAssetSkinInfoBtn));
    }


    private void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == StarterAssetSkinBtn)
        {
            changeButtonState(1, skinNames[0]);
            panelManager.deactivatePanels();
        }
        else if (buttonPressed == PyroAssetSkinBtn)
        {
            changeButtonState(2, skinNames[1]);
            panelManager.deactivatePanels();
        }
        else if (buttonPressed == IceAssetSkinBtn)
        {
            changeButtonState(3, skinNames[2]);
            panelManager.deactivatePanels();
        }
        else if (buttonPressed == EarthAssetSkinBtn){
            changeButtonState(4, skinNames[3]);
            panelManager.deactivatePanels();
        }
        else if (buttonPressed == StarterAssetSkinInfoBtn){
            panelManager.deactivatePanels();
            panelManager.activatePanel(StarterAssetSkinInfoPanel);
        }
        else if (buttonPressed == PyroAssetSkinInfoBtn){
            panelManager.deactivatePanels();
            panelManager.activatePanel(PyroAssetSkinInfoPanel);
        }
        else if (buttonPressed == IceAssetSkinInfoBtn){
            panelManager.deactivatePanels();
            panelManager.activatePanel(IceAssetSkinInfoPanel);
        }
        else if (buttonPressed == EarthAssetSkinInfoBtn){
            panelManager.deactivatePanels();
            panelManager.activatePanel(EarthAssetSkinInfoPanel);
        }
    }

    // function for activating and deactivating buttons
    public void changeButtonState(int buttonID, string skinName){
        if(buttonActivityHashMap.ContainsKey(buttonID))
        {
            // iterate and set to all buttons state to false
            for (int j = 1; j < buttonActivityHashMap.Count + 1; j++){
                buttonActivityHashMap[j] = false; 
            }
            buttonActivityHashMap[buttonID] = true; // if button is pressed, change status to true
            for(int i = 1; i < buttonActivityHashMap.Count + 1; i++){
                if(i == buttonID || i == btnID){ // only call function for 2 buttons, previous and which is pressed now
                    setButtonText(i);
                }
            }
            PlayerPrefs.DeleteKey("skinSelection"); // delete privious skin selection
            PlayerPrefs.DeleteKey("skinName");
            PlayerPrefs.SetInt("skinSelection", buttonID); // add current skin selection
            PlayerPrefs.SetString("skinName", skinName);
            PlayerPrefs.Save(); // save current skin selection to disk
            btnID = buttonID; // save priviously pressed button index
        } 
    }
    // function for setting text
    public void setButtonText(int buttonID){
        switch (buttonID)
        {
            // starter
            case 1:
                if ((bool)buttonActivityHashMap[buttonID]){ // if button is active its value is true, otherwise is false
                    StarterAssetSkinBtnText.text = "Starter" + "\n" + "(selected)";
                    //Debug.Log("Button 1 is active");
                    // set active text
                }
                else{
                    StarterAssetSkinBtnText.text = "Starter";
                    //Debug.Log("Button 1 is not active");
                    // set not active text
                }
                break;
            // pyro
            case 2:
                if ((bool)buttonActivityHashMap[buttonID]){
                    PyroAssetSkinBtnText.text = "Pyro" + "\n" + "(selected)";
                    //Debug.Log("Button 2 is active");
                }
                else{
                    PyroAssetSkinBtnText.text = "Pyro";
                    //Debug.Log("Button 2 is not active");
                }
                break;
            //ice    
            case 3:
                if ((bool)buttonActivityHashMap[buttonID]){
                    IceAssetSkinBtnText.text = "Ice" + "\n" + "(selected)";
                    //Debug.Log("Button 3 is active");
                }
                else{
                    IceAssetSkinBtnText.text = "Ice";
                    //Debug.Log("Button 3 is not active");
                }
                break;
            //earth    
            case 4:
                if ((bool)buttonActivityHashMap[buttonID]){
                    EarthAssetSkinBtnText.text = "Earth" + "\n" + "(selected)";
                    //Debug.Log("Button 4 is active");
                }
                else{
                    EarthAssetSkinBtnText.text = "Earth";
                    //Debug.Log("Button 4 is not active");
                }
                break;
            default:
                break;
        }
        return;
    }
}
