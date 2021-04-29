using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransportTroop : MonoBehaviour
{
    [SerializeField] bool canTransport;
    [SerializeField] bool pickedUp;
    [SerializeField] bool canUnload;
    [SerializeField] bool travelling;
    [SerializeField] bool transportMode;
    [SerializeField] GameObject troopToTransfer;
    [SerializeField] Vector3 destinationToUnload;
    [SerializeField] Button collectBtn;
    [SerializeField] GameObject stopCollectBtn;
    [SerializeField] Button unloadBtn;
    [SerializeField] GameObject bloomMenu;
    private int clickCount = 0;
    private move mv;
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        mv = GetComponent<move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
        {
            unloadBtn.interactable = true;
            collectBtn.interactable = false;
        }
        else
        {
            unloadBtn.interactable = false;
            collectBtn.interactable = true;
        }
        if (transportMode)
        {
            if (canTransport)
            {
                // mv.BloomWorking = true;
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                    {
                        if (hit.collider.tag == "Unit")
                        {
                            troopToTransfer = hit.transform.gameObject;
                            canTransport = false;
                            transportMode = false;
                        }
                    }
                }
            }
        }
        if (troopToTransfer && !pickedUp)
        {
            mv.Agent.destination = troopToTransfer.transform.position;
            var distance = Vector3.Distance(troopToTransfer.transform.position, transform.position);
            if (Mathf.Round(distance) <= 2)
            {
                troopToTransfer.SetActive(false);
                pickedUp = true;
                // canUnload = true;
                // mv.BloomWorking = false;
                stopCollectBtn.SetActive(false);
                collectBtn.gameObject.SetActive(true);
            }
        }
        if (canUnload && troopToTransfer)
        {
            // mv.BloomWorking = true;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("LvlMap"))
                    {
                        destinationToUnload = hit.point;
                        canUnload = false;
                        travelling = true;
                    }
                }
            }
        }
        if (travelling && destinationToUnload != Vector3.zero)
        {
            mv.Agent.destination = destinationToUnload;
            var distance = Vector3.Distance(destinationToUnload, transform.position);
            if (distance <= 0.5f)
            {
                // mv.BloomWorking = false;
                var troopSpawnee = Instantiate(troopToTransfer, destinationToUnload, troopToTransfer.transform.rotation);
                troopSpawnee.SetActive(true);
                Destroy(troopToTransfer);
                travelling = false;
                canTransport = true;
                pickedUp = false;
                destinationToUnload = Vector3.zero;
            }
        }
    }
    private void OnMouseDown() {
        
        if (clickCount < 1)
        {
            bloomMenu.SetActive(true);
            clickCount++;
        }
        else
        {
            bloomMenu.SetActive(false);
            clickCount = 0;
        }
    }
    public void setCanTransport(bool a)
    {
        canTransport = a;
    }
    public void setTransportMode(bool a)
    {
        transportMode = a;
    }
    public void setCanUnload(bool a)
    {
        canUnload = a;
    }
}