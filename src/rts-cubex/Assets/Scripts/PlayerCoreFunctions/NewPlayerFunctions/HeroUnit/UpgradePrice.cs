using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePrice : MonoBehaviour
{   
    
    [SerializeField] Text buttonText;
    [SerializeField] int energon = 0;
    [SerializeField] int credits = 0;

    public int Energon { get { return energon; }}
    public int Credits { get { return credits; }}

    private void Start() {
        buttonText.text += ("\n"+energon+" e, " + credits + " c");
    }
}
