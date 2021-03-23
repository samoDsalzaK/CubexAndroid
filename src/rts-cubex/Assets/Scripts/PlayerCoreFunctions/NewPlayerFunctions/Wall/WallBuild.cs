using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallBuild : MonoBehaviour
{
    // -----------------
    //Add methods
    // NOTE: FIX wall build bugs
    //  Fix UI error
    //NOTE: FIX wall spacing
    [Header("Wall build cnf parameters")]
    [SerializeField] GameObject wallHolo;
    [SerializeField] Material builtWallMaterial;
    [SerializeField] GameObject buildingArea;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] Button wallBuildBtn;
    [SerializeField] GameObject playerBaseModel;
    [SerializeField] GameObject errorWindow;
    [SerializeField] Text errorMessage;
    private GameObject spawnedWallHolo;
    Ray ray;
    RaycastHit hit;
    private Vector3 oldWallIconPos;
    private bool enableDrag;
    private bool rotationClicked = false;
    private float spaceWallX = 0f;
    private float spaceWallZ = 0f;
    private GameObject[] staticWalls;
    private bool buildWalls = false;
    private Base playerBase;
    [Header("Test data")]
    [SerializeField] bool isTesting;
    [SerializeField] int energon = 1000;
    [SerializeField] int credits = 1000;
    private int energonPriceAmount = 0;
    private int creditsPriceAmount = 0;   
    private int energonPriceOffset = 0;
    private int creditsPriceOffset = 0;
    //private int resourcePriceAmount = 0;    
    //private float oldXpos;
    private int priceWallDivider = 0;
    private bool startWallBuild;
    private GameObject startingShiftWall;
    private float startingPosX = 0f;
    private float startingPosZ = 0f;
    private void Start() {
        playerBase = GetComponent<Base>();
        startWallBuild = false;
        buildingArea.SetActive(false);
        errorWindow.SetActive(false);        
        if (spawnedWallHolo == null)
        {           
            var startingPos =  playerBaseModel.transform.position;

            // if (playerBaseModel)
            //     startingPos = playerBaseModel.transform.position;
            // else
            //     print("No base model position available! Using testing starting position");
            // print(startingPos);
            if (playerBaseModel)
            {
                spawnedWallHolo = Instantiate(wallHolo, startingPos, Quaternion.identity, playerBaseModel.transform); 
            }
            else
            {
                spawnedWallHolo = Instantiate(wallHolo, startingPos, Quaternion.identity); 
            }
            var wallModel = getChildGameObjectByName(spawnedWallHolo, "Wall_1");
            var wallChecker = wallModel.GetComponent<WallBuildCheck>(); 
            wallModel.GetComponent<Collider>().isTrigger = true;           
            var wallRend = wallModel.GetComponent<MeshRenderer>();
            wallRend.material.color = new Color(wallRend.material.color.r, wallRend.material.color.g, wallRend.material.color.b, 0.25f);
            startWallBuild = false;
            energonPriceOffset = wallChecker.EnergonPrice;
            creditsPriceOffset =  wallChecker.CreditsPrice;
            spawnedWallHolo.SetActive(false);
        }
        else
        {
             Debug.LogError("Error not wall icon added to component!");
        }
    }
    public void openWallBuild()
    {
        wallBuildBtn.interactable = false;
        startWallBuild = true;
        buildingArea.SetActive(true);
        errorWindow.SetActive(false);
        if (spawnedWallHolo)
            spawnedWallHolo.SetActive(true);
        else
            Debug.LogError("Error not wall icon added to component!");        
    }

    // Update is called once per frame
    void Update()
    {
        if (!startWallBuild)
        {
           return;
        }
        else{
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                startWallBuild = false;  
                wallBuildBtn.interactable = true;   
                buildingArea.SetActive(false);  
                spawnedWallHolo.SetActive(false);
            }
        
            // Handle shift key build walls
            if (buildWalls)
            {
                energonPriceAmount -= energonPriceOffset;
                creditsPriceAmount -= creditsPriceOffset;
                if (!isTesting && (energonPriceAmount > playerBase.getEnergonAmount() || creditsPriceAmount > playerBase.getCreditsAmount()))
                {
                    handleShiftFundError();
                    return;
                }
                if (isTesting && (energonPriceAmount > energon || creditsPriceAmount > credits))
                {
                    handleShiftFundError();
                    return;
                }
                //print("ePA=" + energonPriceAmount + ", cPA=" + creditsPriceAmount);
                if (staticWalls.Length > 0)
                {       
                        if(isTesting)
                        {        
                            energon -= energonPriceAmount;
                            credits -= creditsPriceAmount;
                        }
                        else
                        {
                            playerBase.setEnergonAmount(playerBase.getEnergonAmount() - energonPriceAmount);
                            playerBase.setCreditsAmount(playerBase.getCreditsAmount() - creditsPriceAmount);
                        }
                        foreach (var wall in staticWalls)
                        {
                            var wallModel = getChildGameObjectByName(wall, "Wall_1");
                            if (wallModel)
                            {
                                var wallChecker = wallModel.GetComponent<WallBuildCheck>();
                                if (!wallChecker.SpaceOccupied)
                                {
                                // var wallMat = wall.GetComponent<MeshRenderer>().material;                           
                                    wallModel.GetComponent<MeshRenderer>().material = builtWallMaterial;
                                    wallModel.GetComponent<Collider>().isTrigger = false;  
                                    wallModel.layer = LayerMask.NameToLayer("PlayerBase");                      
                                    wall.tag = "PlayerWall"; 
                                    wallModel.tag = "PlayerWall";                     
                                    wallChecker.IsBuilt = true;
                                    //wallChecker.OriginalColor = new Color(wallMat.color.r, wallMat.color.g, wallMat.color.b, wallMat.color.a * 4);
                                    //wallMat.color = wallChecker.OriginalColor;
                                    
                                }
                            }
                        }
                        startingShiftWall = null;
                        startingPosX = 0f;
                        startingPosZ = 0f;
                        energonPriceAmount = 0;
                        creditsPriceAmount = 0;

                        resetShiftWalls();
                        //staticWalls = new GameObject[0];

                        buildWalls = false;
                        wallBuildBtn.interactable = true;
                        spawnedWallHolo.SetActive(false);
                        buildingArea.SetActive(false);
                        startWallBuild = false;  
                        // Decrease energon and credits amount
                }
                else
                {
                    print("No placed walls in the building area");                 
                    return;
                }
            }
            //NOTE: Add math clamp for locking the cube in the area

            var mousePosition = Input.mousePosition;
            //print(mousePosition);
            ray = Camera.main.ScreenPointToRay(mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // var zPos = (buildingArea.transform.localScale.z / 2) * 10;
                // var xPos = (buildingArea.transform.localScale.x / 2) * 10;            
                
                //spawnedWallHolo.transform.position = new Vector3(Mathf.Clamp(hit.point.x, -xPos, xPos), 0.5f, Mathf.Clamp(hit.point.z, -zPos, zPos));
                
                if (!enableDrag)
                {
                   // print("Drag state: " + (spawnedWallHolo && (hit.transform.tag == "BuildArea" || hit.transform.tag == "HoloTools")));
                    if (spawnedWallHolo && (hit.transform.tag == "BuildArea" || hit.transform.tag == "HoloTools"))
                    {
                        // spawnedWallHolo.transform.position = new Vector3(Mathf.Clamp(hit.point.x, -xPos, xPos), 0.5f, Mathf.Clamp(hit.point.z, -zPos, zPos));
                        spawnedWallHolo.transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
                        oldWallIconPos = spawnedWallHolo.transform.position;
                    }
                }
                else
                {                    
                    //dragWallSpace = spawnedWallHolo.transform.localScale.z;
                    if (rotationClicked)
                    {
                    spawnedWallHolo.transform.position = new Vector3(oldWallIconPos.x, 0.5f, hit.point.z);
                    // spaceWallZ = dragWallSpace;
                    // spaceWallX = 0f;
                    }
                    else 
                    {      
                    // spaceWallX = -dragWallSpace; 
                    // spaceWallZ = 0f;      
                    spawnedWallHolo.transform.position = new Vector3(hit.point.x, 0.5f, oldWallIconPos.z);
                    }
                }
                
                
                // NOTE: Add holographic build drag with merge walls, do not use while loop!!
                if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    enableDrag = true;
                    //print("Drag enabled");
                    //Add logic to drag while holding shift
                }
                if (enableDrag)
                {
                        //New code
                        staticWalls = GameObject.FindGameObjectsWithTag("WallHolo"); //Taking all the spawned temp walls
                        var wallChecker = getChildGameObjectByName(spawnedWallHolo, "Wall_1").GetComponent<WallBuildCheck>();
                        if (!wallChecker.SpaceOccupied)
                        {
                            var spawnedWall = Instantiate(spawnedWallHolo, spawnedWallHolo.transform.position, spawnedWallHolo.transform.rotation);
                            var _spawnedWall = getChildGameObjectByName(spawnedWall, "Wall_1");
                            _spawnedWall.GetComponent<WallBuildCheck>().IsTemp = true;
                            _spawnedWall.GetComponent<Collider>().isTrigger = false;
                            spawnedWall.tag = "WallHolo";
                            energonPriceAmount += wallChecker.EnergonPrice;
                            creditsPriceAmount += wallChecker.CreditsPrice; 
                            // 
                        }              
                }

                    if (Input.GetKeyUp(KeyCode.LeftShift))
                    {
                      startingShiftWall = null;
                      startingPosX = 0f;
                      startingPosZ = 0f;
                      resetShiftWalls(); 
                    // spawnedWallHolo.transform.position = new Vector3(Mathf.Clamp(hit.point.x, -xPos, xPos), 0.5f, Mathf.Clamp(hit.point.z, -zPos, zPos));
                    }
                } 
                if (Input.GetMouseButtonDown(0) && enableDrag)  
                {
                    // Building all the shift walls holos  
                    buildWalls = true;          
                

                }      
                if (Input.GetMouseButtonDown(0) && !buildWalls)
                {
                    if (spawnedWallHolo)
                    {
                        var wallIcon = getChildGameObjectByName(spawnedWallHolo, "Wall_1");
                        if (wallIcon)
                        {
                            var wallCheck = wallIcon.GetComponent<WallBuildCheck>();
                            var checkFunds = false;
                            if (isTesting)
                            {
                                checkFunds = (energon >= wallCheck.EnergonPrice && credits >= wallCheck.CreditsPrice);
                            }
                            else
                            {
                                checkFunds = (playerBase.getEnergonAmount() >= wallCheck.EnergonPrice && playerBase.getCreditsAmount() >= wallCheck.CreditsPrice);
                            }
                            if (!wallCheck.SpaceOccupied && checkFunds)
                            {
                                //spawnedWallHolo.GetComponent<Rigidbody>().isKinematic  = true;
                                var spawnedWall = Instantiate(spawnedWallHolo, spawnedWallHolo.transform.position, spawnedWallHolo.transform.rotation);
                                var _spawnedWall = getChildGameObjectByName(spawnedWall, "Wall_1");
                                _spawnedWall.GetComponent<MeshRenderer>().material = builtWallMaterial;
                                spawnedWall.tag = "PlayerWall";
                                _spawnedWall.tag = "PlayerWall";
                                _spawnedWall.layer = LayerMask.NameToLayer("PlayerBase"); 
                                //spawnedWall.tag = "PlayerWall";
                                _spawnedWall.GetComponent<Collider>().isTrigger = false;                    
                                _spawnedWall.GetComponent<WallBuildCheck>().IsBuilt = true;
                                
                                // Decreasing funds 
                                if(!isTesting && playerBase)
                                {
                                    playerBase.setEnergonAmount(playerBase.getEnergonAmount() - wallCheck.EnergonPrice);
                                    playerBase.setCreditsAmount(playerBase.getCreditsAmount() - wallCheck.CreditsPrice);
                                }
                                else{
                                    energon -= wallCheck.EnergonPrice;
                                    credits -= wallCheck.CreditsPrice;
                                }
                                

                                wallBuildBtn.interactable = true;
                                buildingArea.SetActive(false);
                                spawnedWallHolo.SetActive(false);
                                startWallBuild = false;  
                            }
                            else
                            {
                                print("Can't place wall here");
                                errorWindow.SetActive(true);                                
                                if (!checkFunds)
                                {
                                    print("Not enought funds to place a wall here");
                                    errorMessage.text = "Not enought funds to place a wall here!";
                                }
                                else
                                {
                                    errorMessage.text = "Can't place wall here. Space occupied!";
                                }
                                wallBuildBtn.interactable = true;   
                                buildingArea.SetActive(false);  
                                spawnedWallHolo.SetActive(false);  
                                startWallBuild = false;  
                            }
                        }
                    }
                }          
                if (Input.GetMouseButtonDown(1) && !enableDrag) //Checking if right mouse button is clicked
                {
                    if (rotationClicked)
                    {
                        spawnedWallHolo.transform.eulerAngles = Vector3.Lerp(spawnedWallHolo.transform.rotation.eulerAngles, new Vector3(0f, 0f, 0f), Time.deltaTime * rotationSpeed);
                        rotationClicked = false;
                        return;
                    }
                    spawnedWallHolo.transform.eulerAngles = Vector3.Lerp(spawnedWallHolo.transform.rotation.eulerAngles, new Vector3(0f, 90f, 0f), Time.deltaTime * rotationSpeed);
                    rotationClicked = !rotationClicked;
                }
                
            }
    }
    
    private void resetShiftWalls()
    {
         enableDrag = false;
         staticWalls = GameObject.FindGameObjectsWithTag("WallHolo");
         if (staticWalls.Length > 0)
         {
            foreach (var wall in staticWalls)
            {
                Destroy(wall);
            }
            staticWalls = new GameObject[0];
            //wallBuildBtn.interactable = true;
            //spawnedWallHolo.SetActive(true);
         }
    }
    private void handleShiftFundError()
    {
         print("Not enough funds to build walls!");
         print("R={e=" + energonPriceAmount + ", c=" + creditsPriceAmount + "}");
         errorWindow.SetActive(true);
         errorMessage.text = "Not enough funds to build walls!";
         errorMessage.text += "\nRequired: " + energonPriceAmount + " energon, " + creditsPriceAmount + " credits";
         energonPriceAmount = 0;
         creditsPriceAmount = 0;
         buildWalls = false;
         wallBuildBtn.interactable = true;   
         buildingArea.SetActive(false);  
         spawnedWallHolo.SetActive(false);  
         resetShiftWalls(); 
    }
    private GameObject getChildGameObjectByName(GameObject parentObject, string name)
    {
        Transform trans = parentObject.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null) {
            return childTrans.gameObject;
        } else {
            return null;
        }
    }
}
