using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerForMining : MonoBehaviour
{

    // temporarily store time to mine
    float currCountdownValueSlot1;
    float currCountdownValueSlot2;
    float currCountdownValueSlot3;

    buttonHadler button_Hadler;

    [Header("Main timer configuration parameters")]

    // text fields on slot buttons
    [SerializeField] Text slot1TimeText;

    [SerializeField] Text slot2TimeText;

    [SerializeField] Text slot3TimeText;

    CreditsMiningStation miningStation; // creating Credits mining station variable

    MiningStationCoolDownTimer coolDownTimer;


    // Start is called before the first frame update
    void Start()
    {
        button_Hadler = GetComponent<buttonHadler>();
        miningStation = GetComponent<CreditsMiningStation>();
        coolDownTimer = GetComponent<MiningStationCoolDownTimer>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    // function for mining action timer slot 1
    public IEnumerator StartCountdownSlot1(float timeStart, int slotNumber)
    {
        currCountdownValueSlot1 = timeStart;
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
        miningStation.changeMinedCreditsAmount = miningStation.changeMinedCreditsAmount + miningStation.changeRewards[slotNumber-1]; // set new value after mining action  
        coolDownTimer.startCoolDown(slotNumber, timeStart); 
        yield break;
    }
    
    // function for mining action timer slot 2
    public IEnumerator StartCountdownSlot2(float timeStart, int slotNumber)
    {
        currCountdownValueSlot2 = timeStart;
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
        miningStation.changeMinedCreditsAmount = miningStation.changeMinedCreditsAmount + miningStation.changeRewards[slotNumber-1]; // set new value after mining action
        coolDownTimer.startCoolDown(slotNumber, timeStart); 
        yield break;
    }
    
    // function for mining action timer slot 3
    public IEnumerator StartCountdownSlot3(float timeStart, int slotNumber)
    {
        currCountdownValueSlot3 = timeStart;
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
        miningStation.changeMinedCreditsAmount = miningStation.changeMinedCreditsAmount + miningStation.changeRewards[slotNumber-1]; // set new value after mining action
        coolDownTimer.startCoolDown(slotNumber, timeStart); 
        yield break;
    }

    public void startTimer(float miningStartTime, int slotNumber) // sita funkcija paims laika kiek turi uztrukti minimo procesas
    {       
        if(slotNumber == 1){
            StartCoroutine(StartCountdownSlot1(miningStartTime, slotNumber));
        }
        else if (slotNumber == 2){
            StartCoroutine(StartCountdownSlot2(miningStartTime, slotNumber));
        }
        else if (slotNumber == 3){
            StartCoroutine(StartCountdownSlot3(miningStartTime, slotNumber));
        }
    }
}
