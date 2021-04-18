using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//NOTE: Create obstacles
//NOTE: Create playerBase and Borg spawner
//NOTE: Create deposit spawner
//NOTE: For starting point user: 4x4 platform

public class MapMaker : MonoBehaviour
{
    //[SerializeField] GameObject platform; //platform cube
    [Header("Main cnf. p.")]
    [SerializeField] GameObject pCube; //platform cube
    [SerializeField] GameObject walls;
    [SerializeField] GameObject areaWall;
    [SerializeField] GameObject holeCube;
    [Tooltip("Platform Width in Cubes")]
    [SerializeField] int pWidth = 6;  
    [Tooltip("Platform Height in Cubes")] 
    [SerializeField] int pHeight = 6; 
    [Tooltip("Height of the walls")]
    [SerializeField] float wHeight = 4;
    [Header("Hole generation cnf.")]
    [SerializeField] int holeCount = 2;
    [Tooltip("Transformation offset XYZ")] 
    [SerializeField] float tOffset = 50f;
    [SerializeField] bool generateNavMesh = false;
    [SerializeField] bool generateWalls = false;
    [SerializeField] bool generateGround = false;
    [SerializeField] bool createAreaWalls = false;
    [SerializeField] bool createHoles = false;
    //private float x = 0f, y = 0f, z = 0f;
    //private GameObject sPlatform;
    //Object sub groups
    private GameObject levelMap;
    private GameObject groundGrp;
    private GameObject wallAreaGrp;
    private GameObject holeGrp;

    private List<int> pRandIndexesRow = new List<int>();
    private List<int> pRandIndexesCol = new List<int>();
    private Helper help = new Helper();
   // private GameObject startCube;
    private List<List<GameObject>> sCubes = new List<List<GameObject>>(); //list of spawned cubes
    void Start()
    {
        // sPlatform = Instantiate(platform, Vector3.zero, platform.transform.rotation);
        // sPlatform.transform.localScale = new Vector3(pWidth, 0f, pHeight);
        // sPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
        //var tOffset = pCube.transform.localScale.x;
        //Create platform object groups
        levelMap = new GameObject("LevelMap"); //Main paltform group
        
        levelMap.transform.position = Vector3.zero;
        //startCube = Instantiate(pCube, new Vector3(-(pWidth / 2 + tOffset), y, (pHeight / 2 + tOffset)), pCube.transform.rotation);

        // var sX = startCube.transform.position.x;
        // var sZ = startCube.transform.position.z;

        //startCube.SetActive(false);
        if (generateGround)
        {
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
                if (sCubes[0].Count >= pWidth) print("PRow created!");
               
            }
            groundGrp.transform.parent = levelMap.transform;
        }

        //platform.transform.position = new Vector3(-((pWidth * tOffset) / 2), 0f, ((pHeight * tOffset) / 2));
        //For centering of platform rect in 0,0,0
        var newWidth = (pCube.transform.localScale.x * pWidth) / 2;
        var newHeight = (pCube.transform.localScale.x * pHeight) / 2;
        levelMap.transform.position = new Vector3(-(newWidth - newWidth / 4), 0f, (newHeight - newHeight / 4));
        //Build platform main wall
        if (generateWalls)
        {           
            var spawnedWalls = Instantiate(walls, Vector3.zero, walls.transform.rotation);
            spawnedWalls.transform.localScale = new Vector3((((pWidth * tOffset) / 4)) / 2 - 5, spawnedWalls.transform.localScale.y * wHeight, (((pHeight * tOffset) / 4)) / 2 - 5);
            spawnedWalls.transform.position = new Vector3(spawnedWalls.transform.position.x, wHeight / 2, spawnedWalls.transform.position.z);
            spawnedWalls.transform.parent = levelMap.transform;
        }

        //Creating paltform obstacles
        if (createAreaWalls)
        {
            // var p1 = sCubes[pWidth / 2 - 1].transform;
            // var spawnWall = Instantiate(areaWall, p1.position, p1.rotation);

            // var p1 = sCubes[pWidth / 2 - 1].transform;
            // var spawnWall = Instantiate(areaWall, p1.position, p1.rotation);
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
        }
        if (createHoles)
        {
            var hList = new List<GameObject>();
            holeGrp = new GameObject("Platform holes");
            holeGrp.transform.position = Vector3.zero;
            for(int hIndex = 0; hIndex < holeCount; hIndex++)
            {
                //For making unique random numbers
                var randNum = Random.Range(0, sCubes.Count);
                if (!pRandIndexesRow.Contains(randNum) && randNum != sCubes.Count / 2 - 1)
                {
                    pRandIndexesRow.Add(randNum);
                }
                else
                {
                    while(pRandIndexesRow.Contains(randNum) && randNum == sCubes.Count / 2 - 1)
                    {
                        randNum = Random.Range(0, sCubes.Count);
                        if (!pRandIndexesRow.Contains(randNum) && randNum != sCubes.Count / 2 - 1)
                        {
                            pRandIndexesRow.Add(randNum);
                            break;
                        }
                    }
                }
                var rowIndex = Random.Range(0, sCubes[pRandIndexesRow[0]].Count);
                var ePlatform = sCubes[pRandIndexesRow[0]][rowIndex];
                var spawnedHole = Instantiate(holeCube, ePlatform.transform.position, ePlatform.transform.rotation);
                
                hList.Add(spawnedHole);
                sCubes.Add(hList);
                
                spawnedHole.transform.parent = holeGrp.transform;
                ePlatform.SetActive(false);
                //sCubes.Add()
            }
            holeGrp.transform.parent = levelMap.transform;
            //var holeIndex = sCubex[Random]
        }
        //NOTE: Generate NavMesh at the end
        if (generateNavMesh)
        {
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
                            if(cube.activeSelf)
                            {
                                cube.GetComponent<NavMeshSurface>().BuildNavMesh();
                            }
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
