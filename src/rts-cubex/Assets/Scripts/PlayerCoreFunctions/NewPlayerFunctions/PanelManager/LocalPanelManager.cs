using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPanelManager : MonoBehaviour
{
    [SerializeField] GameObject[] localPanels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void deactivatePanels(){
        foreach(GameObject panel in localPanels){
            panel.SetActive(false);
        }
    }
}
