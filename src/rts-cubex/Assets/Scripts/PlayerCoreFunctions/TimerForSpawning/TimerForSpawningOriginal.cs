using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerForSpawningOriginal : MonoBehaviour
{ 
    // 0 - barako atskaitos laikas;
    // 1 - kolektoriaus atskaitos laikas;
    // 2 - turret atskaitos laikas; 
    // 3 - research centro ataskaitos laikas;
    // 4 - troop research center;
    // 5 - walls;
    // 6 - credits mining station;
	// 7 - army camp
	// 8 - hero shrine
     [Header("Main Timer configuration parameters")]
    [SerializeField] float[] timereferencetasks; // masyvas skirtas saugoti timerio laika skirtingiems buildingams.
	//[SerializeField] float[] timereferencetasksForChangePos; // array used to store time needed to build building afer position change
    bool startCountdown = false;
    [SerializeField] GameObject barrack; // cia ikelsime pacius buildingu objektus
    [SerializeField] GameObject turret;
    [SerializeField] GameObject collector;
    [SerializeField] GameObject researchCenter;
    [SerializeField] GameObject troopsResearchCenter;
    //[SerializeField] GameObject playerDefenceWall;
    [SerializeField] GameObject creditsMiningStation;
	[SerializeField] GameObject armyCamp;
	[SerializeField] GameObject shrine;
    float startingTime = 0;
    //float startingTime1 = 0;
    float fillingvariable;
    //string buildingType;
    int timeArrayPozition;
    Vector3 pozition;
	//Vector3 pozitionFinal;
    private Base playerbase;
    [SerializeField] Text timeLeft;

    [SerializeField] int playerEarnedPoints = 10;
   
    [SerializeField] Image buildingBar; // building bar
   
    private Worker playerworker;
    private WorkerLifeCycle workerLife;
    [SerializeField] GameObject buildingbar;

	int pressedBtnIndex = 0;

	public float[] changeBuildingTimers
    {
        get
        {
            return timereferencetasks;
        }
        set
        {
            timereferencetasks = value;
        }
    }


    void Start()
    {
        if(FindObjectOfType<Base>() == null)
        {
         return;
        }
        else
        {
         	playerbase = FindObjectOfType<Base>();
        }
        workerLife = GetComponent<WorkerLifeCycle>();
        playerworker = GetComponent<Worker>();
        buildingbar.SetActive(false);
        fillingvariable = 0;
    }

    void Update()
    {
       //Debug.Log(Time.deltaTime);
        
        if (startCountdown)
        {          
            buildingbar.SetActive(true);
            fillingvariable += Time.deltaTime;
            buildingBar.fillAmount = fillingvariable / timereferencetasks[timeArrayPozition];
            startingTime -= Time.deltaTime;
			if(startingTime >= 60.00f)
			{
				timeLeft.text = ((int)(startingTime / 60)).ToString() + " min " + ((int)(startingTime % 60)).ToString() + " sec";
			}
			else
			{
				timeLeft.text = ((int)(startingTime % 60)).ToString() + " sec";
			}
            if (startingTime <= 0)
            {
                //resets the timeStart variable to value, which was at the start of the system.
                //startingTime = originalTime;
                
                //Sets the current coundown signal the false, which would say that the system can not run and that it can run once the user clicked on the button.
                startCountdown = false;
                buildingbar.SetActive(false);
                fillingvariable = 0;
                //Sets the state of button by making it interactable again
                //startBtn.interactable = true;
                //Resets the current button's text value, which can be defined in the inspector by using this class string variable nameBtn.
                //var playerworker = FindObjectsOfType<Worker>();
                switch(/*buildingType*/timeArrayPozition)
                {
					case 0/*"barrack"*/:
						if(pressedBtnIndex != 0){
							var changePosBuildings = FindObjectsOfType<changePosition>();
							if(changePosBuildings != null){
								for (int i = 0; i < changePosBuildings.Length; i++){
									if((changePosBuildings[i].returnBtnIndex == pressedBtnIndex) && (changePosBuildings[i].isInChangeMode)){
										changePosBuildings[i].destroyGameObject();
										Instantiate(barrack, pozition, Quaternion.identity);
										FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
										workerLife.decreaseWorkerLife();
										playerworker.setWorkerState(false); // statas reiskia kad workeris yra laisvas
										return;
									}
								}
							}
						}
						else{
							Instantiate(barrack, pozition, Quaternion.identity);
							playerbase.GetComponent<PlayerScoring>().addScoreAfterStructureBuild("barrack");
							FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount()+1);
							playerworker.setWorkerState(false);
							workerLife.decreaseWorkerLife();
						}
					break;
					case 3/*"researchCenter"*/ :
						if(pressedBtnIndex != 0){
							var changePosBuildings = FindObjectsOfType<changePosition>();
							if(changePosBuildings != null){
								for (int i = 0; i < changePosBuildings.Length; i++){
									if((changePosBuildings[i].returnBtnIndex == pressedBtnIndex) && (changePosBuildings[i].isInChangeMode)){
										changePosBuildings[i].destroyGameObject();
										Instantiate(researchCenter, pozition, Quaternion.identity);
										FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
										workerLife.decreaseWorkerLife();
										playerworker.setWorkerState(false); // statas reiskia kas workeris yra laisvas
										return;
									}
								}
							}
						}
						else{
							Instantiate(researchCenter, pozition, Quaternion.identity);
							playerbase.GetComponent<PlayerScoring>().addScoreAfterStructureBuild("buildingResearch");
							FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
							playerworker.setWorkerState(false);
							workerLife.decreaseWorkerLife();
						}
					break;
					case 2/*"turret"*/:
						if(pressedBtnIndex != 0){
							var changePosBuildings = FindObjectsOfType<changePosition>();
							if(changePosBuildings != null){
								for (int i = 0; i < changePosBuildings.Length; i++){
									if((changePosBuildings[i].returnBtnIndex == pressedBtnIndex) && (changePosBuildings[i].isInChangeMode)){
										changePosBuildings[i].destroyGameObject();
										Instantiate(turret, pozition, Quaternion.identity);
										FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
										workerLife.decreaseWorkerLife();
										playerworker.setWorkerState(false); // statas reiskia kas workeris yra laisvas
										return;
									}
								}
							}
						}
						else{
							Instantiate(turret, pozition, Quaternion.identity);
							playerbase.GetComponent<PlayerScoring>().addScoreAfterStructureBuild("turret");
							FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount()+1);
							playerworker.setWorkerState(false);
							workerLife.decreaseWorkerLife();
						}
					break;
					case 4/*"troopsReserach"*/:
						if(pressedBtnIndex != 0){
							var changePosBuildings = FindObjectsOfType<changePosition>();
							if(changePosBuildings != null){
								for (int i = 0; i < changePosBuildings.Length; i++){
									if((changePosBuildings[i].returnBtnIndex == pressedBtnIndex) && (changePosBuildings[i].isInChangeMode)){
										changePosBuildings[i].destroyGameObject();
										Instantiate(troopsResearchCenter, new Vector3(pozition.x, transform.position.y ,pozition.z), Quaternion.identity);
										FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
										workerLife.decreaseWorkerLife();
										playerworker.setWorkerState(false); // statas reiskia kas workeris yra laisvas
										return;
									}
								}
							}
						}
						else{
							Instantiate(troopsResearchCenter, new Vector3(pozition.x, transform.position.y ,pozition.z), Quaternion.identity);
							playerbase.GetComponent<PlayerScoring>().addScoreAfterStructureBuild("troopsResearch");
							FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
							workerLife.decreaseWorkerLife();
							playerworker.setWorkerState(false); // statas reiskia kas workeris yra laisvas
						}
					break;
					case 1/*"Depo"*/:
						Debug.Log("Collector created");
						Instantiate(collector, pozition, Quaternion.identity);
						playerbase.GetComponent<PlayerScoring>().addScoreAfterStructureBuild("energonCollector");
						workerLife.decreaseWorkerLife();
						//playerworker.setWorkerState(false);
					break;
					/*case 5:
						Instantiate(playerDefenceWall, pozition, Quaternion.identity);
						if(playerScorePoints != null)
						{
							playerScorePoints.AddPlayerScorePoints(playerEarnedPoints);
							playerScorePoints.addPlayerWallsAmount(1);
						}
						FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
						workerLife.decreaseWorkerLife();
						playerworker.setWorkerState(false); // statas reiskia kas workeris yra laisvas
					break;*/
					case 6/*credits mining station*/:
						if(pressedBtnIndex != 0){
							var changePosBuildings = FindObjectsOfType<changePosition>();
							if(changePosBuildings != null){
								for (int i = 0; i < changePosBuildings.Length; i++){
									if((changePosBuildings[i].returnBtnIndex == pressedBtnIndex) && (changePosBuildings[i].isInChangeMode)){
										//Debug.Log("Im here");
										//Debug.Log(changePosBuildings[i].canChange);
										changePosBuildings[i].destroyGameObject();
										Instantiate(creditsMiningStation, pozition, Quaternion.identity);
										FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
										workerLife.decreaseWorkerLife();
										playerworker.setWorkerState(false); // reiskia kas workeris yra laisvas
										return;
									}
								}
							}
						}
						else{
							Instantiate(creditsMiningStation, pozition, Quaternion.identity);
							playerbase.GetComponent<PlayerScoring>().addScoreAfterStructureBuild("creditsMiningStation");
							FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
							workerLife.decreaseWorkerLife();
							playerworker.setWorkerState(false); // statas reiskia kas workeris yra laisvas
						}
					break;
					case 7: /*army camp*/
						if(pressedBtnIndex != 0){
							var changePosBuildings = FindObjectsOfType<changePosition>();
							if(changePosBuildings != null){
								for (int i = 0; i < changePosBuildings.Length; i++){
									if((changePosBuildings[i].returnBtnIndex == pressedBtnIndex) && (changePosBuildings[i].isInChangeMode)){
										changePosBuildings[i].destroyGameObject();
										Instantiate(armyCamp, pozition, Quaternion.identity);
										FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
										workerLife.decreaseWorkerLife();
										playerworker.setWorkerState(false); // reiskia kas workeris yra laisvas
										return;
									}
								}
							}
						}
						else{
							Instantiate(armyCamp, pozition, Quaternion.identity);
							playerbase.GetComponent<PlayerScoring>().addScoreAfterStructureBuild("armyCamp");
							FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
							workerLife.decreaseWorkerLife();
							playerworker.setWorkerState(false); // statas reiskia kas workeris yra laisvas
						}
					break;
					case 8: /*shrine*/
						if(pressedBtnIndex != 0){
							var changePosBuildings = FindObjectsOfType<changePosition>();
							if(changePosBuildings != null){
								for (int i = 0; i < changePosBuildings.Length; i++){
									if((changePosBuildings[i].returnBtnIndex == pressedBtnIndex) && (changePosBuildings[i].isInChangeMode)){
										changePosBuildings[i].destroyGameObject();
										Instantiate(shrine, pozition, Quaternion.identity);
										FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
										workerLife.decreaseWorkerLife();
										playerworker.setWorkerState(false); // reiskia kas workeris yra laisvas
										return;
									}
								}
							}
						}
						else{
							Instantiate(shrine, pozition, Quaternion.identity);
							playerbase.GetComponent<PlayerScoring>().addScoreAfterStructureBuild("shrine");
							FindObjectOfType<Base>().setworkersAmount(FindObjectOfType<Base>().getworkersAmount() + 1);
							workerLife.decreaseWorkerLife();
							playerworker.setWorkerState(false); // statas reiskia kas workeris yra laisvas
						}
					break;
                }
                //Once the timer has finished counting, a message appear swhich says that the timer has finished counting
                //displayText.text = "Hurray! it is done!";
                //Then when this process has done counting, it return nothing, at system restarts.
                return;
            }
        }
    }
    public void startTimer(/*string order,*/ Vector3 poz, int arrayPosition, int pressedButtonIndex)
    {
		if(pressedButtonIndex != 0){
			pressedBtnIndex = pressedButtonIndex;
		}
        startCountdown = true; 
        //buildingType = order;
        pozition = poz;
		//Vector3 pozitionFinal = new Vector3(poz.x + 10f, poz.y, poz.z);
		//Debug.Log("Koordinates 1  : " + poz.x + "" + poz.y + "" + poz.z);
		//Debug.Log("Koordinates 2  : " + pozitionFinal.x + "" + pozitionFinal.y + "" + pozitionFinal.z);
        timeArrayPozition = arrayPosition;
        switch(timeArrayPozition)
        {
			case 0:
				startingTime = timereferencetasks[timeArrayPozition];
			break;
			case 1:
				startingTime = timereferencetasks[timeArrayPozition];
			break;
			case 2:
				startingTime = timereferencetasks[timeArrayPozition];
			break;
			case 3:
				startingTime = timereferencetasks[timeArrayPozition];
			break;
			case 4:
				startingTime = timereferencetasks[timeArrayPozition]; 
			break;
			case 5:
				startingTime = timereferencetasks[timeArrayPozition]; 
			break;
			case 6:
				startingTime = timereferencetasks[timeArrayPozition];
			break;
			case 7:
				startingTime = timereferencetasks[timeArrayPozition];
			break;
			case 8:
				startingTime = timereferencetasks[timeArrayPozition];
			break;
        }
    }
} 

