using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class Hero{
//     private string heroType;
//     private int hp;
//     private int shield;
//     private float movementSpeed;
//     public int Hp { set {hp = value;} get { return hp; }}
//     public int Shield { set {shield = value;} get { return shield; }}
//     public string HeroType { set { heroType = value; } get { return heroType; }}

// }
// class Defender : Hero{

// }
// NOTE: Add index to spawned troop names from barracks
// NOTE:  Fix spherecast detection system
[System.Serializable]
public class UpgradeTask : System.Object //Required for managing current upgraded units
{
    [SerializeField] GameObject unit;
    [SerializeField] bool isUpgraded;

    public UpgradeTask(GameObject g)
    {
        this.unit = g;
    }

    public GameObject Unit { set { unit = value; } get {return unit;}}
    public bool IsUpgraded { set { isUpgraded = value; } get {return isUpgraded;}}
}
public class HeroUnit : MonoBehaviour
{
    [Header("Object detection")]
    [SerializeField] float scannerRadius = 10f;
    [Header("General param")]
    [SerializeField] string heroType;
    [SerializeField] int hp;
    [SerializeField] int shield;
    [SerializeField] float movementSpeed;
    [SerializeField] GameObject heroToolbar;
    //For debuging
    [SerializeField] List<UpgradeTask> upgradeTasks = new List<UpgradeTask>();
    public int Hp { set {hp = value;} get { return hp; }}
    public int Shield { set {shield = value;} get { return shield; }}
    public string HeroType { set { heroType = value; } get { return heroType; }}
    public float MovementSpeed { set {movementSpeed = value;} get { return movementSpeed; }}

    private void Start() {
        
    }
    private void Update() {

        // Ally detection system
        RaycastHit hit;

        Vector3 p1 = transform.position;
        float distanceToObstacle = 0;
        detectionSphere(transform.position, scannerRadius); //Scanner to see near by player troops

        if (Input.GetMouseButtonDown(0))
        {
            heroToolbar.SetActive(true);
        }
        // if (Physics.SphereCast(p1, scannerRadius / 2, transform.forward, out hit, scannerRadius))
        // {
        //     distanceToObstacle = hit.distance;
        //     print("Object detected: " + hit.collider.gameObject.name + ", " + hit.collider.gameObject.tag + " distance to it is " + distanceToObstacle);
        //     if (!isInUList(hit.collider.gameObject.name) && (hit.collider.tag == "Unit" && Mathf.Round(distanceToObstacle) < 4f))
        //     {
        //         upgradeTasks.Add(new UpgradeTask(hit.collider.gameObject));
        //     }
        // }
        // if (Physics.SphereCast(p1, scannerRadius, -transform.forward, out hit, scannerRadius))
        // {
        //     distanceToObstacle = hit.distance;
        //     print("Object detected: " + hit.collider.gameObject.name + " distance to it is " + distanceToObstacle);
        // }
    }
    private void detectionSphere(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
           var detectedDis = Vector3.Distance(hitCollider.gameObject.transform.position, transform.position);
           print("Detected: " + hitCollider.gameObject.name + " at distance: " + detectedDis);
           if (!isInUList(hitCollider.gameObject.name) && (hitCollider.tag == "Unit" && hitCollider.gameObject.name.Contains("Troop")))
                 upgradeTasks.Add(new UpgradeTask(hitCollider.gameObject));
        }
    }
    private bool isInUList(string gname)
    {
        if (upgradeTasks == null)
            return false;

        foreach(var uItem in upgradeTasks)
        {
            if (uItem.Unit.name == gname)
                return true;
        }

        return false;
    }
    // private void OnTriggerEnter(Collider other) {
    //     switch(heroType)
    //     {
    //         case "defender":
    //             if (other.gameObject.tag == "Unit")
    //             {
    //                 upgradeTasks.Add(new UpgradeTask(other.gameObject));
    //             }
    //         break;
    //     }
        
  //  }
   
    
}
