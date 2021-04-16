using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//NOTE: Create obstacles
//NOTE: For starting point user: 4x4 platform
public class MapMaker : MonoBehaviour
{
    //[SerializeField] GameObject platform; //platform cube
    [Header("Main cnf. p.")]
    [SerializeField] GameObject pCube; //platform cube
    [SerializeField] GameObject walls;
    [Tooltip("Width & Heigh in Cubes")]
    [SerializeField] int pWidth = 6;  
    [Tooltip("Width & Heigh in Cubes")] 
    [SerializeField] int pHeight = 6; 
    [Tooltip("Transformation offset XYZ")] 
    [SerializeField] float tOffset = 50f;
    [SerializeField] bool generateNavMesh = false;
    [SerializeField] bool generateWalls = false;
    [SerializeField] bool generateGround = false;
    //private float x = 0f, y = 0f, z = 0f;
    //private GameObject sPlatform;
    private GameObject platform;
    private GameObject groundGrp;
   // private GameObject startCube;
    private List<GameObject> sCubes = new List<GameObject>(); //list of spawned cubes
    void Start()
    {
        // sPlatform = Instantiate(platform, Vector3.zero, platform.transform.rotation);
        // sPlatform.transform.localScale = new Vector3(pWidth, 0f, pHeight);
        // sPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
        //var tOffset = pCube.transform.localScale.x;
        //Create platform object groups
        platform = new GameObject("Platform"); //Main paltform group
        groundGrp = new GameObject("Ground");
        groundGrp.transform.position = Vector3.zero;
        platform.transform.position = Vector3.zero;
        //startCube = Instantiate(pCube, new Vector3(-(pWidth / 2 + tOffset), y, (pHeight / 2 + tOffset)), pCube.transform.rotation);

        // var sX = startCube.transform.position.x;
        // var sZ = startCube.transform.position.z;

        //startCube.SetActive(false);
        if (generateGround)
        {
            for (float posZIndex = 0f; posZIndex > -(pHeight * tOffset); posZIndex -= tOffset)
            {
                for(float posXIndex = 0f; posXIndex < (pWidth * tOffset); posXIndex += tOffset)
                {                     
                    var scube = Instantiate(pCube, new Vector3(posXIndex, 0f, posZIndex), pCube.transform.rotation);
                    scube.transform.parent = groundGrp.transform;
                    //scube.SetActive(true);
                    
                    sCubes.Add(scube);
                }
            }
            groundGrp.transform.parent = platform.transform;
        }

        //platform.transform.position = new Vector3(-((pWidth * tOffset) / 2), 0f, ((pHeight * tOffset) / 2));
        //For centering of platform rect in 0,0,0
        var newWidth = (pCube.transform.localScale.x * pWidth) / 2;
        var newHeight = (pCube.transform.localScale.x * pHeight) / 2;
        platform.transform.position = new Vector3(-(newWidth - newWidth / 4), 0f, (newHeight - newHeight / 4));
        //Build platform main wall
        if (generateWalls)
        {           
            var spawnedWalls = Instantiate(walls, Vector3.zero, walls.transform.rotation);
            spawnedWalls.transform.localScale = new Vector3((((pWidth * tOffset) / 4)) / 2 - 5, spawnedWalls.transform.localScale.y, (((pHeight * tOffset) / 4)) / 2 - 5);
            spawnedWalls.transform.parent = platform.transform;
        }

        //
        //NOTE: Generate NavMesh at the end
        if (generateNavMesh)
        {
            if(sCubes.Count > 0)
            {
                foreach(var cube in sCubes)
                {
                    cube.GetComponent<NavMeshSurface>().BuildNavMesh();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
