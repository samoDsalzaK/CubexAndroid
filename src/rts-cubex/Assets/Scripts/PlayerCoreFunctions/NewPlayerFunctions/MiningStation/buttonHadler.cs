using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonHadler : MonoBehaviour
{
    [Header("Configuration parameters for button handler")]
    // slot buttons
    [SerializeField] Button mineSlot1btn;
    [SerializeField] Button mineSlot2btn;
    [SerializeField] Button mineSlot3btn;

    // upgrade buttons
    [SerializeField] Button upgradeBtn;

    // change Position buttons
    [SerializeField] Button changePosBtn;

    // text fields on slot buttons
    [SerializeField] Text slot1TimeText;
    [SerializeField] Text mineSlot1PriceText; 
    [SerializeField] Text mineSlot1RewardText;

    [SerializeField] Text slot2TimeText;
    [SerializeField] Text mineSlot2PriceText; 
    [SerializeField] Text mineSlot2RewardText;

    [SerializeField] Text slot3TimeText;
    [SerializeField] Text mineSlot3PriceText; 
    [SerializeField] Text mineSlot3RewardText;

    [SerializeField] Text lockedSlot2;
    [SerializeField] Text lockedSlot3;

    [SerializeField] Image slot2Image;
    [SerializeField] Image slot3Image;

    // mining station type variable
    CreditsMiningStation miningStation;

    // upgrade manager type varibale
    MiningStationUpgradeManager upgradeManager;

    // change position type variable
    changePosition changePos;

    // hash table for storing button actvity info
    Hashtable buttonEnabledHashMap = new Hashtable();

    // Start is called before the first frame update
    void Start()
    {

        if(FindObjectOfType<CreditsMiningStation>() == null)
        {
           return;
        }
        else
        {
            miningStation = FindObjectOfType<CreditsMiningStation>();
        }
        upgradeManager = GetComponent<MiningStationUpgradeManager>();
        changePos = GetComponent<changePosition>();
        // fill in hash table
        for (int i = 1; i < miningStation.changeMinNeededEnergonAmount.Length+1; i++){
            if(i == 1){
                buttonEnabledHashMap.Add(i, false);
            }
            else{
                buttonEnabledHashMap.Add(i, true);
            }
        }
    
        // set text on slot buttons
        for (int i = 1; i < miningStation.changeMinNeededEnergonAmount.Length+1; i++){
            setButtonTextBack(i, miningStation.changeTimeNeedForMining[i-1]);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnEnable()
    {
        //Register Button Events
        mineSlot1btn.onClick.AddListener(() => buttonCallBack(mineSlot1btn));
        mineSlot2btn.onClick.AddListener(() => buttonCallBack(mineSlot2btn));
        mineSlot3btn.onClick.AddListener(() => buttonCallBack(mineSlot3btn));
        upgradeBtn.onClick.AddListener(() => buttonCallBack(upgradeBtn));
        changePosBtn.onClick.AddListener(() => buttonCallBack(changePosBtn));
    }


    private void buttonCallBack(Button buttonPressed)
    {
        if (buttonPressed == mineSlot1btn)
        {
            mineSlot1btn.interactable = false;
            miningStation.miningAction(1);
        }
        else if (buttonPressed == mineSlot2btn)
        {
            //Your code for button 2
            mineSlot2btn.interactable = false;
            miningStation.miningAction(2);
        }
        else if (buttonPressed == mineSlot3btn)
        {
            //Your code for button 3
            mineSlot3btn.interactable = false;
            miningStation.miningAction(3);
        }
        else if (buttonPressed == upgradeBtn){
            upgradeManager.upgradeAction();
        }
        else if (buttonPressed == changePosBtn){
            changePos.changePosAction();
        }
    }

    
    public void activateButton(int buttonID){
        switch (buttonID)
        {
            case 1:
                mineSlot1btn.interactable = true;
                break;
            case 2:
                mineSlot2btn.interactable = true;
                break;
            case 3: 
                mineSlot3btn.interactable = true;
                break;
            default:
                break;
        }
    }


    public void setButtonTextBack(int buttonID, float timeStart){
        switch (buttonID)
        {
            case 1:
                if (!(bool)buttonEnabledHashMap[buttonID]){
                    mineSlot1PriceText.text = miningStation.changeMinNeededEnergonAmount[buttonID-1] + " energon ";
                    if(timeStart >= 60.00f)
                    {
                        slot1TimeText.text = Mathf.Round(timeStart / 60f) % 60 + " min " + Mathf.Round(timeStart % 60f ).ToString() + " sec";
                    }
                    else{
                        slot1TimeText.text = Mathf.Round(timeStart % 60f ).ToString() + " sec";
                    }
                    mineSlot1RewardText.text = miningStation.changeRewards[buttonID-1] + " credits ";
                }
                break;
            case 2:
                if (!(bool)buttonEnabledHashMap[buttonID]){
                    mineSlot2btn.interactable = true;
                    slot2Image.gameObject.SetActive(true);
                    lockedSlot2.gameObject.SetActive(false);
                    Color btnColour = new Color32(70,132, 209, 255);
                    mineSlot2btn.GetComponent<UnityEngine.UI.Image>().color = btnColour;
                    mineSlot2PriceText.text = miningStation.changeMinNeededEnergonAmount[buttonID-1] + " energon ";
                    if(timeStart >= 60.00f)
                    {
                        slot2TimeText.text = Mathf.Round(timeStart / 60f) % 60 + " min " + Mathf.Round(timeStart % 60f ).ToString() + " sec";
                    }
                    else{
                        slot2TimeText.text = Mathf.Round(timeStart % 60f ).ToString() + " sec";
                    }
                    mineSlot2RewardText.text = miningStation.changeRewards[buttonID-1] + " credits ";
                }
                else {
                    //Debug.Log("Im here");
                    slot2Image.gameObject.SetActive(false);
                    lockedSlot2.gameObject.SetActive(true);
                    mineSlot2btn.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                    mineSlot2btn.interactable = false;
                }
                break;
            case 3: 
                if (!(bool)buttonEnabledHashMap[buttonID]){
                    mineSlot3btn.interactable = true;
                    slot3Image.gameObject.SetActive(true);
                    lockedSlot3.gameObject.SetActive(false);
                    Color btnColour = new Color32(70,132, 209, 255);
                    mineSlot3btn.GetComponent<UnityEngine.UI.Image>().color = btnColour;
                    mineSlot3PriceText.text = miningStation.changeMinNeededEnergonAmount[buttonID-1] + " energon ";
                    if(timeStart >= 60.00f)
                    {
                        slot3TimeText.text = Mathf.Round(timeStart / 60f) % 60 + " min " + Mathf.Round(timeStart % 60f ).ToString() + " sec";
                    }
                    else{
                        slot3TimeText.text = Mathf.Round(timeStart % 60f ).ToString() + " sec";
                    }
                    mineSlot3RewardText.text = miningStation.changeRewards[buttonID-1] + " credits ";
                }
                else{
                    slot3Image.gameObject.SetActive(false);
                    lockedSlot3.gameObject.SetActive(true);
                    mineSlot3btn.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                    mineSlot3btn.interactable = false;
                }
                break;
            default:
                break;
        }
    }


    public void setButtonTextForSlotCoolDown(int slotIndex){
        if (slotIndex == 1){
            mineSlot1PriceText.text = "Slot cool down :";
            mineSlot1RewardText.text = "";
        }
        else if (slotIndex == 2){
            mineSlot2PriceText.text = "Slot cool down :";
            mineSlot2RewardText.text = "";
        }
        else if (slotIndex == 3){
            mineSlot3PriceText.text = "Slot cool down :";
            mineSlot3RewardText.text = "";
        }
    }

    // unlock btn action
    public void unlockBtn(int buttonID){
        if (buttonEnabledHashMap.ContainsKey(buttonID)){
            buttonEnabledHashMap[buttonID] = false;
        }
        /*foreach (DictionaryEntry ele in buttonEnabledHashMap){ 
            Debug.Log("Modified values" + ele.Key +  "  "  + ele.Value);
            //Debug.Log("Value" + ele.Value);
        }*/
    }

}
