using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class BuildTask : System.Object
{
    [SerializeField] int credits = 0;
    [SerializeField] int energon = 0;
    [SerializeField] string taskName = "";
    [SerializeField] bool state;//FinishedOrNot
    [SerializeField] bool pending;
    public BuildTask(int credits, int energon, string taskName)
    {
        this.credits = credits;
        this.energon = energon;
        this.taskName = taskName;
        this.state = false;
        this.pending = false;
    }

    public int Credits {set {credits = value;} get  {return credits; }}
    public int Energon {set {energon = value; } get {return energon; }}
    public string TaskName {set {taskName = value;} get {return taskName; }}
    public bool State {set { state = value; } get { return state; }}
    public bool Pending {set { pending = value; } get { return pending; }}
}
public class TaskManager : MonoBehaviour
{    
    // Works in a FIFO way
    [Header("Configuration parameters")]
    [SerializeField] int maxTaskAmount = 3;//For testing
    [SerializeField] int nextTaskIndex = 0;
    [SerializeField] List<BuildTask> bTasks; //Serializing for testing
    [SerializeField] float waitForIncomingTaskDelay = 2f;
    [SerializeField] GameObject buildingCanvas;
    [SerializeField] Text displayTaskCountText;
    [SerializeField] GameObject emptyText; 
    [SerializeField] GameObject errorWindow;
    [SerializeField] Text errorMsgWindowText;
    [SerializeField] float cardSpacing = 1f;
    [Header("Testing data")]
    [SerializeField] bool isTesting = false;
    [SerializeField] int baseEnergon = 1000;
    [SerializeField] int baseCredits = 1000;    
    private UnitBuildTimer troopTimer;
    private Base playerBase;
    private bool stayEmpty;
    private GameObject taskWindow;
    private GameObject taskCard;
    private GameObject clonedTaskWindow;
    private float cardSpacingOffset = 0f;
    private Helper helperTools = new Helper();
    [SerializeField] List<GameObject> taskCards;
    
    void Start()
    {
        stayEmpty = false;
        bTasks = new List<BuildTask>();
        taskCards = new List<GameObject>();
        troopTimer = GetComponent<UnitBuildTimer>(); 
        playerBase = FindObjectOfType<Base>();      
        if (playerBase)
        {
            print("Building " + gameObject.name + " task manager ready!");
            
        }
         // GUI functionality
        taskWindow = helperTools.getChildGameObjectByName(buildingCanvas, "TasksWindow");
        taskCard = helperTools.getChildGameObjectByName(taskWindow, "TaskCard");
        if (taskCard)
            print("Task window is found");
        
    }

    // Update is called once per frame
    void Update()
    {
        displayTaskCount();
        
        if(waitingTasks())
        {
                emptyText.SetActive(false);
                var task = bTasks[0];
                if (task != null){
                    if (!task.State) //False - wating for execution / True - Done
                    {
                        switch(task.TaskName)
                        {
                            case "LightTroop":                  
                               
                                if (!task.Pending)
                                {
                                    if (playerBase && !isTesting) 
                                    {
                                        playerBase.setEnergonAmount(playerBase.getEnergonAmount() - task.Energon);
                                        playerBase.setCreditsAmount(playerBase.getCreditsAmount() - task.Credits);
                                        
                                    }
                                    else 
                                    {
                                        baseEnergon -= task.Energon;
                                        baseCredits -= task.Credits;
                                    }
                                    
                                    task.Pending = true;                              
                                    StartCoroutine(trainTimer(troopTimer, task)); //Delay and changing task state                            
                                }                               
                                
                                
                            
                            break;
                            case "HeavyTroop":                                 
                                if (!task.Pending)
                                {
                                        if (playerBase && !isTesting)
                                        {
                                            playerBase.setEnergonAmount(playerBase.getEnergonAmount() - task.Energon);
                                            playerBase.setCreditsAmount(playerBase.getCreditsAmount() - task.Credits);
                                        }
                                        else 
                                        {
                                            baseEnergon -= task.Energon;
                                            baseCredits -= task.Credits;
                                        }
                                    
                                        task.Pending = true;                              
                                        StartCoroutine(trainTimer(troopTimer, task)); //Delay and changing task state                            
                                }   

                            break;

                            case "SniperTroop":                                 
                                if (!task.Pending)
                                {
                                        if (playerBase && !isTesting)
                                        {
                                            playerBase.setEnergonAmount(playerBase.getEnergonAmount() - task.Energon);
                                            playerBase.setCreditsAmount(playerBase.getCreditsAmount() - task.Credits);
                                        }
                                        else 
                                        {
                                            baseEnergon -= task.Energon;
                                            baseCredits -= task.Credits;
                                        }
                                    
                                        task.Pending = true;                              
                                        StartCoroutine(trainTimer(troopTimer, task)); //Delay and changing task state                            
                                }   

                            break;
                        }
                }                
            }
        }
        else
        {
            emptyText.SetActive(true);
        }
    }
    IEnumerator trainTimer(UnitBuildTimer troopTimer, BuildTask task)
    {        
        var card = taskCards[0];
        var cardProgressText = helperTools.getChildGameObjectByName(card, "Progress");
        troopTimer.startTimer(task.TaskName, cardProgressText.GetComponent<Text>());
        while(troopTimer.StartCountdown)
        {
            yield return null; 
        }
        // Handle task finish
        if (troopTimer.FinishedTask)
        {
            task.State = troopTimer.FinishedTask;            
            cardSpacingOffset -= cardSpacing;  
            taskCards.Remove(card);
            Destroy(card);
            Debug.Log(troopTimer.FinishedTask ? "Task ("+bTasks.IndexOf(task)+") is done!" : "Task ("+bTasks.IndexOf(task)+") still is not done yet!");
            bTasks.Remove(task);
            if (taskCards.Count > 0)
            {
                foreach(var cardGUI in taskCards)
                {
                    helperTools.getChildGameObjectByName(cardGUI, "TaskIndex").GetComponent<Text>().text = (taskCards.IndexOf(cardGUI) + 1).ToString();
                    cardGUI.transform.position = new Vector3(cardGUI.transform.position.x, cardGUI.transform.position.y + cardSpacing, cardGUI.transform.position.z);
                }
            }
           
        }
        displayTaskCount();
    } 
  
    public void createTask(string name, int energonPrice, int creditsPrice)
    {
        // Add player m base code
        if (playerBase && !isTesting)
        {
            if ((energonPrice > playerBase.getEnergonAmount() || playerBase.getEnergonAmount() == 0) || (creditsPrice > playerBase.getCreditsAmount() || playerBase.getCreditsAmount() == 0))
            {
                // Debug.Log("ERROR! Insuficient funds");               
                errorWindow.SetActive(true);
                errorMsgWindowText.text = "ERROR! Insuficient funds";
                return;
            }
        }
        else{
            if (energonPrice > baseEnergon || creditsPrice > baseEnergon)
            {
                //Debug.Log("ERROR! Insuficient test funds");
                errorWindow.SetActive(true);
                errorMsgWindowText.text = "ERROR! Insuficient test funds";
                return;
            }
        }
        
        if (bTasks.Count >= maxTaskAmount)
        {
            //Debug.Log("ERROR! full stack of tasks. Please wait when additional room for task will be ready");
            errorWindow.SetActive(true);
            errorMsgWindowText.text = "ERROR! full stack of tasks. Please wait when additional room for task will be ready";
            return;
        }
        print("Doing " + name + " training task");
        stayEmpty = true;
        var existingTaskAmount = bTasks.Count;
        var newTask =  new BuildTask(
                                   energonPrice, 
                                   creditsPrice,
                                   name
                                );   
        bTasks.Add(newTask);
        // GUI Part - creating task card in task window
        clonedTaskWindow = Instantiate(taskCard, taskCard.transform.position, taskCard.transform.rotation, taskWindow.transform);
        clonedTaskWindow.SetActive(true);
        var cardPos = clonedTaskWindow.transform.position;
        clonedTaskWindow.transform.position = new Vector3(cardPos.x, cardPos.y - cardSpacingOffset, cardPos.z);
        cardSpacingOffset += cardSpacing;
        taskCards.Add(clonedTaskWindow);

        // Assining initial info 
        var taskIndex = helperTools.getChildGameObjectByName(clonedTaskWindow, "TaskIndex");
        var taskName = helperTools.getChildGameObjectByName(clonedTaskWindow, "Taskname");
        //var progressText = getChildGameObjectByName(clonedTaskWindow, "Progress");
    
        taskIndex.GetComponent<Text>().text = (bTasks.IndexOf(newTask) + 1).ToString();
        taskName.GetComponent<Text>().text = name;


        Debug.Log(existingTaskAmount < bTasks.Count ? "Task make " + name + " added!" : "ERROR: task make " + name + " not added!");
        //currentIndexAmount++;
        displayTaskCount();
    }
    private bool waitingTasks()
    {
        if (bTasks.Count == 0) return false;
        int amount = 0;
        foreach(var task in bTasks)
        {
            if (!task.State)
                amount++;
        }
        return amount > 0;
    }
  
    private void displayTaskCount()
    {
        displayTaskCountText.text = "View tasks(" + bTasks.Count + ")";
    }
}
