using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createAnimatedPopUp : MonoBehaviour
{
    [SerializeField] GameObject addCreditsPopUp, decreaseCreditsPopUp, addEnergonPopUp, decreaseEnergonPopUp, decreaseEnergonPopUpPosition1, addEnergonPopUpPosition1;
    [SerializeField] GameObject addCreditsPopUpPLCanvas, decreaseCreditsPopUpPLCanvas, addEnergonPopUpPLCanvas, decreaseEnergonPopUpPLCanvas;
    // Start is called before the first frame update

    // addCreditsPopUp - done
    // decreaseEnergonPopUp - done
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // create adding credits pop up
    public void createAddCreditsPopUp(int creditsAmountToAdd){  
        GameObject addCreditsPopUpObject = Instantiate(addCreditsPopUpPLCanvas, addCreditsPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
        Transform[] ts = addCreditsPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "+" + creditsAmountToAdd + " credits ↑ ";
                }
            }
        Destroy(addCreditsPopUpObject, 2f);
    }

    // create decreasing credits pop up
    public void createDecreaseCreditsPopUp(int creditsAmountToDecrease){
        GameObject decreaseCreditsPopUpObject = Instantiate(decreaseCreditsPopUpPLCanvas, decreaseCreditsPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
        Transform[] ts = decreaseCreditsPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "-" + creditsAmountToDecrease + " credits ↓ ";
                }
            }
        Destroy(decreaseCreditsPopUpObject, 2f);
    }

    // create adding energon pop up
    public void createAddEnergonPopUp(int energonAmountToAdd, int position){
        if (position == 1){
            GameObject addEnergonPopUpObject = Instantiate(addEnergonPopUpPLCanvas, addEnergonPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
            Transform[] ts = addEnergonPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "+" + energonAmountToAdd + " energon ↑ ";
                }
            }
            Destroy(addEnergonPopUpObject, 2f); 
        }
        else if (position == 2){
            GameObject addEnergonPopUpObject = Instantiate(addEnergonPopUpPLCanvas, addEnergonPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
            Transform[] ts = addEnergonPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "+" + energonAmountToAdd + " energon ↑ ";
                }
            }
            Destroy(addEnergonPopUpObject, 2f); 
        }
        else{
            return;
        }
    }

    // create decreasing energon pop up
    public void createDecreaseEnergonPopUp(int energonAmountToDecrease, int position){
        if (position == 1){
            GameObject decreaseEnergonPopUpObject = Instantiate(decreaseEnergonPopUpPLCanvas, decreaseEnergonPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
            Transform[] ts = decreaseEnergonPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "-" + energonAmountToDecrease + " energon ↓ ";
                }
            }
            Destroy(decreaseEnergonPopUpObject, 2f);
        }
        else if (position == 2) {
            GameObject decreaseEnergonPopUpObject = Instantiate(decreaseEnergonPopUpPLCanvas, decreaseEnergonPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
            Transform[] ts = decreaseEnergonPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "-" + energonAmountToDecrease + " energon ↓ ";
                }
            }
            Destroy(decreaseEnergonPopUpObject, 2f);
        }
        else{
            return;
        }
    }
}


