using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ArmyCamp : MonoBehaviour
{
    [SerializeField] int capacity = 4;
    [SerializeField] int occupied = 0;
    float startingTime = 0;
    [SerializeField] Text usage;
    [SerializeField] GameObject textBar;
    public int Capacity { get { return capacity; } }
    public int Occupied { set { occupied = value; } get { return occupied; } }
    private List<GameObject> group;
    [SerializeField] GameObject armyCampMainPanel;
    // panel Manager type variable
    PanelManager panelManager;
    [SerializeField] GameObject selectionCanvas;

    void Start()
    {
        group = new List<GameObject>();
        textBar.SetActive(false);
        panelManager = GetComponent<PanelManager>();
    }
    void Update()
    {
        var camps = GameObject.FindGameObjectsWithTag("Camp");
        if (camps.Length > 0)
        {
            foreach (var item in camps)
            {
                var army = item.GetComponent<ArmyCamp>();
                usage.text = Occupied.ToString() + "/" + Capacity.ToString();
            }
            if (Occupied == 0)
            {
                textBar.SetActive(false);
            }
        }
        if (group.Count > 0 && group.Count <= capacity)
        {
            foreach (var item in group)
            {
                if (item != null)
                {
                    var shielded = item.GetComponent<TroopHealth>();
                    if (shielded != null)
                    {
                        shielded.setCanShield(true);
                    }
                    if (shielded == null)
                    {
                        var shieldedH = item.GetComponent<HeavyHealth>();
                        if (shieldedH != null)
                        {
                            shieldedH.setCanShield(true);
                        }
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var troop = other.gameObject;
        if (troop.tag == "Unit")
        {
            if (troop.GetComponent<move>())
            {
                if (troop.GetComponent<move>().getOnItsWay())
                {
                    group.Add(troop);
                    if (occupied < Capacity)
                    {
                        occupied++;
                    }
                    textBar.SetActive(true);
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var troop = other.gameObject;
        if (troop.tag == "Unit")
        {
            if (troop.GetComponent<move>())
            {
                if (troop.GetComponent<move>().getOnItsWay())
                {
                    troop.GetComponent<TroopHealth>().setCanShield(false);
                    group.Remove(troop);
                    occupied--;
                }
            }
        }
    }

    private void OnMouseDown(){
        if(panelManager.checkIfWhereAreActivePanelsOnTheMap()){
            return;
        }
        else{
            // check for active panels in this building hierarchy if yes do not trigger on mouse click
            var status = panelManager.checkForActivePanels();
            if (status){
                return;
            }  
            else{
                // set main window
                selectionCanvas.SetActive(true);
                armyCampMainPanel.SetActive(true);
                // deactivate other building panels
                panelManager.changeStatusOfAllPanels();
            }
        }
    }
}