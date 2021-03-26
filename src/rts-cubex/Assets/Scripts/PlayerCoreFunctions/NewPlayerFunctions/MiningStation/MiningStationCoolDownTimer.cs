using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningStationCoolDownTimer : MonoBehaviour
{
    [Header("Upgrade Manager configuration parameters")]
    buttonHadler button_Hadler; // button handler type var
    CreditsMiningStation miningStation; // creating Credits mining station variable
    [SerializeField] Text slot1TimeText;
    [SerializeField] Text slot2TimeText;
    [SerializeField] Text slot3TimeText;
    float currCountdownValueSlot1;
    float currCountdownValueSlot2;
    float currCountdownValueSlot3;

    // Start is called before the first frame update
    void Start()
    {
        button_Hadler = GetComponent<buttonHadler>();
        miningStation = GetComponent<CreditsMiningStation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // function for slot cool down
    public IEnumerator coolDownSlot1(int slotNumber, float timeStart)
    {
        //int x = currCountdownValueSlot;
        currCountdownValueSlot1 = miningStation.changeTimeNeedForSlotCoolDown[slotNumber-1];
        while (currCountdownValueSlot1 > 0)
            {
                if(currCountdownValueSlot1 >= 60.00f)
                {
                    // here will be slot time text 
                    slot1TimeText.text = Mathf.Round(currCountdownValueSlot1 / 60f) % 60 + " min " + Mathf.Round(currCountdownValueSlot1 % 60f ).ToString() + " sec";
                }
                else{
                    slot1TimeText.text = Mathf.Round(currCountdownValueSlot1 % 60f ).ToString() + " sec";
                }
                Debug.Log("Countdown: " + currCountdownValueSlot1);
                yield return new WaitForSeconds(1.0f);
                currCountdownValueSlot1--;
            }
            button_Hadler.setButtonTextBack(slotNumber, timeStart);
            button_Hadler.activateButton(slotNumber); 
        yield break; 
    }

    public IEnumerator coolDownSlot2(int slotNumber, float timeStart)
    {
        //int x = currCountdownValueSlot;
        currCountdownValueSlot2 = miningStation.changeTimeNeedForSlotCoolDown[slotNumber-1];
        while (currCountdownValueSlot2 > 0)
            {
                if(currCountdownValueSlot2 >= 60.00f)
                {
                    // here will be slot time text 
                    slot2TimeText.text = Mathf.Round(currCountdownValueSlot2 / 60f) % 60 + " min " + Mathf.Round(currCountdownValueSlot2 % 60f ).ToString() + " sec";
                }
                else{
                    slot2TimeText.text = Mathf.Round(currCountdownValueSlot2 % 60f ).ToString() + " sec";
                }
                Debug.Log("Countdown: " + currCountdownValueSlot2);
                yield return new WaitForSeconds(1.0f);
                currCountdownValueSlot2--;
            }
            button_Hadler.setButtonTextBack(slotNumber, timeStart);
            button_Hadler.activateButton(slotNumber); 
        yield break; 
    }

    public IEnumerator coolDownSlot3(int slotNumber, float timeStart)
    {
        //int x = currCountdownValueSlot;
        currCountdownValueSlot3 = miningStation.changeTimeNeedForSlotCoolDown[slotNumber-1];
        while (currCountdownValueSlot3 > 0)
            {
                if(currCountdownValueSlot3 >= 60.00f)
                {
                    // here will be slot time text 
                    slot3TimeText.text = Mathf.Round(currCountdownValueSlot3 / 60f) % 60 + " min " + Mathf.Round(currCountdownValueSlot3 % 60f ).ToString() + " sec";
                }
                else{
                    slot3TimeText.text = Mathf.Round(currCountdownValueSlot3 % 60f ).ToString() + " sec";
                }
                Debug.Log("Countdown: " + currCountdownValueSlot3);
                yield return new WaitForSeconds(1.0f);
                currCountdownValueSlot3--;
            }
            button_Hadler.setButtonTextBack(slotNumber, timeStart);
            button_Hadler.activateButton(slotNumber); 
        yield break; 
    }

    public void startCoolDown(int slotNumber, float timeStart){
        if(slotNumber == 1){
            button_Hadler.setButtonTextForSlotCoolDown(slotNumber);
            StartCoroutine(coolDownSlot1(slotNumber, timeStart));
        }
        else if (slotNumber == 2){
            button_Hadler.setButtonTextForSlotCoolDown(slotNumber);
            StartCoroutine(coolDownSlot2(slotNumber, timeStart));
        }
        else if (slotNumber == 3){
            button_Hadler.setButtonTextForSlotCoolDown(slotNumber);
            StartCoroutine(coolDownSlot3(slotNumber, timeStart));
        }
    }
}
