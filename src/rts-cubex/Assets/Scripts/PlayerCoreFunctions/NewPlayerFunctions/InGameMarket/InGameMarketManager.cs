using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMarketManager : MonoBehaviour
{
    [Header("Configuration parameters for in game market manager")]
    [SerializeField] GameObject inGameMarketPanel;
    [SerializeField] int minLevelNeededToUnlockMarket; // minimum palyer base level needed to unlock in game market btn 
    public int getMinLevelNeeded {get {return minLevelNeededToUnlockMarket;}}
    // playerbase type variable
    private Base playerbase;
    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectOfType<Base>() == null)
        {
            return;
        }
        else{
            playerbase = FindObjectOfType<Base>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buyEnergon(int priceInCredits, int getEnergonValue){
        // player base to set new value of energon
        // decrease value of credits
    }

    public void buyCredits(int priceInEnergon, int getCreditsValue){
        // player base to set new value of credits
        // decrease value of energon
    }

    public void inCreaseLevelTime(int price, string type, int increaseValue){
        // find timesession
    }

    public void inCreaseTroopsCapacity(int price, string type, int increaseValue){
        // playerbase increase troop capacity
    }

    public void buttonOfferGenerator(){
        // Cool down : 2 min 30 sec
    }
}
