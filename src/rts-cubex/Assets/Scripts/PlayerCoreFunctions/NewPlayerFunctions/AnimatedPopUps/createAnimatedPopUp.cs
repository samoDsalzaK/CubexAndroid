using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createAnimatedPopUp : MonoBehaviour
{
    /*[Header("Configuration parameters Animated Pop UPs v1")]
    [SerializeField] GameObject addCreditsPopUp, decreaseCreditsPopUp, addEnergonPopUp, decreaseEnergonPopUp, decreaseEnergonPopUpPosition1, addEnergonPopUpPosition1; // version 1
    [Header("Configuration parameters Animated Pop UPs v2")]
    [SerializeField] GameObject addCreditsPopUpPLCanvas, decreaseCreditsPopUpPLCanvas, addEnergonPopUpPLCanvas, decreaseEnergonPopUpPLCanvas; // version 2*/
    [Header("Configuration parameters Animated Pop UPs v3")]
    [SerializeField] GameObject addCreditsPopUpV3, decreaseCreditsPopUpV3, addEnergonPopUpV3, decreaseEnergonPopUpV3, addTimePopUpV3, addTroopsPopUpV3; // version 3
    // Start is called before the first frame update

    // addCreditsPopUp - done
    // decreaseEnergonPopUp - done
    void Start()
    {
        addCreditsPopUpV3.SetActive(false);
        decreaseCreditsPopUpV3.SetActive(false);
        addEnergonPopUpV3.SetActive(false);
        decreaseEnergonPopUpV3.SetActive(false);
        /*addTimePopUpV3.SetActive(false);
        addTroopsPopUpV3.SetActive(false);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // create adding credits pop up
    public void createAddCreditsPopUp(int creditsAmountToAdd){  
        // version 3
        Transform[] ts = addCreditsPopUpV3.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) {
            if(t.gameObject.GetComponent<Text>() != null)
            {
                t.gameObject.GetComponent<Text>().text = "+" + creditsAmountToAdd + " credits ↑ ";
            }
        }
        addCreditsPopUpV3.SetActive(true);
        StartCoroutine(creditsPopUpTimer(1));
        // version 2
        /*GameObject addCreditsPopUpObject = Instantiate(addCreditsPopUpPLCanvas, addCreditsPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
        Transform[] ts = addCreditsPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "+" + creditsAmountToAdd + " credits ↑ ";
                }
            }
        Destroy(addCreditsPopUpObject, 2f);*/
    }

    // create decreasing credits pop up
    public void createDecreaseCreditsPopUp(int creditsAmountToDecrease){
        // version 3
        Transform[] ts = decreaseCreditsPopUpV3.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) {
            if(t.gameObject.GetComponent<Text>() != null)
            {
                t.gameObject.GetComponent<Text>().text = "-" + creditsAmountToDecrease + " credits ↓ ";
            }
        }
        decreaseCreditsPopUpV3.SetActive(true);
        StartCoroutine(creditsPopUpTimer(2));
        // version 2
        /*GameObject decreaseCreditsPopUpObject = Instantiate(decreaseCreditsPopUpPLCanvas, decreaseCreditsPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
        Transform[] ts = decreaseCreditsPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "-" + creditsAmountToDecrease + " credits ↓ ";
                }
            }
        Destroy(decreaseCreditsPopUpObject, 2f);*/
    }

    // create adding energon pop up
    public void createAddEnergonPopUp(int energonAmountToAdd /*int position*/){
        // version 3
        Transform[] ts = addEnergonPopUpV3.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) {
            if(t.gameObject.GetComponent<Text>() != null)
            {
                t.gameObject.GetComponent<Text>().text = "+" + energonAmountToAdd + " energon ↑ ";
            }
        }
        addEnergonPopUpV3.SetActive(true);
        StartCoroutine(energonPopUpTimer(1));
        // version 2
        /*if (position == 1){*/
            /*GameObject addEnergonPopUpObject = Instantiate(addEnergonPopUpPLCanvas, addEnergonPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
            Transform[] ts = addEnergonPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "+" + energonAmountToAdd + " energon ↑ ";
                }
            }
            Destroy(addEnergonPopUpObject, 2f); */
        //}
        /*else if (position == 2){
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
        }*/
    }

    // create decreasing energon pop up
    public void createDecreaseEnergonPopUp(int energonAmountToDecrease /*int position*/){
        // version 3
        Transform[] ts = decreaseEnergonPopUpV3.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) {
            if(t.gameObject.GetComponent<Text>() != null)
            {
                t.gameObject.GetComponent<Text>().text = "-" + energonAmountToDecrease + " energon ↓ ";
            }
        }
        decreaseEnergonPopUpV3.SetActive(true);
        StartCoroutine(energonPopUpTimer(2));
        // version 2
        /*if (position == 1){*/
            /*GameObject decreaseEnergonPopUpObject = Instantiate(decreaseEnergonPopUpPLCanvas, decreaseEnergonPopUpPLCanvas.transform.position, Quaternion.identity) as GameObject;
            Transform[] ts = decreaseEnergonPopUpObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) {
                if(t.gameObject.GetComponent<Text>() != null)
                {
                    t.gameObject.GetComponent<Text>().text = "-" + energonAmountToDecrease + " energon ↓ ";
                }
            }
            Destroy(decreaseEnergonPopUpObject, 2f);*/
        //}
        /*else if (position == 2) {
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
        }*/
    }

    public void createAddTimePopUp(int timeAmountToAdd){
        //
        Transform[] ts = addEnergonPopUpV3.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) {
            if(t.gameObject.GetComponent<Text>() != null)
            {
                t.gameObject.GetComponent<Text>().text = "+" + timeAmountToAdd + " minutes ↑ ";
            }
        }
        addEnergonPopUpV3.SetActive(true);
        StartCoroutine(energonPopUpTimer(1));
    }

    public void createAddTroopsCapacityPopUp(int troopsAmountToAdd){
        //
        Transform[] ts = addEnergonPopUpV3.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) {
            if(t.gameObject.GetComponent<Text>() != null)
            {
                t.gameObject.GetComponent<Text>().text = "+" + troopsAmountToAdd + " troops ↑ ";
            }
        }
        addEnergonPopUpV3.SetActive(true);
        StartCoroutine(energonPopUpTimer(1));
    }

    public IEnumerator creditsPopUpTimer(int var){
        yield return new WaitForSeconds(1.3f);
        if(var == 1){
            addCreditsPopUpV3.SetActive(false);
        }
        else if (var == 2){
            decreaseCreditsPopUpV3.SetActive(false);
        }
        else{
            addCreditsPopUpV3.SetActive(false);
            decreaseCreditsPopUpV3.SetActive(false);
        }
    }

    public IEnumerator energonPopUpTimer(int var){
        yield return new WaitForSeconds(1.3f);
        if(var == 1){
            addCreditsPopUpV3.SetActive(false);
        }
        else if(var == 2){
            decreaseEnergonPopUpV3.SetActive(false);
        }
        else{
            addCreditsPopUpV3.SetActive(false);
            decreaseEnergonPopUpV3.SetActive(false);
        }
    }

    public IEnumerator timePopUpTimer(){
        yield return new WaitForSeconds(1.3f);
        addTimePopUpV3.SetActive(false);
    }

    public IEnumerator troopsPopUpTimer(){
        yield return new WaitForSeconds(1.3f);
        addTroopsPopUpV3.SetActive(false);
    }
}


