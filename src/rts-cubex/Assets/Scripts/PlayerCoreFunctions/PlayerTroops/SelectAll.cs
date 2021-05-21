using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAll : MonoBehaviour {
    [SerializeField] Text select;
    private int count;
    private void Update() {
        count=0;
        var spawnedUnits = GameObject.FindGameObjectsWithTag("Unit");
        int size = spawnedUnits.Length;
        // ClickOn[] unitNumber = FindObjectsOfType<ClickOn> ();
        // for (int j = 0; j < size; j++) {
        //     if(unitNumber[j].isSelected){
        //         count++;
        //     }
        // }
        for (int j = 0; j < size; j++) {
                if (spawnedUnits[j].tag != "usave")
                {
                    var click = spawnedUnits[j].GetComponent<ClickOn>();
                    if (click)
                    {
                        if(click.isSelected)
                        {
                            count++; 
                        }                       
                    }
                }
        }
        if(count == 0){
            select.text ="Select All";
        }
    }
    public void selectAllUnits () {
        count=0;
        //int size = FindObjectsOfType<ClickOn> ().Length;
        var spawnedUnits = GameObject.FindGameObjectsWithTag("Unit");
        int size = spawnedUnits.Length;
        //ClickOn[] unitNumber = FindObjectsOfType<ClickOn> ();
        if (select.text == "Select All") {
            for (int i = 0; i < size; i++) {
                if (spawnedUnits[i].tag != "usave")
                {
                    var click = spawnedUnits[i].GetComponent<ClickOn>();
                    if (click)
                    {
                        click.isSelected = true;
                        click.ClickMe ();
                        count++;
                    }
                }
            }
            select.text ="Deselect All";
        }
        else {
            for (int i = 0; i < size; i++) {
                if (spawnedUnits[i].tag != "usave")
                {
                    var click = spawnedUnits[i].GetComponent<ClickOn>();
                    if (click)
                    {
                        click.isSelected = false;
                        click.ClickMe ();
                        count++;
                    }
                }
            }
            select.text ="Select All";
        }
    }
}