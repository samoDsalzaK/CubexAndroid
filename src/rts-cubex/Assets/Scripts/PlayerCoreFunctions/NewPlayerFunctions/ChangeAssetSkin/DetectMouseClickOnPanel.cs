using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DetectMouseClickOnPanel : MonoBehaviour, IPointerClickHandler
{
    PanelManager panelManager;
    // Start is called before the first frame update
    void Start()
    {
        panelManager = GetComponent<PanelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        panelManager.deactivatePanels();
        //Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

}
