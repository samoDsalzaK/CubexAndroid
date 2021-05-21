using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSesionTime : MonoBehaviour
{
    [SerializeField] Text counterText;
    [SerializeField] float seconds;
    [SerializeField] float minutes;
    [SerializeField] float maxSeconds = 0;
    [SerializeField] float maxMinutes = 0;
    [SerializeField] bool isTimeFinished = false;
    //[SerializeField] float maxMinutes;
    [SerializeField] bool isTimeUnlimited = false;
    float startingTime;
    //float currentValue; // variable to store current level time
    float totalTimeToAdd = 0f;
    CubexWindowManager cubexWindowManager;
    public bool IsTimeFinished {set {isTimeFinished = value; } get {return isTimeFinished; }}
    // Update is called once per frame
    void Start(){
        // if (!isTimeUnlimited){
        //     startingTime = minutes * 60f + seconds;
        //     StartCoroutine(StartCountDown(startingTime));
        // }
    }
    void Update()
    {
        // for unlimited level time 
        if (!isTimeFinished)
        {
            seconds = (int)(Time.timeSinceLevelLoad % 60f);
            minutes = (int)(Time.timeSinceLevelLoad / 60f) % 60;
            counterText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        if (!isTimeUnlimited)
        {
            if (maxSeconds <= seconds && maxMinutes <= minutes)
            {
                isTimeFinished = true;
            }
        }
    }

    public IEnumerator StartCountDown(float startingTime)
    {
        while (startingTime > 0)
            {
                if(startingTime >= 60.00f)
                {
                    // change Text field with current time
                    counterText.text = ((int)startingTime / 60).ToString("00") + ":" + (startingTime % 60).ToString("00");
                }
                else{
                    counterText.color = Color.red;
                    counterText.text = (startingTime % 60).ToString("00");
                }
                yield return new WaitForSeconds(1.0f);
                startingTime--;
                if (totalTimeToAdd != 0f){ // check for this value, if not 0 then add seconds to level time
                    startingTime += totalTimeToAdd;
                    totalTimeToAdd = 0f;
                }
            }
        cubexWindowManager = GetComponent<CubexWindowManager>();
        cubexWindowManager.returnLevelIndex = 9;
        cubexWindowManager.LoadLevel();
        // add level compliation lines
        yield break;
    }
    public bool timeFinished()
    {
        return isTimeFinished;
    }
    public void addTime(float timeInMinutes){
        totalTimeToAdd = timeInMinutes * 60f; // convert minutes to seconds
    }
}
