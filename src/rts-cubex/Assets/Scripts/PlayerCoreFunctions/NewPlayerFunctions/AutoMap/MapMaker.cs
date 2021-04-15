using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//NOTE: Create test logic to create NavMesh on runtime
//Create a platform, that creates obstacles
public class MapMaker : MonoBehaviour
{
    [SerializeField] GameObject pCube; //platform cube
    [SerializeField] float pWidth = 6f;   
    [SerializeField] float pHeight = 6f;  
    private float x = 0f, y = 0f, z = 0f;
    private GameObject startCube;
    //List<NavMeshSurface> pNCubes;
    void Start()
    {
        startCube = Instantiate(pCube, new Vector3(-(pWidth / 2), y, pHeight / 2), pCube.transform.rotation);

        var sX = startCube.transform.position.x;
        var sZ = startCube.transform.position.z;

        startCube.SetActive(false);

        for (float posZIndex = sZ; posZIndex >= -(pHeight / 2); posZIndex--)
        {
            for(float posXIndex = sX; posXIndex <= (pWidth / 2); posXIndex++)
            {                     
                Instantiate(startCube, new Vector3(posXIndex, y, posZIndex), startCube.transform.rotation);
                startCube.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
