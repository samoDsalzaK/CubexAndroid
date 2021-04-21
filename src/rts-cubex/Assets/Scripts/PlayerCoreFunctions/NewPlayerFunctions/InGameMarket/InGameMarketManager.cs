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
    // setting range of values for market in game offers
    [SerializeField] int priceInEnergon;
    [SerializeField] int priceInCredits;
    [SerializeField] int getEnergonValue;
    [SerializeField] int getCreditsValue;
    [SerializeField] float[] increaseLevelTimeValues;
    [SerializeField] int[] inCreaseTroopsCapacityValues;
    public float[] getLevelTimeValues {get{return increaseLevelTimeValues;}}
    public int getPriceInEnergon {get {return priceInEnergon;}}
    public int getPriceInCredits {get {return priceInCredits;}}
    public int getBoughtEnergonValue {get {return getEnergonValue;}}
    public int getBoughtCreditsValue {get {return getCreditsValue;}}
    public int[] getTroopsCapacityValues {get {return inCreaseTroopsCapacityValues;}}

    InGameMarketButtonHandler inGameButtonHandler;
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
        inGameButtonHandler = GetComponent<InGameMarketButtonHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buyEnergon(int priceInCredits, int getEnergonValue, int buttonID){
        // player base to set new value of energon
        // decrease value of credits
        Debug.Log("Offer 1 bought");
        inGameButtonHandler.changeStateOfInGameMarketBtn(buttonID,1);
        inGameButtonHandler.setMarketOfferButtonText(buttonID);
        return;
    }

    public void buyCredits(int priceInEnergon, int getCreditsValue, int buttonID){
        // player base to set new value of credits
        // decrease value of energon
        Debug.Log("Offer 2 bought");
        inGameButtonHandler.changeStateOfInGameMarketBtn(buttonID,1);
        inGameButtonHandler.setMarketOfferButtonText(buttonID);
        return;
    }

    public void inCreaseLevelTime(int price, string type, float increaseValue, int buttonID){
        // find timesession
        Debug.Log("Offer 3 bought");
        inGameButtonHandler.changeStateOfInGameMarketBtn(buttonID,1);
        inGameButtonHandler.setMarketOfferButtonText(buttonID);
        return;
    }

    public void inCreaseTroopsCapacity(int price, string type, int increaseValue, int buttonID){
        // playerbase increase troop capacity
        Debug.Log("Offer 4 bought");
        inGameButtonHandler.changeStateOfInGameMarketBtn(buttonID,1);
        inGameButtonHandler.setMarketOfferButtonText(buttonID);
        return;
    }
}
// Cool down : 2 min 30 sec