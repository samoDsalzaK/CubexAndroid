using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WallDestroy : MonoBehaviour
{
    // NOTE: Remove not required code
    //Make more methods
    [Header("Configuration parameters")]
    [SerializeField] GameObject notificationIcon;
    [SerializeField] GameObject errorWindow;
    [SerializeField] GameObject buildingArea;
    [SerializeField] Text errorText;    
    [SerializeField] Button destroyButton;    
    [SerializeField] string gameObjectTag = "PlayerWall";
    private bool destroyMode = false;
    private Base playerBase;
    Ray ray;
    RaycastHit hit;
    private GameObject spawnedWall;
    // Button method to start destruction mode
    public void enterDestroyBuildingMode()
    {
        buildingArea.SetActive(true);
        notificationIcon.SetActive(true);
        destroyButton.interactable = false;
        destroyMode = true;
    }

    void Start()
    {
        playerBase = GetComponent<Base>();
        notificationIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            notificationIcon.SetActive(false);
            destroyMode = false;
            destroyButton.interactable = true;
            buildingArea.SetActive(false);
        }
        if (destroyMode)
        {     
            // Hovering logic
            var mousePosition = Input.mousePosition;        
            notificationIcon.transform.position = new Vector3(mousePosition.x, mousePosition.y, notificationIcon.transform.position.z);
            ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                
                if (hit.transform.tag == gameObjectTag)
                {
                   // print(hit.transform.tag);
                    spawnedWall = hit.transform.gameObject;
                    if (spawnedWall)
                    {
                        var wallRend = spawnedWall.GetComponent<MeshRenderer>();
                        if (wallRend)
                        {
                            //var wallCheck = spawnedWall.GetComponent<WallBuildCheck>();
                            wallRend.material.color = new Color(255f, 0, 0);
                        }
                  }
                }
                else
                {
                    if (spawnedWall)
                    {
                        var wallRend = spawnedWall.GetComponent<MeshRenderer>();
                        if (wallRend)
                        {
                            var wallCheck = spawnedWall.GetComponent<WallBuildCheck>();
                            wallRend.material.color = wallCheck.OriginalColor;
                        }
                        spawnedWall = null;
                    }
                }
            }
            // Raycast destroy building logic
            if(Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0) {
                if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.transform.tag == gameObjectTag)
                    {
                        hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = new Color(255f, 0f, 0f);
                        destroyMode = false;
                        notificationIcon.SetActive(false);
                        destroyButton.interactable = true;
                        if (playerBase)
                        {
                            var wall = hit.transform.gameObject.GetComponent<WallBuildCheck>();
                            playerBase.setEnergonAmount(playerBase.getEnergonAmount() + wall.EnergonPrice);
                            playerBase.setCreditsAmount(playerBase.getCreditsAmount() + wall.CreditsPrice);
                        }
                        var gobj = hit.transform.gameObject; 
                        buildingArea.SetActive(false);   
                        playerBase.GetComponent<setFidexAmountOfStructures>().changePlayerWallsAmountInLevel = playerBase.GetComponent<setFidexAmountOfStructures>().changePlayerWallsAmountInLevel - 1;                    
                        Destroy(gobj.transform.parent.gameObject);
                    }
                    else
                    {
                        errorWindow.SetActive(true);
                        destroyButton.interactable = true;
                        errorText.text = "No objects selected to destroy!";
                        destroyMode = false;
                        notificationIcon.SetActive(false);
                        buildingArea.SetActive(false);     
                    }
                }
            }
       
        }
    }
   
}
