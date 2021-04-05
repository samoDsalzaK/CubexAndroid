using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchLevel : MonoBehaviour
{
    [SerializeField] Button researchUpgradeBtn;
    [SerializeField] Text researchUpgradeBtnText;
    [SerializeField] Text researchCenterLevelText;
    [SerializeField] Text researchCenterHPText;
    [SerializeField] GameObject researchCenterUpgradePanel;
    private Base playerbase;
    private HealthOfRegBuilding researchHealth;
    [SerializeField] ResearchConf oBGResearch;
	PanelManager panelManager;
	createAnimatedPopUp animatedPopUps;
    // Start is called before the first frame update
    void Start()
    {
		if(FindObjectOfType<Base>() == null)
		{
			return;
		}
		else
		{
			playerbase = FindObjectOfType<Base>();
		}
		researchCenterUpgradePanel.SetActive(false);
		researchHealth = GetComponent<HealthOfRegBuilding>();
		researchHealth.setHealthOfStructureOriginal(oBGResearch.getBuildingResearchHealth());
		researchHealth.setHealth(oBGResearch.getBuildingResearchHealth());
	 	panelManager = GetComponent<PanelManager>();
		animatedPopUps = GetComponent<createAnimatedPopUp>();
    }

    // Update is called once per frame
    void Update()
    {
        if(researchHealth.getHealth() <= 0)
          {
            playerbase.setResearchCentreUnitAmount(playerbase.getResearchCentreUnitAmount() - 1);
            Destroy(gameObject);
          }
        researchCenterLevelText.text = "Research Center Level : " + oBGResearch.getBuildingResearchLevel(); 
        researchUpgradeBtnText.text = "Upgrade Research Center to level " + (oBGResearch.getBuildingResearchLevel() + 1) + "\n" + "(" + oBGResearch.getMinNeededCreditsAmountForResearch() + " credits & " + oBGResearch.getMinNeededEnergonAmountForResearch() + " energon)"; 
        researchCenterHPText.text = "Health " + researchHealth.getHealth() + " / " + researchHealth.getHealthOfStructureOriginal();
        // kai zaidejas pasiekia max leveli tai tekstas pasikeicia i si
        if(oBGResearch.getBuildingResearchLevel() == oBGResearch.getMaxBuildingResearchLevel())
        {
          researchUpgradeBtnText.text = "You have reached max level";
        }
        // research centras visa laika tikrina savo hp ir jeigu nuzudo, tai pasikeicia reiksmes  
        
    }
    void OnMouseDown()
    { 
      	// check for active panels in this building hierarchy if yes do not trigger on mouse click
        var status = panelManager.checkForActivePanels();
        if (status){
            return;
        }  
        else{
            // set main window
			researchCenterUpgradePanel.SetActive(true);  
			researchCenterLevelText.text = "Research Center Level : " + oBGResearch.getBuildingResearchLevel();   
			researchUpgradeBtnText.text = "Upgrade Research Center to level " + (oBGResearch.getBuildingResearchLevel()+ 1) + "\n" + "(" + oBGResearch.getMinNeededCreditsAmountForResearch() + " credits & " + oBGResearch.getMinNeededEnergonAmountForResearch()  + " energon)"; 
            // deactivate other building panels
            panelManager.changeStatusOfAllPanels();
        }
    }
    public void upgradeBaseResearchLevel() // cia apskritai visos bazes, o ne playerio tawn holo!
    {
       if(oBGResearch.getBuildingResearchLevel() == oBGResearch.getMaxBuildingResearchLevel())
        {
          return;
        }
       if(oBGResearch.getBuildingResearchLevel() == playerbase.getPlayerBaseLevel())
        {
          playerbase.setErrorStateForPlayerBase(true);
          researchCenterUpgradePanel.SetActive(false);
          return;
        }
       if(playerbase.getCreditsAmount() < oBGResearch.getMinNeededCreditsAmountForResearch() || playerbase.getEnergonAmount() < oBGResearch.getMinNeededEnergonAmountForResearch())
        {
          playerbase.setResourceAMountScreenStateForUpgrade(true);
          return;
        } 
        
      oBGResearch.setBuildingResearchLevel(oBGResearch.getBuildingResearchLevel() + 1); // scriptable object change value
	  animatedPopUps = GetComponent<createAnimatedPopUp>();
	  animatedPopUps.createDecreaseCreditsPopUp(oBGResearch.getMinNeededCreditsAmountForResearch()); // creating pop ups
	  animatedPopUps.createDecreaseEnergonPopUp(oBGResearch.getMinNeededEnergonAmountForResearch(),2); // creating pop ups
      playerbase.setCreditsAmount(playerbase.getCreditsAmount() - oBGResearch.getMinNeededCreditsAmountForResearch());
      playerbase.setEnergonAmount(playerbase.getEnergonAmount() - oBGResearch.getMinNeededEnergonAmountForResearch());
      researchHealth.setHealthOfStructureOriginal(researchHealth.getHealthOfStructureOriginal() + oBGResearch.getUpgradeBuildingResearchLevelHP());
      oBGResearch.setBuildingResearchHealth(oBGResearch.getBuildingResearchHealth() + oBGResearch.getUpgradeBuildingResearchLevelHP());
      oBGResearch.setMinNeededEnergonAmountForResearch(oBGResearch.getMinNeededEnergonAmountForResearch() + 20);
      oBGResearch.setMinNeededCreditsAmountForResearch(oBGResearch.getMinNeededCreditsAmountForResearch() + 10);
      oBGResearch.setUpgradeBuildingResearchLevelHP(oBGResearch.getUpgradeBuildingResearchLevelHP() + ((oBGResearch.getBuildingResearchLevel() + 1))*10);
      var playerScorePoints = FindObjectOfType<GameSession>();

      if(playerScorePoints != null)
      {
        playerScorePoints.AddPlayerScorePoints(oBGResearch.getPlayerScoreEarned()); // for each upgrade player gets points.
        oBGResearch.setPlayerScoreEarned(oBGResearch.getPlayerScoreEarned());
      }
    }
    public int getBaseResearchLevel()
    {
      return oBGResearch.getBuildingResearchLevel();
    }
}
