using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuildCheck : MonoBehaviour
{
    [Header("Unit price")]
    [SerializeField] int energonPrice = 15;
    [SerializeField] int creditsPrice = 15;
    [SerializeField] bool spaceOccupied;
    [SerializeField] Material wallBuiltMat;
    public bool SpaceOccupied { get { return spaceOccupied; } set {spaceOccupied = value;} }
    private string detectedTag;
    [SerializeField] bool isBuilt;
    [SerializeField] bool isTemp; 
    public bool IsBuilt {get {return isBuilt;} set {isBuilt = value;}}
    private MeshRenderer rend;
    private Color originalColor;
    public Color OriginalColor {get {return originalColor;} set {originalColor = value;}}
    public bool IsTemp {get { return isTemp; } set { isTemp = value; }}
    public int EnergonPrice {get { return energonPrice; }}
    public int CreditsPrice {get { return creditsPrice; }}
    public Material WallBuiltMat {get {return wallBuiltMat; }}
    private void Start() {
        //IsBuilt = false;
        rend = GetComponent<MeshRenderer>();
        originalColor = rend.material.color;
        OriginalColor = originalColor;

       
    }  
    private void Update() {
        if(isBuilt && !isTemp)
        {
            rend.material = wallBuiltMat;
        }
    }
    private void OnTriggerEnter(Collider other) {
       // var rayBuilder = GetComponent<BuildRayField>();
        if (!other.gameObject.name.Contains("_s") && (other.gameObject.tag != "BuildArea" && !IsBuilt))
        {
            SpaceOccupied = true;
            if (rend != null && !IsBuilt)
                rend.material.color = new Color(255f, 0f, 0f, 0.25f);
        }  
    }
    private void OnTriggerStay(Collider other) {
        //print(other.gameObject.tag + ", K= " + other.gameObject.transform.position);
       // var rayBuilder = GetComponent<BuildRayField>();
        if (!other.gameObject.name.Contains("_s") && (other.gameObject.tag != "BuildArea" && !IsBuilt))
        {
            SpaceOccupied = true;
            if (rend != null && !IsBuilt)
                rend.material.color = new Color(255f, 0f, 0f, 0.25f);
        }       
        
    } 
    private void OnTriggerExit(Collider other) {
        if (SpaceOccupied)
        {
            rend.material.color = originalColor;
            SpaceOccupied = false;
        }
    }
}
