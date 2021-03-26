using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCube : MonoBehaviour
{
    // NOTE: Add to main project code
    // Start is called before the first frame update
    [SerializeField] GameObject startingCube;
    [SerializeField] string targetTag = "Wall";
    [SerializeField] int maxIndex = 1;
    [SerializeField] int cubeIndex = 0;
    [SerializeField] float spacing = 0f;
    bool valuesSet = false;
    private bool isRotated = false;
    private Vector3 stoppedPos;
    public bool IsRotated { set { isRotated = value; } get { return isRotated; }}
    // Ray rayMouse;
    // RaycastHit mouseHit;
    void Start()
    {
        //print(transform.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            RaycastHit hit;
            var directionZ = Vector3.forward;
            var directionX = Vector3.right;
            // Checking from the Z axis
            //transform.position.z = Input.mousePosition.z;
            if (IsRotated)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(directionX), out hit, Mathf.Infinity))
                {
                    if (hit.transform.tag == targetTag)
                    {
                       print("We found a " + hit.transform.gameObject.tag + ", distance: " + Mathf.Round(hit.distance));
                        
                    
                        var dx = Mathf.Abs(Mathf.Round(transform.position.x - stoppedPos.x));
                        //print("dz: " + dz);
                        if (valuesSet && dx > 0f)
                        {
                            valuesSet = false;
                        }
                        if (!valuesSet)
                        {
                            maxIndex = (int)(Mathf.Round(hit.distance) / transform.localScale.z);
                            valuesSet = true;
                            stoppedPos = transform.position;
                        }

                        if (cubeIndex >= maxIndex)
                        {
                            cubeIndex = 0;
                            return;
                        }
                        if (Mathf.Round(hit.distance) > 1)
                        {
                            var sobj = Instantiate(startingCube, hit.point - new Vector3(spacing, 0f, 0f), startingCube.transform.rotation);
                            sobj.name = sobj.name + "_Holo";
                            cubeIndex++;
                        }
                    }
                }
                else if (Physics.Raycast(transform.position, transform.TransformDirection(-directionX), out hit, Mathf.Infinity))
                {
                    if (hit.transform.tag == targetTag)
                    {
                       print("We found a " + hit.transform.gameObject.tag + ", distance: " + Mathf.Round(hit.distance));
                        
                    
                        var dx = Mathf.Abs(Mathf.Round(transform.position.x - stoppedPos.x));
                        //print("dz: " + dz);
                        if (valuesSet && dx > 0f)
                        {
                            valuesSet = false;
                        }
                        if (!valuesSet)
                        {
                            maxIndex = (int)(Mathf.Round(hit.distance) / transform.localScale.z);
                            valuesSet = true;
                            stoppedPos = transform.position;
                        }

                        if (cubeIndex >= maxIndex)
                        {
                            cubeIndex = 0;
                            return;
                        }
                        if (Mathf.Round(hit.distance) > 1)
                        {
                            var sobj = Instantiate(startingCube, hit.point + new Vector3(spacing, 0f, 0f), startingCube.transform.rotation);
                            sobj.name = sobj.name + "_Holo";
                            cubeIndex++;
                        }
                    }
                }
            }
            else 
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(directionZ), out hit, Mathf.Infinity))
                {
                
                    if (hit.transform.tag == targetTag)
                    {
                       // print("We found a " + hit.transform.gameObject.tag + ", distance: " + Mathf.Round(hit.distance));
                        
                    
                        var dz = Mathf.Abs(Mathf.Round(transform.position.z - stoppedPos.z));
                        //print("dz: " + dz);
                        if (valuesSet && dz > 0f)
                        {
                            valuesSet = false;
                        }
                        if (!valuesSet)
                        {
                            maxIndex = (int)(Mathf.Round(hit.distance) / transform.localScale.z);
                            valuesSet = true;
                            stoppedPos = transform.position;
                        }

                        if (cubeIndex >= maxIndex)
                        {
                            cubeIndex = 0;
                            return;
                        }
                        if (Mathf.Round(hit.distance) > 1)
                        {
                            var sobj = Instantiate(startingCube, hit.point - new Vector3(0f, 0f, spacing), startingCube.transform.rotation);
                            sobj.name = sobj.name + "_Holo";
                            cubeIndex++;
                        }
                    }
                }
                else if (Physics.Raycast(transform.position, transform.TransformDirection(-directionZ), out hit, Mathf.Infinity))
                {
                    if (hit.transform.tag == targetTag)
                    {
                       // print("We found a " + hit.transform.gameObject.tag + ", distance: " + Mathf.Round(hit.distance));
                        
                    
                        var dz = Mathf.Abs(Mathf.Round(transform.position.z - stoppedPos.z));
                        //print("dz: " + dz);
                        if (valuesSet && dz > 0f)
                        {
                            valuesSet = false;
                        }
                        if (!valuesSet)
                        {
                            maxIndex = (int)(Mathf.Round(hit.distance) / transform.localScale.z);
                            valuesSet = true;
                            stoppedPos = transform.position;
                        }

                        if (cubeIndex >= maxIndex)
                        {
                            cubeIndex = 0;
                            return;
                        }
                        if (Mathf.Round(hit.distance) > 1)
                        {
                            var sobj = Instantiate(startingCube, hit.point + new Vector3(0f, 0f, spacing), startingCube.transform.rotation);
                            sobj.name = sobj.name + "_Holo";
                            cubeIndex++;
                        }
                    }
                }
            
            
         }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "PlayerWall" && other.gameObject.name.Contains("_Holo"))
        {
            Destroy(other.gameObject);
        }
    }
  
}
