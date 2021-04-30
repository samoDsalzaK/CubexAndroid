using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitBuildTimer : MonoBehaviour {
    
    //Initial variables for the timer countdown system
    [Header ("Main configuration parameters")]

    //Starting time value, from where the timer will go to 0
    [SerializeField] float timeStart = 5f;

    //Button variable, which will used in setting the current button's interactability state. The button
    
    [SerializeField] TimedSpawn timedSpawn;
    //Printing a meessage, which indicates when the timer has started coundowning. When the timer starts then in the windows a message pops out, saying:
    //"Timer started". Once the coundown finishes, then it prints "Coundown is done!"
    
    //Cached references
    //Variable for checking when the timer should start and when it can stop
    [SerializeField] bool startCountdown = false;
    [SerializeField] bool finishedTask = false;
    [SerializeField] string unitName = "";
    public string UnitName { set {unitName = value; } get { return unitName; }}
    public bool FinishedTask{set {finishedTask = value;} get { return finishedTask; }}
    public bool StartCountdown {set {startCountdown = value;} get {return startCountdown;}}
    //Button text component for outputting what is the current time on the button...
    [SerializeField] Text textBox;
    private float oldTime = 0;
    [SerializeField] Text taskProgress;
    

    //Variable for storing the current startTime value, from where it tryed calculating time. Once the current time reached 0, then
    //this value's variable will be used for resetting the current time.
    //string originalText;
    // Update is called once per frame
    void Start() {
        oldTime = timeStart;
        //textBox.text = "Train troop (" + timedSpawn.getLightTroopUnitCost() + " credits)";
    }
    void Update () {

        //Main timer countdown processs
        if (startCountdown) {
            //The system tryes to subract from current starting timer value...
            timeStart -= Time.deltaTime;
            //When it subracts, then the system rounds up the value when it will try to outputthe current time's values.
            //textBox.text = Mathf.Round (timeStart).ToString ();
            taskProgress.text = "Progress: " + Mathf.Round (timeStart).ToString();
            //Checks if the timeStart value reached 0, when true then the timer button becomes interactable, a message is printed, which indicates 
            //that the time coundown is done and resets the current timeStart variable's value and text. As well, it returns nothing, so it could cancel
            //the system for continuing any further.. 
            if (timeStart <= 0) {
                //resets the timeStart variable to value, which was at the start of the system.
                //timeStart = startingTime;

                //Sets the current coundown signal the false, which would say that the system can not run and that it can run once the user clicked on the button.
                //startCountdown = false;

                //Sets the state of button by making it interactable again
               // startBtn.interactable = true;
                startCountdown = false;
                FinishedTask = true;
                timeStart = oldTime;
                //Resets the current button's text value, which can be defined in the inspector by using this class string variable nameBtn.
                //textBox.text = "Train troop (" + timedSpawn.getLightTroopUnitCost() + " credits)"; 
                switch(UnitName)
                {
                    case "LightTroop" :
                        timedSpawn.spawnObject();
                    break;
                    case "HeavyTroop" :
                        timedSpawn.spawnHeavyObject();
                    break;
                    case "SniperTroop" :
                        timedSpawn.spawnSniperObject();
                    break;
                    case "bloom":
                        timedSpawn.spawnBloomObject();
                    break;
                }
                
                 // textBox.text = originalText;
                return;
            }
        }
    }
    //public function, which will be used for the timer button. It is used for starting the current timer.
    public void startTimer (string tname, Text gText) {
        //print("Training timer length: " + timeStart);
        //timeStart=startTime;
        //Gives the signal to start the coundown timer by setting the current startCountdown value to true.
        UnitName = tname;
        startCountdown = true;
        taskProgress = gText;
        //Makes the current timer button to be not interactable
        //startBtn.interactable = false;
        //Prints a message using the text object to indicate that the timer is counting..
    }

}