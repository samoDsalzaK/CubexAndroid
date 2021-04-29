using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TaskTimer : MonoBehaviour
{
   
    //Initial variables for the timer countdown system
    [Header("Main Timer configuration parameters")]
    //Button variable, which will used in setting the current button's interactability state. The button
    // [SerializeField] Button startBtn;
    // [SerializeField] Button additionalWorker;
    // Base playerbase; 
    [SerializeField] bool startCountdown = false;
    [SerializeField] bool finishedTask = false;
    public bool FinishedTask{set {finishedTask = value;} get { return finishedTask; }}
    public bool StartCountdown {set {startCountdown = value;} get {return startCountdown;}}
    //[SerializeField] Text btnText;
    //[SerializeField] Text btnWorkersAmountText;
    //[SerializeField] BuildWorker buildRegWorker;
    [SerializeField] float timeStart = 5;
    public float TimeStart { get { return timeStart; }} 
    private float oldTime = 0;
	// Use this for initialization
	void Start () 
    {
        oldTime = timeStart;
    //   playerbase = GetComponent<Base>();
	}
	
	// Update is called once per frame
	void Update () {
        // btnWorkersAmountText.text = playerbase.getExistingworkersAmount() + "/" + playerbase.getMaxWorkerAmountInLevel();
        //Main timer countdown processs
        if (startCountdown)
        {
           // FinishedTask = false;
            //The system tryes to subract from current starting timer value...
            timeStart -= Time.deltaTime;
            //When it subracts, then the system rounds up the value when it will try to outputthe current time's values.
           // textBox.text = Mathf.Round(timeStart).ToString();
            //btnText.text = Mathf.Round(timeStart).ToString();
           // Debug.Log(Mathf.Round(timeStart).ToString());
            //Checks if the timeStart value reached 0, when true then the timer button becomes interactable, a message is printed, which indicates 
            //that the time coundown is done and resets the current timeStart variable's value and text. As well, it returns nothing, so it could cancel
            //the system for continuing any further.. 
            if (timeStart <= 0)
            {
                //resets the timeStart variable to value, which was at the start of the system.
    
                //Sets the current coundown signal the false, which would say that the system can not run and that it can run once the user clicked on the button.
                startCountdown = false;
                FinishedTask = true;
                timeStart = oldTime;
                //Sets the state of button by making it interactable again
                //startBtn.interactable = true;
                
                //additionalWorker.interactable = true;
                
                //playerbase.Spawning();
                  
               // btnText.text = "Create Worker \n"  + buildRegWorker.getMinNeededEnergonAmount() + " credits)";
                return;
            }
        }
	}
    public void startTimer(float t)
    {
        FinishedTask = false;
        timeStart = t;
        oldTime = timeStart;
        startCountdown = true;
        StartCountdown = startCountdown;
    }


}
