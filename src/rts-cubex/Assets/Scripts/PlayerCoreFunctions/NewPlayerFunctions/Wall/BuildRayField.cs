using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildRayField : MonoBehaviour
{    
    [SerializeField] bool checkBuildings = false;
    [SerializeField] bool isStartBuilding = false;
    [SerializeField] bool isValuesSet = false;
    [SerializeField] GameObject sBuilding;
    [SerializeField] string targetTag = "Wall";
    [SerializeField] int maxIndex = 1;
    [SerializeField] int sbIndex = 0; //building current spawn amount
    [SerializeField] float spacing = 15f; //Distance betweet object center points
    [SerializeField] int allBEnergonPrice = 0;
    [SerializeField] int allBCreditsPrice = 0;
    public int AllBEnergonPrice { set {allBEnergonPrice = value;} get {return allBEnergonPrice;}}
    public int AllBCreditsPrice { set {allBCreditsPrice = value;} get {return allBCreditsPrice;}}
    private Vector3 stoppedPos;
    private bool isRotated = false;
    [SerializeField] float tOffset = 0f;
    public GameObject SBuilding { set {sBuilding = value;} get {return sBuilding;}}
    public bool CheckBuildings { set {checkBuildings = value;} get {return checkBuildings;}}
    public bool IsStartBuilding { set {isStartBuilding = value;} get {return isStartBuilding;}}
    public bool IsRotated { set {isRotated = value;} get {return isRotated;}}
    
    [SerializeField] GameObject _SBuilding;    
    private Helper helperTools = new Helper();
    private bool destroyHolos = false;
    // Debug data
    [SerializeField] List<GameObject> spawnedShiftBuilds;
    private Vector3 endBuildPos = new Vector3(0f, 0f, 0f);
    public List<GameObject> SpawnedShiftBuilds { set {spawnedShiftBuilds = value; } get {return spawnedShiftBuilds;}}
    
    private void Start() {
        tOffset = transform.localScale.z;
        spawnedShiftBuilds = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        if (CheckBuildings)
        {
            if(IsRotated)
            {
                scanForStartBuilding(Vector3.forward, IsRotated); //Right side building scanning
                scanForStartBuilding(-Vector3.forward, IsRotated); //Left side building scanning
            }
            else
            {
                scanForStartBuilding(Vector3.forward, IsRotated); //Right side building scanning
                scanForStartBuilding(-Vector3.forward, IsRotated); //Left side building scanning
            }
        }
        
    }
    private void scanForStartBuilding(Vector3 dirVector, bool isBuildingRotated)
    {
        RaycastHit hit;
           if (Physics.Raycast(transform.position, transform.TransformDirection(dirVector), out hit, Mathf.Infinity))
                    {                        
                        if (hit.transform.tag == "WallIcon")
                        {
                            var d = Mathf.Round(hit.distance);                           
                            
                            if (sBuilding && d - transform.localScale.z > 0f)
                            {
                                destroyHolos = false;
                                    if (isValuesSet && d - transform.localScale.z > 0f)
                                    {
                                        maxIndex = 0;
                                        isValuesSet = false;
                                    }
                                    if (!isValuesSet)
                                    {
                                        if (d - transform.localScale.z > 0)
                                        {    
                                            maxIndex++;
                                            //print("Max wall spawn amount: " + maxIndex);
                                            isValuesSet = true;
                                            stoppedPos = gameObject.transform.parent.gameObject.transform.position;
                                        }
                                    }
                                    if (sbIndex >= maxIndex)
                                    {
                                        sbIndex = 0;
                                        return;
                                    }
                                    if (d - transform.localScale.z > 0)
                                    {       
                                        var newSBPos = Vector3.zero;             
                                        if(isBuildingRotated)
                                        {
                                            if (dirVector == Vector3.forward)
                                                newSBPos = sBuilding.transform.position + new Vector3(0f, 0f, transform.localScale.z); 
                                            else
                                                newSBPos = sBuilding.transform.position - new Vector3(0f, 0f, transform.localScale.z); 
                                        }
                                        else
                                        {
                                            if (dirVector == Vector3.forward)
                                                newSBPos = sBuilding.transform.position - new Vector3(transform.localScale.z, 0f, 0f); 
                                            else
                                                newSBPos = sBuilding.transform.position + new Vector3(transform.localScale.z, 0f, 0f); 
                                        }
                                        endBuildPos += newSBPos;
                                        var nBuilding = Instantiate(sBuilding, newSBPos, sBuilding.transform.rotation);
                                        var nBModel = helperTools.getChildGameObjectByName(nBuilding, "Wall_1");
                                        nBModel.GetComponent<BuildRayField>().IsStartBuilding = false;
                                        nBModel.GetComponent<WallBuildCheck>().IsTemp = true;
                                        var cName = nBuilding.name.Replace("_s", "");
                                        nBuilding.name = cName;
                                        sBuilding = nBuilding;

                                        spawnedShiftBuilds.Add(nBuilding);
                                        // Counting spend fund amount
                                        allBEnergonPrice += nBModel.GetComponent<WallBuildCheck>().EnergonPrice;
                                        allBCreditsPrice += nBModel.GetComponent<WallBuildCheck>().CreditsPrice;
                                        sbIndex++;

                                    }
                                }
                               else
                               {
                                   if(destroyHolos)
                                   {
                                        sBuilding = _SBuilding;                                    
                                   }      
                               }
                        }    
                    }
    }
    private void OnTriggerEnter(Collider other) { //When aproacing placed holos, then delete them
        // if ((other.transform.position == transform.position) && (other.gameObject.GetComponent<WallBuildCheck>().IsTemp && !other.gameObject.name.Contains("_s")))
        // {
        //     Destroy(other.transform.parent.gameObject);
        // }
        //dx and dz are used to detect is the object moving on a specific axis
        var dx = Mathf.Abs(Mathf.Round(stoppedPos.x - transform.position.x));
        var dz = Mathf.Abs(Mathf.Round(transform.position.z - stoppedPos.z));
        //print("dx=" + dx + ", dz=" + dz);
        if ((dx > 0 || dz > 0) && CheckBuildings)
        {
            
            var buildRays = other.gameObject.GetComponent<BuildRayField>();
            if (buildRays)
            {
                if ((!other.gameObject.name.Contains("_s")  && !other.gameObject.name.Contains("_built")) && !buildRays.IsStartBuilding)
                {
                    destroyHolos = true;
                    spawnedShiftBuilds.Remove(other.transform.parent.gameObject);
                    Destroy(other.transform.parent.gameObject);
                    // if (IsRotated)
                    // {
                    //     if (transform.position == Vector3.forward)
                    //     {
                    //         endBuildPos = new Vector3(transform.position.x, transform.position.y, endBuildPos.z - transform.position.z);
                    //     }
                    //     else
                    //     {
                    //         endBuildPos = new Vector3(transform.position.x, transform.position.y, endBuildPos.z + transform.position.z);
                    //     }
                    // }
                    // else{
                    //     if (transform.position == Vector3.forward)
                    //     {                            
                    //         endBuildPos = new Vector3(endBuildPos.x + transform.position.x, transform.position.y, transform.position.z);
                    //     }
                    //     else
                    //     {
                    //         endBuildPos = new Vector3(endBuildPos.x - transform.position.x, transform.position.y, transform.position.z);
                    //     }
                    // }
                    // _SBuilding.transform.position = endBuildPos;
                    
                }
            }
        }
    }
    
    public void makeStartCopy() //Method used for making ini building copies 
    {
        if (SBuilding)
        {
            _SBuilding = SBuilding;
            if (_SBuilding) 
            {
                print("Copy saved! " + _SBuilding.name);
            }
        }
    }
}
