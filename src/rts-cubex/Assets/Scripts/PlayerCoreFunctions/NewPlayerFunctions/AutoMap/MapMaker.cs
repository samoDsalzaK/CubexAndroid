using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//NOTE: Create test logic to create NavMesh on runtime
//Create a platform, that creates obstacles
public class MapMaker : MonoBehaviour
{
    //[SerializeField] GameObject platform; //platform cube
    [SerializeField] GameObject pCube; //platform cube
    [SerializeField] float pWidth = 6f;   
    [SerializeField] float pHeight = 6f;  
    private float x = 0f, y = 0f, z = 0f;
    //private GameObject sPlatform;
    private GameObject startCube;
    private List<GameObject> sCubes = new List<GameObject>(); //list of spawned cubes
    void Start()
    {
        // sPlatform = Instantiate(platform, Vector3.zero, platform.transform.rotation);
        // sPlatform.transform.localScale = new Vector3(pWidth, 0f, pHeight);
        // sPlatform.GetComponent<NavMeshSurface>().BuildNavMesh();
        startCube = Instantiate(pCube, new Vector3(-(pWidth / 2), y, pHeight / 2), pCube.transform.rotation);

        var sX = startCube.transform.position.x;
        var sZ = startCube.transform.position.z;

        startCube.SetActive(false);

        for (float posZIndex = sZ; posZIndex >= -(pHeight / 2); posZIndex--)
        {
            for(float posXIndex = sX; posXIndex <= (pWidth / 2); posXIndex++)
            {                     
                var scube = Instantiate(startCube, new Vector3(posXIndex, y, posZIndex), startCube.transform.rotation);
                scube.SetActive(true);
                sCubes.Add(scube);
            }
        }
        // if(sCubes.Count > 0)
        // {
        //     foreach(var cube in sCubes)
        //     {
        //         cube.GetComponent<NavMeshSurface>().BuildNavMesh();
        //     }
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
