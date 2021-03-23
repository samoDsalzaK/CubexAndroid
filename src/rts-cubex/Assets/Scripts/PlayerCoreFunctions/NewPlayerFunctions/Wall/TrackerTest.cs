using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTest : MonoBehaviour
{
    //Plan
    // Start adding code to original wall models
    
    [SerializeField] GameObject tracker;
    [SerializeField] string buildIconTag = "Depo";
    Ray rayMouse;
    RaycastHit mouseHit;
    private bool isRotated = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var mousePosition = Input.mousePosition;
        //print(mousePosition);
        rayMouse = Camera.main.ScreenPointToRay(mousePosition);
        if (Input.GetKeyDown(KeyCode.R))
        {
            tracker.GetComponent<RayCube>().IsRotated = true;
            isRotated = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            tracker.GetComponent<RayCube>().IsRotated = false;
            isRotated = false;
        }
        if(Physics.Raycast(rayMouse, out mouseHit, Mathf.Infinity))
        {
                //print(new Vector3(0f, 0f, mousePosition.z));
            
                if (mouseHit.transform.tag == "BuildArea" || mouseHit.transform.tag == buildIconTag)
                {
                    var posX = 0f;
                    var posZ = 0f;
                    if(isRotated)
                    {
                        posX = mouseHit.point.x;
                        posZ = tracker.transform.position.z;
                    }    
                    else
                    {
                        posX = tracker.transform.position.x;
                        posZ = mouseHit.point.z;
                    }
                    //tracker.transform.position = new Vector3(tracker.transform.position.x, 0f, mouseHit.point.z);
                    tracker.transform.position = new Vector3(posX, 0f, posZ);
                }
            
         }
    }
}
