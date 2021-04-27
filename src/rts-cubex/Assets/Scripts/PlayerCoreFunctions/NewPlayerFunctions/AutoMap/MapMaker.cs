using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
//NOTE: Fix boundary walls groups and y scale
//NOTE: FIX map shadows
//NOTE: Spawn game hood
//NOTE: FIX energon deposit spawning
//NOTE: For starting point user: 6x6 platform

public class MapMaker : MonoBehaviour
{
    [Header("Map UI cnf.:")]
    [SerializeField] GameObject levelLoadingCanvas;
    [SerializeField] Image loadBar;
    [SerializeField] GameObject gameHood;
    [SerializeField] float delayTime = 2f;
    [Header("Level map cnf.:")]   
    [SerializeField] GameObject mainCamera; 
    [SerializeField] GameObject pCube; //platform cube
    [SerializeField] GameObject walls;
    [SerializeField] GameObject areaWall;
    [SerializeField] GameObject holeCube;
    [SerializeField] GameObject moundCube;
    [SerializeField] GameObject moundNormal;
    [SerializeField] GameObject energonDeposit;
    [SerializeField] GameObject wallCube;
    [SerializeField] GameObject sPlayerBase;
    [SerializeField] GameObject sBorgBase;
    [Tooltip("Platform Width in Cubes")]
    [Range(6, 6)] //For locking values in inspector
    [SerializeField] int pWidth = 6;  
    [Tooltip("Platform Height in Cubes")] 
    [Range(6, 6)] //For locking values in inspector
    [SerializeField] int pHeight = 6; 
    [Tooltip("Height of the walls")]
    [Range(2, 4)]
    [SerializeField] float wallHeight = 4;
    [Header("Hole generation cnf.")]
    [Range(1, 2)]
    [SerializeField] int holeCount = 2;
    [Header("Mound generation cnf")]
    [Range(1, 2)]
    [SerializeField] int moundCount = 2;
    [Tooltip("Transformation offset XYZ")] 
    [SerializeField] float tOffset = 50f;
    [SerializeField] bool isTesting = true;
    [SerializeField] bool generateNavMesh = false;
    [SerializeField] bool generateWalls = false;
    [SerializeField] bool createAreaWalls = false;
    [SerializeField] bool createHoles = false;
    [SerializeField] bool createMounds = false;
    [SerializeField] bool spawnEDeposits = false;
    [SerializeField] bool spawnGameBases = false;
   
    
    //Object sub groups
    private GameObject levelMap;
    private GameObject groundGrp;
    private GameObject wallAreaGrp;
    private GameObject boundWalls;
    private GameObject holeGrp;
    private GameObject moundGrp;
    private GameObject energonGrp;
    private GameObject gSpawnMBaseGrp;
    
    private int oldHoleIndex = 0;
    private int oldMoundIndex = 1;
    private bool holeLaneSwitch = false;
    private bool moundLaneSwitch = false;
    private bool generateGround = true;
    private bool createLevel = true;

    private Helper help = new Helper();
   // private GameObject startCube;
    private List<List<GameObject>> sCubes = new List<List<GameObject>>(); //list of spawned cubes
    private List<GameObject> mounds = new List<GameObject>();
    void Start()
    {
      
        // if (isTesting)
        // {
        //     levelLoadingCanvas.SetActive(false);
        // }
        // else
        // {
        //     generateNavMesh = true;
        //     generateWalls = true;
        //     createAreaWalls = true;
        //     createHoles = true;
        //     createMounds = true;
        //     spawnEDeposits = true;
        //     spawnGameBases = true;
        // }
        //Prearing main game level map group
        //By default gamehood is deactivated
        gameHood.SetActive(false);
        //Create platform object groups
        levelMap = new GameObject("LevelMap"); //Main paltform group
        
        levelMap.transform.position = Vector3.zero;   
        
    }

    // Update is called once per frame
    void Update()
    {
        if (createLevel)
        {
            //Generating the required level map ground
            if (generateGround)
            {           
                //Ground generation
                //createPart(0, delayTime);
                groundGrp = new GameObject("Ground");
                groundGrp.transform.position = Vector3.zero;
                for (float posZIndex = 0f; posZIndex > -(pHeight * tOffset); posZIndex -= tOffset)
                {
                        var cList = new List<GameObject>();
                        var countW = 0;
                        for(float posXIndex = 0f; posXIndex < (pWidth * tOffset); posXIndex += tOffset)
                        {                     
                            var scube = Instantiate(pCube, new Vector3(posXIndex, 0f, posZIndex), pCube.transform.rotation);
                            scube.transform.parent = groundGrp.transform;
                            //scube.SetActive(true);
                            //countW++;
                            cList.Add(scube);
                        }
                        sCubes.Add(cList);
                    //  if (sCubes[0].Count >= pWidth) print("PRow created!");
                                    
                }
                groundGrp.transform.parent = levelMap.transform;   

                //For centering of platform rect in 0,0,0
                var newWidth = (pCube.transform.localScale.x * pWidth) / 2;
                var newHeight = (pCube.transform.localScale.x * pHeight) / 2;
                levelMap.transform.position = new Vector3(-(newWidth - newWidth / 4), 0f, (newHeight - newHeight / 4));
                                
                generateGround = false;

                // delay(delayTime);
                // loadBar.fillAmount += 0.125f;

                // if (!isTesting)
                //      generateWalls = true;
            }
                    //Build platform main boundary wall
            if (generateWalls && sCubes.Count > 0)
            {        
                //createPart(1, delayTime);       
                boundWalls = new GameObject("BoundWalls");
                boundWalls.transform.position = Vector3.zero;     
                //New wall spawn logic
                //Vertical wall generation logic
                int indexOffset = 1;
                for (int lIndex = sCubes.Count / 2 - 1; lIndex >= 0; lIndex--)
                {
                    var spawnedWall1 = Instantiate(wallCube, 
                                                //wall position
                                                sCubes[lIndex][0].transform.position + 
                                                //wall pos offset
                                                new Vector3((-pCube.transform.localScale.x / 2) - 5f, //x
                                                wallHeight / 2, //y
                                                0f), //z
                                                wallCube.transform.rotation);
                    spawnedWall1.transform.parent = boundWalls.transform;

                    var spawnedWall2 = Instantiate(wallCube, 
                                                //wall position
                                                sCubes[lIndex][sCubes[lIndex].Count - 1].transform.position + 
                                                //wall pos offset
                                                new Vector3(pCube.transform.localScale.x / 2 + 5f, //x
                                                wallHeight / 2, //y
                                                0f), //z
                                                wallCube.transform.rotation);
                    spawnedWall2.transform.parent = boundWalls.transform;                               
                    
                    var spawnedWall3 = Instantiate(wallCube, 
                                                //wall position
                                                sCubes[lIndex + indexOffset][0].transform.position +
                                                //wall pos offset
                                                new Vector3((-pCube.transform.localScale.x / 2) - 5f, //x
                                                wallHeight / 2, //y
                                                0f), //z
                                                wallCube.transform.rotation);
                    spawnedWall3.transform.parent = boundWalls.transform;

                    var spawnedWall4 = Instantiate(wallCube, 
                                                //wall position
                                                sCubes[lIndex + indexOffset][sCubes[lIndex + indexOffset].Count - 1].transform.position +
                                                //wall pos offset
                                                new Vector3(pCube.transform.localScale.x / 2 + 5f, //x
                                                wallHeight / 2, //y
                                                0f), //z
                                                wallCube.transform.rotation);
                    spawnedWall4.transform.parent = boundWalls.transform;

                    indexOffset += 2;
                }
                //Horizontal wall spawning
                indexOffset = 1;
                for (int lIndex = sCubes[0].Count / 2 - 1; lIndex >= 0; lIndex--)
                {
                    var spawnedWall1 = Instantiate(wallCube, 
                                                sCubes[0][lIndex].transform.position +
                                                new Vector3(0f, wallHeight / 2, (pCube.transform.localScale.x / 2) + 5f), 
                                                Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                    spawnedWall1.transform.parent = boundWalls.transform;

                    var spawnedWall2 = Instantiate(wallCube, 
                                                sCubes[0][lIndex + indexOffset].transform.position + 
                                                new Vector3(0f, wallHeight / 2, (pCube.transform.localScale.x / 2) + 5f), 
                                                Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                    spawnedWall2.transform.parent = boundWalls.transform;
                    
                    var spawnedWall3 = Instantiate(wallCube, 
                                                sCubes[sCubes.Count - 1][lIndex].transform.position + 
                                                new Vector3(0f, wallHeight / 2, -((pCube.transform.localScale.x / 2) + 5f)), 
                                                Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                    spawnedWall3.transform.parent = boundWalls.transform;

                    var spawnedWall4 = Instantiate(wallCube, 
                                                sCubes[sCubes.Count - 1][lIndex + indexOffset].transform.position + 
                                                new Vector3(0f, wallHeight / 2, -((pCube.transform.localScale.x / 2) + 5f)), 
                                                Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                    spawnedWall4.transform.parent = boundWalls.transform;
                    
                    indexOffset += 2;
                }
                //Regulating the scale
                boundWalls.transform.localScale = new Vector3(
                                                                boundWalls.transform.localScale.x,
                                                                wallHeight,
                                                                boundWalls.transform.localScale.z);
                //Parenting to main map group
                boundWalls.transform.parent = levelMap.transform;

                generateWalls = false;

                // delay(delayTime);
                // loadBar.fillAmount += 0.125f;
                
                // if (!isTesting)
                //     createAreaWalls = true;
            }

            //Creating paltform wall obstacles in cross pattern
            if (createAreaWalls && sCubes.Count > 0)
            {               
                //createPart(2, delayTime);
                wallAreaGrp = new GameObject("AreaWalls");
                wallAreaGrp.transform.position = Vector3.zero;
                bool rotateWall = false;
                int rIndex = 1;
                int cIndex = 1;
                float zOffset = 0f;
                for (int sCubeIndex = pHeight / 2 - 1; sCubeIndex >= 0; sCubeIndex--)
                {
                    if (sCubeIndex == pHeight / 2 - 1)
                    {
                        float xOffset = 0f; //For getting away from center
                        zOffset = pCube.transform.localScale.x / 2;
                        for(int sColIndex = pWidth / 2 - 1; sColIndex >= 0; sColIndex--)
                        {
                            var _leftTransform = sCubes[sCubeIndex + 1][sColIndex].transform;
                            var _rightTransform = sCubes[sCubeIndex + 1][sColIndex + cIndex].transform;
                            
                            if (sColIndex == pWidth / 2 - 1)
                            {
                                xOffset = pCube.transform.localScale.x / 2;
                            }
                            else
                            {
                                xOffset = 0f;
                            }

                            var _spawnWall1 = Instantiate(areaWall, _leftTransform.position + new Vector3(-xOffset, 0f, pCube.transform.localScale.x / 2), _leftTransform.rotation);
                            _spawnWall1.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            
                            var _spawnWall2 = Instantiate(areaWall, _rightTransform.position + new Vector3(xOffset, 0f, pCube.transform.localScale.x / 2), _rightTransform.rotation);
                            _spawnWall2.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

                            _spawnWall1.transform.parent = wallAreaGrp.transform;
                            _spawnWall2.transform.parent = wallAreaGrp.transform;

                            cIndex += 2;
                        }
                    }
                    else
                    {
                        zOffset = 0f;
                    }
                    var leftTransform = sCubes[sCubeIndex][pWidth / 2 - 1].transform;
                    var rightTransform = sCubes[sCubeIndex + rIndex][pWidth / 2 - 1].transform;
                    var spawnWall1 = Instantiate(areaWall, leftTransform.position + new Vector3(pCube.transform.localScale.x / 2, 0f, zOffset), leftTransform.rotation);
                    var spawnWall2 = Instantiate(areaWall, rightTransform.position + new Vector3(pCube.transform.localScale.x / 2, 0f, -zOffset), rightTransform.rotation);
                    spawnWall1.transform.parent = wallAreaGrp.transform;
                    spawnWall2.transform.parent = wallAreaGrp.transform;
                    rIndex += 2;
                // print(rIndex);
                }            

                //Parenting to main map group
                wallAreaGrp.transform.parent = levelMap.transform;
                
                createAreaWalls = false;

                // delay(delayTime);
                // loadBar.fillAmount += 0.125f;
                // if (!isTesting)
                //     createHoles = true;
            }
            //Creating platform holes
            if (createHoles && sCubes.Count > 0)
            {     
                //createPart(3, delayTime);     
                var hList = new List<GameObject>();
                holeGrp = new GameObject("Platform holes");
                holeGrp.transform.position = Vector3.zero;
                var pFirstLane = sCubes[0];
                var pEndLane = sCubes[sCubes.Count - 1];
                var spawnIndexes = new List<int>();
                //NOTE: Since we are working with a square ground platform, first and last lanes will always be equal
                spawnIndexes.Add(pFirstLane.Count / 2 - 1);
                spawnIndexes.Add(pFirstLane.Count / 2);            
                
                if (spawnIndexes.Count > 0)
                {
                    if(spawnIndexes.Count >= holeCount)
                    {
                        for(int index = 0; index < holeCount; index++)
                        {
                            holeLaneSwitch = !holeLaneSwitch;
                            List<GameObject> pLane = new List<GameObject>();
                            //List<int> lIndexes = new List<int>();
                            //For random range generation
                            // var sValue = 0;
                            // var eValue = 0;
                            if (holeLaneSwitch)
                            {
                                //lIndexes = rHoleIndexes;
                                pLane = pFirstLane;
                                //  sValue = 0;
                                //  eValue = spawnIndexes.Count / 2;
                            }
                            else
                            {
                                //lIndexes = rEHoleIndexes;
                                pLane = pEndLane;
                                // sValue = spawnIndexes.Count / 2;
                                // eValue = spawnIndexes.Count;
                            }
                            var randPosIndex = Random.Range(0, spawnIndexes.Count);
                            // For generating unique random numbers
                            if (oldHoleIndex == randPosIndex)
                            {
                                while(oldHoleIndex == randPosIndex)
                                {
                                    randPosIndex = Random.Range(0, spawnIndexes.Count);
                                    if (oldHoleIndex != randPosIndex)
                                    {      
                                    oldHoleIndex = randPosIndex;                            
                                    break; 
                                    }
                                }
                            }
                            else
                            {
                                oldHoleIndex = randPosIndex;  
                            }
                            //rHoleIndexes.Add(randNum);
                        
                            var sPosLane = pLane[spawnIndexes[randPosIndex]];//hole spawn point
                            
                            var spawnedHole = Instantiate(holeCube, sPosLane.transform.position, sPosLane.transform.rotation);
                            pLane.Add(spawnedHole);
                            spawnedHole.transform.parent = holeGrp.transform;
                            //Removing existing platform model
                            pLane.Remove(sPosLane);
                            Destroy(sPosLane);
                        }
                        
                    }
                    else
                    {
                        Debug.LogError("ERROR: Hole count is larger than spawn indexes set!");
                    }
                }
                
                holeGrp.transform.parent = levelMap.transform;            
                
                createHoles = false;

                // delay(delayTime);
                // loadBar.fillAmount += 0.125f;

                // if (!isTesting)
                //     createMounds = true;
            }

            //Standard mound generation
            if(createMounds && sCubes.Count > 0)
            {     
                //createPart(4, delayTime);       
                
                //moundCube
                moundGrp = new GameObject("Mounds");
                moundGrp.transform.position = Vector3.zero;

                var leftLane = sCubes[(((sCubes.Count - 1) / 2) - 1)];
                var rightLane = sCubes[sCubes.Count - ((sCubes.Count - 1) / 2)];
                var spawnIndexes = new List<int>();
                spawnIndexes.Add(((leftLane.Count - 1) / 2) - 1);
                spawnIndexes.Add(leftLane.Count - ((leftLane.Count - 1) / 2));
                // spawnIndexes.Add(((leftLane.Count - 1) / 2) + 1);
                // spawnIndexes.Add();
                
                for(int mIndex = 0; mIndex < moundCount; mIndex++)
                {
                    moundLaneSwitch = !moundLaneSwitch;
                    List<GameObject> pLane = new List<GameObject>(); 
                    List<GameObject> nPLane = new List<GameObject>(); 
                    var mRotAngle = Vector3.zero; //Rotation angle on Yaxis
                    if (moundLaneSwitch)
                    {
                        pLane = leftLane;
                        nPLane = rightLane;
                        mRotAngle = new Vector3(0f, 90f, 0f);
                    }
                    else
                    {
                        pLane = rightLane;
                        nPLane = leftLane;
                    }
                    var randPosIndex = Random.Range(0, spawnIndexes.Count);
                    if (randPosIndex == oldMoundIndex)
                    {
                        while(randPosIndex == oldMoundIndex)
                        {
                            randPosIndex = Random.Range(0, spawnIndexes.Count);
                            if (randPosIndex != oldMoundIndex)
                            {
                            oldMoundIndex = randPosIndex;
                            break; 
                            }
                        }
                    }
                    else
                    {
                        oldMoundIndex = randPosIndex;
                    }

                    //Mound spawning
                    var cPScale = pLane[spawnIndexes[randPosIndex]].transform.localScale;
                    var mPos = pLane[spawnIndexes[randPosIndex]].transform.position + new Vector3(0f, cPScale.y, 0f);
                    
                    //Mound normal spawning pos
                    //var nMIndex = spawnIndexes[randPosIndex] > 0 ? spawnIndexes[randPosIndex] - 1 : spawnIndexes[randPosIndex];
                    var mNPos = nPLane[spawnIndexes[randPosIndex]].transform.position + new Vector3(0f, cPScale.y, 0f);
                    
                    var spawnedMound = Instantiate(moundCube, mPos, Quaternion.Euler(mRotAngle));
                    mounds.Add(spawnedMound);

                    var spawnedMoundN = Instantiate(moundNormal, mNPos, Quaternion.Euler(mRotAngle));
                    //Parent to required sub group
                    spawnedMound.transform.parent = moundGrp.transform;
                    spawnedMoundN.transform.parent = moundGrp.transform;
                }
                moundGrp.transform.parent = levelMap.transform;
            
                createMounds = false;

                // delay(delayTime);
                // loadBar.fillAmount += 0.125f;

                //  if (!isTesting)
                //     spawnEDeposits = true;
            }

            //Energon deposit spawning
            if (spawnEDeposits && sCubes.Count > 0)
            {     
                //createPart(5, delayTime);      
                energonGrp = new GameObject("EnergonDeposits");
                energonGrp.transform.position = Vector3.zero;

                var rightLane = sCubes[((sCubes.Count - 1) / 2) - 2];
                var leftLane = sCubes[(sCubes.Count - 1) / 2];
                var offsetT = pCube.transform.localScale.x / 4;


                var endPosX = rightLane[(rightLane.Count / 2)].transform.position.x - 2 * pCube.transform.localScale.x;           
                
                //Right lane spawning
                //Spawning in the right region energon deposit
                
                for(float xPos = rightLane[0].transform.position.x; xPos < endPosX; xPos += offsetT)
                {        
                    print("Spawning right lane energon deposits");
                    var ePos = new Vector3(xPos, pCube.transform.localScale.y + 1f, rightLane[(rightLane.Count / 2 - 1)].transform.position.z - 2 * offsetT);       
                    var spawnedDeposit = Instantiate(energonDeposit, ePos, energonDeposit.transform.rotation);               
                    spawnedDeposit.transform.parent = energonGrp.transform;
                }

                //Spawning in the left region energon deposits
                endPosX = rightLane[(rightLane.Count - 1)].transform.position.x + pCube.transform.localScale.x;           
                for(float xPos = rightLane[rightLane.Count - 2].transform.position.x; xPos >= endPosX; xPos -= offsetT)
                {     
                    print("Spawning right lane energon deposits");   
                    var ePos = new Vector3(xPos, pCube.transform.localScale.y + 1f, rightLane[(rightLane.Count / 2 - 1)].transform.position.z - 2 * offsetT);       
                    var spawnedDeposit = Instantiate(energonDeposit, ePos, energonDeposit.transform.rotation);               
                    spawnedDeposit.transform.parent = energonGrp.transform;
                }
                //--------------------------------
                //Left lane spawning
                endPosX = leftLane[(leftLane.Count / 2)].transform.position.x - 2 * pCube.transform.localScale.x;           
                //endPosX works with both lanes because platform is a square
                for(float xPos = leftLane[0].transform.position.x; xPos < endPosX; xPos += offsetT)
                {     
                    print("Spawning left lane energon deposits");
                    var ePos = new Vector3(xPos, pCube.transform.localScale.y + 1f, leftLane[(leftLane.Count / 2 - 1)].transform.position.z - 10 * offsetT);       
                    var spawnedDeposit = Instantiate(energonDeposit, ePos, energonDeposit.transform.rotation);               
                    spawnedDeposit.transform.parent = energonGrp.transform;
                }

                //Spawning in the left region energon deposits
                //FIX this-----------------------
                endPosX = leftLane[(leftLane.Count - 1)].transform.position.x - 2 * pCube.transform.localScale.x;           
                for(float xPos = leftLane[leftLane.Count - 2].transform.position.x; xPos >= endPosX; xPos -= offsetT)
                {    
                    print("Spawning left lane energon deposits");    
                    var ePos = new Vector3(xPos, pCube.transform.localScale.y + 1f, leftLane[(leftLane.Count / 2 - 1)].transform.position.z - 10 * offsetT);       
                    var spawnedDeposit = Instantiate(energonDeposit, ePos, energonDeposit.transform.rotation);               
                    spawnedDeposit.transform.parent = energonGrp.transform;
                }
                energonGrp.transform.parent = levelMap.transform;
                
                spawnEDeposits = false;

                // delay(delayTime);
                // loadBar.fillAmount += 0.125f;

                // if (!isTesting)
                //     spawnGameBases = true;
            }

            //Spawning main level bases and required game hood
            if (spawnGameBases && sCubes.Count > 0)
            {    
                //createPart(6, delayTime);        
                // //Activating the game hood
                gameHood.SetActive(true);

                gSpawnMBaseGrp = new GameObject("Game_MBases");
                gSpawnMBaseGrp.transform.position = Vector3.zero;
                var firstLane = sCubes[0];
                var endLane = sCubes[sCubes.Count - 1];
                var spawnPoints = new List<Transform>();

                // foreach(var i in firstLane)
                //     print(i.name + "----" + i.transform.position);

                spawnPoints.Add(firstLane[0].transform);
                spawnPoints.Add(firstLane[firstLane.Count - 2].transform); //2 cuz for not looking at the attached hole object
                spawnPoints.Add(endLane[0].transform);
                spawnPoints.Add(endLane[firstLane.Count - 2].transform);

                var yPosOffset = pCube.transform.localScale.y + 2f;

                //SpawnPlayerBase
                var sPos = spawnPoints[Random.Range(0, spawnPoints.Count)];            
                var spawnPlayerBase = Instantiate(sPlayerBase, sPos.position + new Vector3(0f, yPosOffset, 0f), sPlayerBase.transform.rotation);
                mainCamera.transform.position = new Vector3(spawnPlayerBase.transform.position.x, mainCamera.transform.position.y, spawnPlayerBase.transform.position.z);
                
                spawnPoints.Remove(sPos);
                spawnPlayerBase.transform.parent = gSpawnMBaseGrp.transform;
                gSpawnMBaseGrp.transform.parent = levelMap.transform;

                //SpawnBorgBase
                sPos = spawnPoints[Random.Range(0, spawnPoints.Count)];
                var spawnBorgBase = Instantiate(sBorgBase, sPos.position + new Vector3(0f, yPosOffset, 0f), sBorgBase.transform.rotation);
                spawnPoints.Remove(sPos);
                spawnBorgBase.transform.parent = gSpawnMBaseGrp.transform;
                gSpawnMBaseGrp.transform.parent = levelMap.transform;

                spawnGameBases = false;

                // delay(delayTime);
                // loadBar.fillAmount += 0.125f;

                // if (!isTesting)
                //     generateNavMesh = true;
            }
            //NOTE: Generate NavMesh at the end
            if (generateNavMesh && sCubes.Count > 0)
            {   
                //createPart(7, delayTime);         
                if(sCubes.Count > 0)
                {
                    foreach(var crow in sCubes)
                    {
                        foreach(var cube in crow)
                        {
                            if ((cube.name.ToLower()).Contains("hole"))
                            {
                                var iPlatform = help.getChildGameObjectByName(cube, "BWArea");
                                iPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
                            }                        
                            else
                            {
                                // if(cube.activeSelf)
                                // {
                                    cube.GetComponent<NavMeshSurface>().BuildNavMesh();
                                //}
                            }
                        }
                    }
                }
                //Mound navmesh generation
                if (mounds.Count > 0)
                {
                    foreach(var m in mounds)
                    {
                        m.GetComponent<MoundNavGen>().BuildNavMesh = true;
                    }
                }
                
                 generateNavMesh = false;
                // loadBar.fillAmount += 0.125f;                
            }
            createLevel = false;
            levelLoadingCanvas.SetActive(false);
        }
    }
    // private void delay(float time){
    //     StartCoroutine(handleDelay(time));
    // }
    // IEnumerator handleDelay(float time)
    // {
    //     yield return new WaitForSeconds(time);
    // }

    // private void createPart(int pID, float loadTime)
    // {
    //     StartCoroutine(handleLoad(pID, loadTime));
    // }
    // IEnumerator handleLoad(int pID, float time)
    // {
    //     switch(pID)
    //     {
    //         case 0:
    //         //Ground generation
    //             groundGrp = new GameObject("Ground");
    //             groundGrp.transform.position = Vector3.zero;
    //             for (float posZIndex = 0f; posZIndex > -(pHeight * tOffset); posZIndex -= tOffset)
    //             {
    //                     var cList = new List<GameObject>();
    //                     var countW = 0;
    //                     for(float posXIndex = 0f; posXIndex < (pWidth * tOffset); posXIndex += tOffset)
    //                     {                     
    //                         var scube = Instantiate(pCube, new Vector3(posXIndex, 0f, posZIndex), pCube.transform.rotation);
    //                         scube.transform.parent = groundGrp.transform;
    //                         //scube.SetActive(true);
    //                         //countW++;
    //                         cList.Add(scube);
    //                     }
    //                     sCubes.Add(cList);
    //                 //  if (sCubes[0].Count >= pWidth) print("PRow created!");
                                    
    //             }
    //             groundGrp.transform.parent = levelMap.transform;   

    //             //For centering of platform rect in 0,0,0
    //             var newWidth = (pCube.transform.localScale.x * pWidth) / 2;
    //             var newHeight = (pCube.transform.localScale.x * pHeight) / 2;
    //             levelMap.transform.position = new Vector3(-(newWidth - newWidth / 4), 0f, (newHeight - newHeight / 4));
                                
    //             generateGround = false;
    //             yield return new WaitForSeconds(time);
    //             loadBar.fillAmount += 0.125f;

    //             if (!isTesting)
    //                  generateWalls = true;
    //         break;

    //         case 1:
    //              boundWalls = new GameObject("BoundWalls");
    //             boundWalls.transform.position = Vector3.zero;     
    //             //New wall spawn logic
    //             //Vertical wall generation logic
    //             int indexOffset = 1;
    //             for (int lIndex = sCubes.Count / 2 - 1; lIndex >= 0; lIndex--)
    //             {
    //                 var spawnedWall1 = Instantiate(wallCube, 
    //                                             //wall position
    //                                             sCubes[lIndex][0].transform.position + 
    //                                             //wall pos offset
    //                                             new Vector3((-pCube.transform.localScale.x / 2) - 5f, //x
    //                                             wallHeight / 2, //y
    //                                             0f), //z
    //                                             wallCube.transform.rotation);
    //                 spawnedWall1.transform.parent = boundWalls.transform;

    //                 var spawnedWall2 = Instantiate(wallCube, 
    //                                             //wall position
    //                                             sCubes[lIndex][sCubes[lIndex].Count - 1].transform.position + 
    //                                             //wall pos offset
    //                                             new Vector3(pCube.transform.localScale.x / 2 + 5f, //x
    //                                             wallHeight / 2, //y
    //                                             0f), //z
    //                                             wallCube.transform.rotation);
    //                 spawnedWall2.transform.parent = boundWalls.transform;                               
                    
    //                 var spawnedWall3 = Instantiate(wallCube, 
    //                                             //wall position
    //                                             sCubes[lIndex + indexOffset][0].transform.position +
    //                                             //wall pos offset
    //                                             new Vector3((-pCube.transform.localScale.x / 2) - 5f, //x
    //                                             wallHeight / 2, //y
    //                                             0f), //z
    //                                             wallCube.transform.rotation);
    //                 spawnedWall3.transform.parent = boundWalls.transform;

    //                 var spawnedWall4 = Instantiate(wallCube, 
    //                                             //wall position
    //                                             sCubes[lIndex + indexOffset][sCubes[lIndex + indexOffset].Count - 1].transform.position +
    //                                             //wall pos offset
    //                                             new Vector3(pCube.transform.localScale.x / 2 + 5f, //x
    //                                             wallHeight / 2, //y
    //                                             0f), //z
    //                                             wallCube.transform.rotation);
    //                 spawnedWall4.transform.parent = boundWalls.transform;

    //                 indexOffset += 2;
    //             }
    //             //Horizontal wall spawning
    //             indexOffset = 1;
    //             for (int lIndex = sCubes[0].Count / 2 - 1; lIndex >= 0; lIndex--)
    //             {
    //                 var spawnedWall1 = Instantiate(wallCube, 
    //                                             sCubes[0][lIndex].transform.position +
    //                                             new Vector3(0f, wallHeight / 2, (pCube.transform.localScale.x / 2) + 5f), 
    //                                             Quaternion.Euler(new Vector3(0f, 90f, 0f)));
    //                 spawnedWall1.transform.parent = boundWalls.transform;

    //                 var spawnedWall2 = Instantiate(wallCube, 
    //                                             sCubes[0][lIndex + indexOffset].transform.position + 
    //                                             new Vector3(0f, wallHeight / 2, (pCube.transform.localScale.x / 2) + 5f), 
    //                                             Quaternion.Euler(new Vector3(0f, 90f, 0f)));
    //                 spawnedWall2.transform.parent = boundWalls.transform;
                    
    //                 var spawnedWall3 = Instantiate(wallCube, 
    //                                             sCubes[sCubes.Count - 1][lIndex].transform.position + 
    //                                             new Vector3(0f, wallHeight / 2, -((pCube.transform.localScale.x / 2) + 5f)), 
    //                                             Quaternion.Euler(new Vector3(0f, 90f, 0f)));
    //                 spawnedWall3.transform.parent = boundWalls.transform;

    //                 var spawnedWall4 = Instantiate(wallCube, 
    //                                             sCubes[sCubes.Count - 1][lIndex + indexOffset].transform.position + 
    //                                             new Vector3(0f, wallHeight / 2, -((pCube.transform.localScale.x / 2) + 5f)), 
    //                                             Quaternion.Euler(new Vector3(0f, 90f, 0f)));
    //                 spawnedWall4.transform.parent = boundWalls.transform;
                    
    //                 indexOffset += 2;
    //             }
    //             //Regulating the scale
    //             boundWalls.transform.localScale = new Vector3(
    //                                                             boundWalls.transform.localScale.x,
    //                                                             wallHeight,
    //                                                             boundWalls.transform.localScale.z);
    //             //Parenting to main map group
    //             boundWalls.transform.parent = levelMap.transform;

    //             generateWalls = false;                
    //             yield return new WaitForSeconds(time);
               
    //             loadBar.fillAmount += 0.125f;
                
    //             if (!isTesting)
    //                 createAreaWalls = true;
    //         break;

    //         case 2:
    //         wallAreaGrp = new GameObject("AreaWalls");
    //             wallAreaGrp.transform.position = Vector3.zero;
    //             bool rotateWall = false;
    //             int rIndex = 1;
    //             int cIndex = 1;
    //             float zOffset = 0f;
    //             for (int sCubeIndex = pHeight / 2 - 1; sCubeIndex >= 0; sCubeIndex--)
    //             {
    //                 if (sCubeIndex == pHeight / 2 - 1)
    //                 {
    //                     float xOffset = 0f; //For getting away from center
    //                     zOffset = pCube.transform.localScale.x / 2;
    //                     for(int sColIndex = pWidth / 2 - 1; sColIndex >= 0; sColIndex--)
    //                     {
    //                         var _leftTransform = sCubes[sCubeIndex + 1][sColIndex].transform;
    //                         var _rightTransform = sCubes[sCubeIndex + 1][sColIndex + cIndex].transform;
                            
    //                         if (sColIndex == pWidth / 2 - 1)
    //                         {
    //                             xOffset = pCube.transform.localScale.x / 2;
    //                         }
    //                         else
    //                         {
    //                             xOffset = 0f;
    //                         }

    //                         var _spawnWall1 = Instantiate(areaWall, _leftTransform.position + new Vector3(-xOffset, 0f, pCube.transform.localScale.x / 2), _leftTransform.rotation);
    //                         _spawnWall1.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            
    //                         var _spawnWall2 = Instantiate(areaWall, _rightTransform.position + new Vector3(xOffset, 0f, pCube.transform.localScale.x / 2), _rightTransform.rotation);
    //                         _spawnWall2.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

    //                         _spawnWall1.transform.parent = wallAreaGrp.transform;
    //                         _spawnWall2.transform.parent = wallAreaGrp.transform;

    //                         cIndex += 2;
    //                     }
    //                 }
    //                 else
    //                 {
    //                     zOffset = 0f;
    //                 }
    //                 var leftTransform = sCubes[sCubeIndex][pWidth / 2 - 1].transform;
    //                 var rightTransform = sCubes[sCubeIndex + rIndex][pWidth / 2 - 1].transform;
    //                 var spawnWall1 = Instantiate(areaWall, leftTransform.position + new Vector3(pCube.transform.localScale.x / 2, 0f, zOffset), leftTransform.rotation);
    //                 var spawnWall2 = Instantiate(areaWall, rightTransform.position + new Vector3(pCube.transform.localScale.x / 2, 0f, -zOffset), rightTransform.rotation);
    //                 spawnWall1.transform.parent = wallAreaGrp.transform;
    //                 spawnWall2.transform.parent = wallAreaGrp.transform;
    //                 rIndex += 2;
    //             // print(rIndex);
    //             }            

    //             //Parenting to main map group
    //             wallAreaGrp.transform.parent = levelMap.transform;
                
    //             createAreaWalls = false;                
                
    //             yield return new WaitForSeconds(time);

    //             loadBar.fillAmount += 0.125f;
    //             if (!isTesting)
    //                 createHoles = true;
    //         break;

    //         case 3:
    //             var hList = new List<GameObject>();
    //             holeGrp = new GameObject("Platform holes");
    //             holeGrp.transform.position = Vector3.zero;
    //             var pFirstLane = sCubes[0];
    //             var pEndLane = sCubes[sCubes.Count - 1];
    //             var spawnIndexes = new List<int>();
    //             //NOTE: Since we are working with a square ground platform, first and last lanes will always be equal
    //             spawnIndexes.Add(pFirstLane.Count / 2 - 1);
    //             spawnIndexes.Add(pFirstLane.Count / 2);            
                
    //             if (spawnIndexes.Count > 0)
    //             {
    //                 if(spawnIndexes.Count >= holeCount)
    //                 {
    //                     for(int index = 0; index < holeCount; index++)
    //                     {
    //                         holeLaneSwitch = !holeLaneSwitch;
    //                         List<GameObject> pLane = new List<GameObject>();
    //                         //List<int> lIndexes = new List<int>();
    //                         //For random range generation
    //                         // var sValue = 0;
    //                         // var eValue = 0;
    //                         if (holeLaneSwitch)
    //                         {
    //                             //lIndexes = rHoleIndexes;
    //                             pLane = pFirstLane;
    //                             //  sValue = 0;
    //                             //  eValue = spawnIndexes.Count / 2;
    //                         }
    //                         else
    //                         {
    //                             //lIndexes = rEHoleIndexes;
    //                             pLane = pEndLane;
    //                             // sValue = spawnIndexes.Count / 2;
    //                             // eValue = spawnIndexes.Count;
    //                         }
    //                         var randPosIndex = Random.Range(0, spawnIndexes.Count);
    //                         // For generating unique random numbers
    //                         if (oldHoleIndex == randPosIndex)
    //                         {
    //                             while(oldHoleIndex == randPosIndex)
    //                             {
    //                                 randPosIndex = Random.Range(0, spawnIndexes.Count);
    //                                 if (oldHoleIndex != randPosIndex)
    //                                 {      
    //                                 oldHoleIndex = randPosIndex;                            
    //                                 break; 
    //                                 }
    //                             }
    //                         }
    //                         else
    //                         {
    //                             oldHoleIndex = randPosIndex;  
    //                         }
    //                         //rHoleIndexes.Add(randNum);
                        
    //                         var sPosLane = pLane[spawnIndexes[randPosIndex]];//hole spawn point
                            
    //                         var spawnedHole = Instantiate(holeCube, sPosLane.transform.position, sPosLane.transform.rotation);
    //                         pLane.Add(spawnedHole);
    //                         spawnedHole.transform.parent = holeGrp.transform;
    //                         //Removing existing platform model
    //                         pLane.Remove(sPosLane);
    //                         Destroy(sPosLane);
    //                     }
                        
    //                 }
    //                 else
    //                 {
    //                     Debug.LogError("ERROR: Hole count is larger than spawn indexes set!");
    //                 }
    //             }
                
    //             holeGrp.transform.parent = levelMap.transform;            
                
    //             createHoles = false;

    //             yield return new WaitForSeconds(time);

    //             loadBar.fillAmount += 0.125f;

    //             if (!isTesting)
    //                 createMounds = true;
    //         break;

    //         case 4:
    //              //moundCube
    //             moundGrp = new GameObject("Mounds");
    //             moundGrp.transform.position = Vector3.zero;

    //             var leftLane = sCubes[(((sCubes.Count - 1) / 2) - 1)];
    //             var rightLane = sCubes[sCubes.Count - ((sCubes.Count - 1) / 2)];
    //             var spawnIndexes4 = new List<int>();
    //             spawnIndexes4.Add(((leftLane.Count - 1) / 2) - 1);
    //             spawnIndexes4.Add(leftLane.Count - ((leftLane.Count - 1) / 2));
    //             // spawnIndexes.Add(((leftLane.Count - 1) / 2) + 1);
    //             // spawnIndexes.Add();
                
    //             for(int mIndex = 0; mIndex < moundCount; mIndex++)
    //             {
    //                 moundLaneSwitch = !moundLaneSwitch;
    //                 List<GameObject> pLane = new List<GameObject>(); 
    //                 List<GameObject> nPLane = new List<GameObject>(); 
    //                 var mRotAngle = Vector3.zero; //Rotation angle on Yaxis
    //                 if (moundLaneSwitch)
    //                 {
    //                     pLane = leftLane;
    //                     nPLane = rightLane;
    //                     mRotAngle = new Vector3(0f, 90f, 0f);
    //                 }
    //                 else
    //                 {
    //                     pLane = rightLane;
    //                     nPLane = leftLane;
    //                 }
    //                 var randPosIndex = Random.Range(0, spawnIndexes4.Count);
    //                 if (randPosIndex == oldMoundIndex)
    //                 {
    //                     while(randPosIndex == oldMoundIndex)
    //                     {
    //                         randPosIndex = Random.Range(0, spawnIndexes4.Count);
    //                         if (randPosIndex != oldMoundIndex)
    //                         {
    //                         oldMoundIndex = randPosIndex;
    //                         break; 
    //                         }
    //                     }
    //                 }
    //                 else
    //                 {
    //                     oldMoundIndex = randPosIndex;
    //                 }

    //                 //Mound spawning
    //                 var cPScale = pLane[spawnIndexes4[randPosIndex]].transform.localScale;
    //                 var mPos = pLane[spawnIndexes4[randPosIndex]].transform.position + new Vector3(0f, cPScale.y, 0f);
                    
    //                 //Mound normal spawning pos
    //                 //var nMIndex = spawnIndexes[randPosIndex] > 0 ? spawnIndexes[randPosIndex] - 1 : spawnIndexes[randPosIndex];
    //                 var mNPos = nPLane[spawnIndexes4[randPosIndex]].transform.position + new Vector3(0f, cPScale.y, 0f);
                    
    //                 var spawnedMound = Instantiate(moundCube, mPos, Quaternion.Euler(mRotAngle));
    //                 mounds.Add(spawnedMound);

    //                 var spawnedMoundN = Instantiate(moundNormal, mNPos, Quaternion.Euler(mRotAngle));
    //                 //Parent to required sub group
    //                 spawnedMound.transform.parent = moundGrp.transform;
    //                 spawnedMoundN.transform.parent = moundGrp.transform;
    //             }
    //             moundGrp.transform.parent = levelMap.transform;
            
    //             createMounds = false;

    //             yield return new WaitForSeconds(time);

    //             loadBar.fillAmount += 0.125f;

    //              if (!isTesting)
    //                 spawnEDeposits = true;
    //         break;

    //         case 5:
    //             energonGrp = new GameObject("EnergonDeposits");
    //             energonGrp.transform.position = Vector3.zero;

    //             var _rightLane = sCubes[((sCubes.Count - 1) / 2) - 2];
    //             var _leftLane = sCubes[(sCubes.Count - 1) / 2];
    //             var offsetT = pCube.transform.localScale.x / 4;


    //             var endPosX = _rightLane[(_rightLane.Count / 2)].transform.position.x - 2 * pCube.transform.localScale.x;           
                
    //             //Right lane spawning
    //             //Spawning in the right region energon deposit
                
    //             for(float xPos = _rightLane[0].transform.position.x; xPos < endPosX; xPos += offsetT)
    //             {        
    //                 print("Spawning right lane energon deposits");
    //                 var ePos = new Vector3(xPos, pCube.transform.localScale.y + 1f, _rightLane[(_rightLane.Count / 2 - 1)].transform.position.z - 2 * offsetT);       
    //                 var spawnedDeposit = Instantiate(energonDeposit, ePos, energonDeposit.transform.rotation);               
    //                 spawnedDeposit.transform.parent = energonGrp.transform;
    //             }

    //             //Spawning in the left region energon deposits
    //             endPosX = _rightLane[(_rightLane.Count - 1)].transform.position.x + pCube.transform.localScale.x;           
    //             for(float xPos = _rightLane[_rightLane.Count - 2].transform.position.x; xPos >= endPosX; xPos -= offsetT)
    //             {     
    //                 print("Spawning right lane energon deposits");   
    //                 var ePos = new Vector3(xPos, pCube.transform.localScale.y + 1f, _rightLane[(_rightLane.Count / 2 - 1)].transform.position.z - 2 * offsetT);       
    //                 var spawnedDeposit = Instantiate(energonDeposit, ePos, energonDeposit.transform.rotation);               
    //                 spawnedDeposit.transform.parent = energonGrp.transform;
    //             }
    //             //--------------------------------
    //             //Left lane spawning
    //             endPosX = _leftLane[(_leftLane.Count / 2)].transform.position.x - 2 * pCube.transform.localScale.x;           
    //             //endPosX works with both lanes because platform is a square
    //             for(float xPos = _leftLane[0].transform.position.x; xPos < endPosX; xPos += offsetT)
    //             {     
    //                 print("Spawning left lane energon deposits");
    //                 var ePos = new Vector3(xPos, pCube.transform.localScale.y + 1f, _leftLane[(_leftLane.Count / 2 - 1)].transform.position.z - 10 * offsetT);       
    //                 var spawnedDeposit = Instantiate(energonDeposit, ePos, energonDeposit.transform.rotation);               
    //                 spawnedDeposit.transform.parent = energonGrp.transform;
    //             }

    //             //Spawning in the left region energon deposits
    //             //FIX this-----------------------
    //             endPosX = _leftLane[(_leftLane.Count - 1)].transform.position.x - 2 * pCube.transform.localScale.x;           
    //             for(float xPos = _leftLane[_leftLane.Count - 2].transform.position.x; xPos >= endPosX; xPos -= offsetT)
    //             {    
    //                 print("Spawning left lane energon deposits");    
    //                 var ePos = new Vector3(xPos, pCube.transform.localScale.y + 1f, _leftLane[(_leftLane.Count / 2 - 1)].transform.position.z - 10 * offsetT);       
    //                 var spawnedDeposit = Instantiate(energonDeposit, ePos, energonDeposit.transform.rotation);               
    //                 spawnedDeposit.transform.parent = energonGrp.transform;
    //             }
    //             energonGrp.transform.parent = levelMap.transform;
                
    //             spawnEDeposits = false;

    //             yield return new WaitForSeconds(time);
    //             loadBar.fillAmount += 0.125f;

    //             if (!isTesting)
    //                 spawnGameBases = true;

    //         break;

    //         case 6:
    //             //Activating the game hood
    //             gameHood.SetActive(true);

    //             gSpawnMBaseGrp = new GameObject("Game_MBases");
    //             gSpawnMBaseGrp.transform.position = Vector3.zero;
    //             var firstLane = sCubes[0];
    //             var endLane = sCubes[sCubes.Count - 1];
    //             var spawnPoints = new List<Transform>();

    //             // foreach(var i in firstLane)
    //             //     print(i.name + "----" + i.transform.position);

    //             spawnPoints.Add(firstLane[0].transform);
    //             spawnPoints.Add(firstLane[firstLane.Count - 2].transform); //2 cuz for not looking at the attached hole object
    //             spawnPoints.Add(endLane[0].transform);
    //             spawnPoints.Add(endLane[firstLane.Count - 2].transform);

    //             var yPosOffset = pCube.transform.localScale.y + 2f;

    //             //SpawnPlayerBase
    //             var sPos = spawnPoints[Random.Range(0, spawnPoints.Count)];            
    //             var spawnPlayerBase = Instantiate(sPlayerBase, sPos.position + new Vector3(0f, yPosOffset, 0f), sPlayerBase.transform.rotation);
    //             spawnPoints.Remove(sPos);
    //             spawnPlayerBase.transform.parent = gSpawnMBaseGrp.transform;
    //             gSpawnMBaseGrp.transform.parent = levelMap.transform;

    //             //SpawnBorgBase
    //             sPos = spawnPoints[Random.Range(0, spawnPoints.Count)];
    //             var spawnBorgBase = Instantiate(sBorgBase, sPos.position + new Vector3(0f, yPosOffset, 0f), sBorgBase.transform.rotation);
    //             spawnPoints.Remove(sPos);
    //             spawnBorgBase.transform.parent = gSpawnMBaseGrp.transform;
    //             gSpawnMBaseGrp.transform.parent = levelMap.transform;

    //             spawnGameBases = false;

    //             yield return new WaitForSeconds(time);
    //             loadBar.fillAmount += 0.125f;

    //             if (!isTesting)
    //                 generateNavMesh = true;
    //         break;

    //         case 7:
    //             if(sCubes.Count > 0)
    //             {
    //                 foreach(var crow in sCubes)
    //                 {
    //                     foreach(var cube in crow)
    //                     {
    //                         if ((cube.name.ToLower()).Contains("hole"))
    //                         {
    //                             var iPlatform = help.getChildGameObjectByName(cube, "BWArea");
    //                             iPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
    //                         }                        
    //                         else
    //                         {
    //                             // if(cube.activeSelf)
    //                             // {
    //                                 cube.GetComponent<NavMeshSurface>().BuildNavMesh();
    //                             //}
    //                         }
    //                     }
    //                 }
    //             }
    //             //Mound navmesh generation
    //             if (mounds.Count > 0)
    //             {
    //                 foreach(var m in mounds)
    //                 {
    //                     m.GetComponent<MoundNavGen>().BuildNavMesh = true;
    //                 }
    //             }
                
    //             generateNavMesh = false;
    //             yield return new WaitForSeconds(time);
    //             loadBar.fillAmount += 0.125f;   
    //         break;
    //     }
       
        
   // }
}
