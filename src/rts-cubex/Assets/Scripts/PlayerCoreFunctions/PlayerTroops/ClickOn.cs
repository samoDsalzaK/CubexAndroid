using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOn : MonoBehaviour {
    [Header ("Unit color")]
    [SerializeField] private Material not_selected;
    [SerializeField] private Material selected;
    [SerializeField] GameObject[] figures;
    [SerializeField] public bool isSelected = false;
    void Start () {
        isSelected = false;
    }
    public void ClickMe () {
        if (isSelected == false) {
            if (figures.Length > 0)
            {
                for (int i = 0; i < figures.Length; i++) {
                    figures[i].GetComponent<MeshRenderer> ().material = not_selected;
                }
            }
        } else {
            if (figures.Length > 0)
            {
                for (int i = 0; i < figures.Length; i++) {
                    figures[i].GetComponent<MeshRenderer> ().material = selected;
                    
                }
                isSelected = true;
            }
        }
    }
    public bool GetSelected () {
        return isSelected;
    }
    public void SetSelected (bool sel) {
        isSelected = sel;
    }
}