using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeSkinButtonHandler : MonoBehaviour
{
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

    // hash table for storong button activity info (pressed or not)
    Hashtable buttonActivityHashMap = new Hashtable();

    [SerializeField] int btnID; // previously pressed button
    // Start is called before the first frame update
    void Start()
    {
        // fill in hash table
        // if button is active state is true, otherwise it is false
        for (int i = 1; i < 5; i++){
            if(i == 1){
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
    }


    private void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == StarterAssetSkinBtn)
        {
            changeButtonState(1);
        }
        else if (buttonPressed == PyroAssetSkinBtn)
        {
            changeButtonState(2);
        }
        else if (buttonPressed == IceAssetSkinBtn)
        {
            changeButtonState(3);
        }
        else if (buttonPressed == EarthAssetSkinBtn){
            changeButtonState(4);
        }
    }

    // function for activating and deactivating buttons
    public void changeButtonState(int buttonID){
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
                    Debug.Log("Button 1 is active");
                    // set active text
                }
                else{
                    Debug.Log("Button 1 is not active");
                    // set not active text
                }
                break;
            // pyro
            case 2:
                if ((bool)buttonActivityHashMap[buttonID]){
                    Debug.Log("Button 2 is active");
                }
                else{
                    Debug.Log("Button 2 is not active");
                }
                break;
            //ice    
            case 3:
                if ((bool)buttonActivityHashMap[buttonID]){
                    Debug.Log("Button 3 is active");
                }
                else{
                    Debug.Log("Button 3 is not active");
                }
                break;
            //earth    
            case 4:
                if ((bool)buttonActivityHashMap[buttonID]){
                    Debug.Log("Button 4 is active");
                }
                else{
                    Debug.Log("Button 4 is not active");
                }
                break;
            default:
                break;
        }
        return;
    }
}
