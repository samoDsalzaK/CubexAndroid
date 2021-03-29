using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSesionTime : MonoBehaviour
{
    [SerializeField] Text counterText;
    [SerializeField] float seconds;
    [SerializeField] float minutes;
    [SerializeField] bool isTimeFinished = false;
    //[SerializeField] float maxMinutes;
    [SerializeField] bool isTimeUnlimited = false;
    float startingTime;
    // Update is called once per frame
    void Start(){
        if (!isTimeUnlimited){
            startingTime = minutes * 60f + seconds;
            StartCoroutine(StartCountDown());
        }
    }
    void Update()
    {
        // for unlimited level time 
        if (isTimeUnlimited)
        {
            seconds = (int)(Time.timeSinceLevelLoad % 60f);
            minutes = (int)(Time.timeSinceLevelLoad / 60f) % 60;
            counterText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    public IEnumerator StartCountDown()
    {
        while (startingTime > 0)
            {
                if(startingTime >= 60.00f)
                {
                    // change Text field with current time
                    counterText.text = ((int)startingTime / 60).ToString("00") + ":" + (startingTime % 60).ToString("00");
                }
                else{
                    counterText.text = (startingTime % 60).ToString("00");
                }
                yield return new WaitForSeconds(1.0f);
                startingTime--;
            }
        // add level compliation lines
        yield break;
    }
    public bool timeFinished()
    {
        return isTimeFinished;
    }
}
