using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeSkinManager : MonoBehaviour
{
    // skin ids
    // 1 - Starter
    // 2 - Pyro
    // 3 - Ice
    // 4 - Earth 
    // Start is called before the first frame update
    [Header("Configuration parameters for aset skin management")]
    // Pyro skin addtional boosts (in persantage)
    [SerializeField] int increaseTroopsDamage;
    [SerializeField] int decreaseTroopsHealth;
    // Ice skin addtional boosts (in persantage)
    [SerializeField] int decreaseBuildingTime;
    [SerializeField] int decreaseBuildingHealth;
    // Earth skin additional boosts (in persantage)
    [SerializeField] int increaseBuildingHealth;
    [SerializeField] int increaseBuildingTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // function for player skin management
    public void applyChosenSkin(GameObject gameObject){
        int selectedSkinValue = PlayerPrefs.GetInt("skinSelection"); // grab selected skin value
        //Debug.Log(PlayerPrefs.GetString("skinName"));
        switch(selectedSkinValue)
        {
            case 1: 
                // Starter skin changer
                Debug.Log("Default asset skin applied");
                break;
            case 2:
                // Pyro skin changer
                /*if (gameObject.tag == "Unit"){
                    if(gameObject.GetComponent<TroopAttack>() != null){
                        //gameObject.GetComponent<TroopAttack>();
                    }
                    else if (gameObject.GetComponent<TroopAttack>() != null){
                        //gameObject.GetComponent<TroopAttack>();
                    }
                }
                else{
                    Color changeColour = new Color32(255,0, 13, 100);
                    if(gameObject.GetComponent<Renderer>() != null){
                        gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
                    }
                    else{
                        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
                        foreach (Transform t in ts) {
                            if(t.gameObject.GetComponent<Renderer>() != null)
                            {
                                t.gameObject.GetComponent<Renderer>().material.SetColor("_Color", changeColour); // change building colour when it is seleted for position change
                            }
                        }
                    }
                }*/
                // check if object is troop with the tag "Unit"
                // other object just only change colour
                // +15 % troops damage
                // -10 % troops health
                Debug.Log("Pyro asset skin applied");
                break;
            case 3:
                /*if(gameObject.tag != "Unit"){

                }
                else{

                }*/
                // Ice skin changer
                // all change colour
                // to all buildings apply health changes
                // on worker take component and apply building time decrease
                //-10 % buiding time
                //-20 % building health
                Debug.Log("Ice asset skin applied");
                break;
            case 4:
                // Earth skin changer 
                //+15 % building health
                //+15 % building time
                Debug.Log("Earth asset skin applied");
                break;
            default:
                break;
        }
        return;
    }
}
