using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMarketManager : MonoBehaviour
{
    [Header("Configuration parameters for in game market manager")]
    [SerializeField] int minLevelNeededToUnlockMarket; // minimum palyer base level needed to unlock in game market btn 
    public int getMinLevelNeeded {get {return minLevelNeededToUnlockMarket;}}
    // playerbase type variable
    private Base playerbase;
    // setting range of values for market in game offers
    [SerializeField] int priceInEnergon;
    [SerializeField] int priceInCredits;
    [SerializeField] int getEnergonValue;
    [SerializeField] int getCreditsValue;
    [SerializeField] float[] increaseLevelTimeValues; // time specified in minutes
    [SerializeField] int[] inCreaseTroopsCapacityValues;
    public float[] getLevelTimeValues {get{return increaseLevelTimeValues;}}
    public int getPriceInEnergon {get {return priceInEnergon;}}
    public int getPriceInCredits {get {return priceInCredits;}}
    public int getBoughtEnergonValue {get {return getEnergonValue;}}
    public int getBoughtCreditsValue {get {return getCreditsValue;}}
    public int[] getTroopsCapacityValues {get {return inCreaseTroopsCapacityValues;}}

    [SerializeField] float[] refreshCoolDownTimers;

    [SerializeField] Text refreshMarketButtonText;

    InGameMarketButtonHandler inGameButtonHandler;

    GameSesionTime gameTime;

    createAnimatedPopUp animatedPopUps;
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
        gameTime = FindObjectOfType<GameSesionTime>();
        animatedPopUps = GetComponent<createAnimatedPopUp>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buyEnergon(int priceInCredits, int getEnergonValue, int buttonID){
        // player base to set new value of energon
        // decrease value of credits
        Debug.Log("Offer 1 bought");
        playerbase.setCreditsAmount(playerbase.getCreditsAmount() - priceInCredits); // decreasing credits amount
        playerbase.setEnergonAmount(playerbase.getEnergonAmount() + getEnergonValue); // increasing energon amount
        animatedPopUps.createDecreaseCreditsPopUp(priceInCredits);
        animatedPopUps.createAddEnergonPopUp(getEnergonValue);
        inGameButtonHandler.changeStateOfInGameMarketBtn(buttonID,1); // change button state to true
        inGameButtonHandler.setMarketOfferButtonText(buttonID);
        return;
    }

    public void buyCredits(int priceInEnergon, int getCreditsValue, int buttonID){
        // player base to set new value of credits
        // decrease value of energon
        Debug.Log("Offer 2 bought");
        playerbase.setEnergonAmount(playerbase.getEnergonAmount() - priceInEnergon); // decreasing energon amount
        playerbase.setCreditsAmount(playerbase.getCreditsAmount() + getCreditsValue); // increasing credits amount
        animatedPopUps.createDecreaseEnergonPopUp(priceInEnergon);
        animatedPopUps.createAddCreditsPopUp(getCreditsValue);
        inGameButtonHandler.changeStateOfInGameMarketBtn(buttonID,1);
        inGameButtonHandler.setMarketOfferButtonText(buttonID);
        return;
    }

    public void inCreaseLevelTime(int price, string type, float increaseValue, int buttonID){
        // find timesession
        Debug.Log("Offer 3 bought");
        gameTime = FindObjectOfType<GameSesionTime>();
        gameTime.addTime(increaseValue); // adding minutes to level time
        inGameButtonHandler.changeStateOfInGameMarketBtn(buttonID,1);
        inGameButtonHandler.setMarketOfferButtonText(buttonID);
        return;
    }

    public void inCreaseTroopsCapacity(int price, string type, int increaseValue, int buttonID){
        // playerbase increase troop capacity
        Debug.Log("Offer 4 bought");
        playerbase.setPlayerMaxTroopsAmount(playerbase.getPlayerMaxTroopsAmount() + increaseValue); // increasing player troops capacity
        inGameButtonHandler.changeStateOfInGameMarketBtn(buttonID,1);
        inGameButtonHandler.setMarketOfferButtonText(buttonID);
        return;
    }

    public IEnumerator StartCountdownRefresh(float timeStart)
    {
        //currCountdownValueSlot3 = timeStart;
        while (timeStart > 0)
            {
                if(timeStart >= 60.00f)
                {
                    refreshMarketButtonText.text = "Cool down :" + "\n" + (int)(timeStart / 60)+ " min " + (timeStart % 60).ToString() + " sec";
                }
                else{
                    refreshMarketButtonText.text = "Cool down :" + "\n" + (timeStart % 60).ToString() + " sec";
                }
                //Debug.Log("Countdown: " + timeStart);
                yield return new WaitForSeconds(1.0f);
                timeStart--;
            }
        inGameButtonHandler.unlockRefreshBtn();
        yield break;
    }

    // function to start market refresh button cool down timer 
    public void startCoolDown(int index){
        float time = refreshCoolDownTimers[index] * 60;
        StartCoroutine(StartCountdownRefresh(time));
    }
}
