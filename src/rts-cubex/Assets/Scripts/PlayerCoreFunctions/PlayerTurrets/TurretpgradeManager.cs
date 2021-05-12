using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurretpgradeManager : MonoBehaviour
{
    [SerializeField] Button turretUpgradeBtn;
    [SerializeField] Text turretUpgrageBtText;
    [SerializeField] Text turretLevelText;
    [SerializeField] Text turretDamagePoints; // sita plaiekame damagui.
    [SerializeField] Text turretHealthPoins;
    [SerializeField] GameObject turretUpgradePanel;
    [SerializeField] int minNeedCreditsAmountForTurretUpgrade; // pradines reiksmes
    [SerializeField] int minNeedEnergonAmountForTurretUpgrade; // pradines reiksmes
    [SerializeField] int turretUpgradeHPAmount; // pradine reiksme; 
    [SerializeField] int turretUpgradeDamageAmount; // pradine reiksme;
    [SerializeField] int turretLevel;
    [SerializeField] int turretmaxLevel; // maximalus ginklo levelis
    private Base playerbase;
    private TurretHealth turretHealth;
    private ResearchLevel researchLevel;
    private TurretFire turretFire;
    PanelManager panelManager;
    createAnimatedPopUp animatedPopUps;
    [SerializeField] GameObject selectionCanvas;
    changePosition changePos;
    // Start is called before the first frame update
    void Start()
    {
        researchLevel = FindObjectOfType<ResearchLevel>();
        if(FindObjectOfType<Base>() == null)
        {
           return;
        }
        else
        {
           playerbase = FindObjectOfType<Base>();
        }
        turretUpgradePanel.SetActive(false);
        turretHealth = GetComponent<TurretHealth>();
        turretFire = GetComponent<TurretFire>(); 
        panelManager = GetComponent<PanelManager>();
        animatedPopUps = playerbase.GetComponent<createAnimatedPopUp>();
        changePos = GetComponent<changePosition>();
    }
    // Update is called once per frame
    void Update()
    {
        if(turretHealth.getCurrentTurretHealth() <= 0)
        {
			playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTurretAmountInLevel = playerbase.GetComponent<setFidexAmountOfStructures>().changePlayerTurretAmountInLevel - 1;
			playerbase.GetComponent<setFidexAmountOfStructures>().changeBuildStructureButton(4);
            Destroy(gameObject);
        }
        turretUpgrageBtText.text = "Upgrade Turret to level " + (turretLevel + 1) + "\n" + "(" + minNeedCreditsAmountForTurretUpgrade + " credits & " + minNeedEnergonAmountForTurretUpgrade + " energon)";
        turretLevelText.text = "Turret Level : " + turretLevel;
        turretDamagePoints.text = "Damage Points : " + turretFire.getDamage();
        turretHealthPoins.text = "Health " + turretHealth.getCurrentTurretHealth() + " / " + turretHealth.getTurretHealth();
        researchLevel = FindObjectOfType<ResearchLevel>();
        if(turretLevel == turretmaxLevel) // kai pasieks max leveli tekstas ant mygtuko pasikeicia
        {
            turretUpgrageBtText.text = "You have reached max level";
        }
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
            selectionCanvas.SetActive(true);
            turretUpgradePanel.SetActive(true);    
            turretLevelText.text = "Turret Level : " + turretLevel;
            turretUpgrageBtText.text = "Upgrade Turret to level" + (turretLevel + 1) + "(" + minNeedCreditsAmountForTurretUpgrade + " credits & " + minNeedEnergonAmountForTurretUpgrade + " energon)";
            turretHealthPoins.text = "Health " + turretHealth.getCurrentTurretHealth() + " / " + turretHealth.getTurretHealth();
            turretDamagePoints.text = "Damage Points : " + turretFire.getDamage();
            // deactivate other building panels
            changePos.setDefaultValues();
            panelManager.changeStatusOfAllPanels();
        }
        
    }
    public void checkingUpgradeBtnLevel()
    { 
        if(researchLevel == null)
        {    
        playerbase.setErrorStateToBuildStructure(true);
        turretUpgradePanel.SetActive(false);
        return;
        }

        if(turretLevel == turretmaxLevel)
        {
           return; 
        }

        if(playerbase.getCreditsAmount() < minNeedCreditsAmountForTurretUpgrade || playerbase.getEnergonAmount() < minNeedEnergonAmountForTurretUpgrade)
        {
            playerbase.setResourceAMountScreenStateForUpgrade(true); 
            turretUpgradePanel.SetActive(false);
            return;
        }

      switch (researchLevel.getBaseResearchLevel())
      {
        case 1 :
        if(turretLevel < 2)
        {
        turretUpgradeBtn.interactable = true;
        turretUpgrade();
        }
        if(turretLevel >= 2)
        {
        playerbase.setErrorStateForResearchCenter(true);
        turretUpgradePanel.SetActive(false);
        }
        break;
        case 2 :
        if(turretLevel < 4)
        {
        turretUpgradeBtn.interactable = true;
        turretUpgrade();
        }
        if(turretLevel >= 4)
        {
        playerbase.setErrorStateForResearchCenter(true);
        turretUpgradePanel.SetActive(false);    
        }
        break;
        case 3 :
        if(turretLevel < 6)
        {
        turretUpgradeBtn.interactable = true;
        turretUpgrade();
        }
        if(turretLevel >= 6)
        {
        playerbase.setErrorStateForResearchCenter(true);
        turretUpgradePanel.SetActive(false);
        }
        break;
        case 4 :
        if(turretLevel < 8)
        {
        turretUpgradeBtn.interactable = true;
        turretUpgrade();
        }
        if(turretLevel >= 8)
        {
        playerbase.setErrorStateForResearchCenter(true);
        turretUpgradePanel.SetActive(false); 
        }
        break;
        case 5 :
        if(turretLevel < 10)
        {
        turretUpgradeBtn.interactable = true;
        turretUpgrade();
        }
        if(turretLevel >= 10)
        {
        turretUpgradeBtn.interactable  = true;   
        }
        // You have reched the max level of the turret upgrade. 
        break;
    }
    }
    private void turretUpgrade()
    {
    turretLevel++;
    animatedPopUps.createDecreaseCreditsPopUp(minNeedCreditsAmountForTurretUpgrade);
    animatedPopUps.createDecreaseEnergonPopUp(minNeedEnergonAmountForTurretUpgrade);
    playerbase.setCreditsAmount(playerbase.getCreditsAmount() - minNeedCreditsAmountForTurretUpgrade);
    playerbase.setEnergonAmount(playerbase.getEnergonAmount() - minNeedEnergonAmountForTurretUpgrade);
    turretHealth.setTurretHealth(turretHealth.getTurretHealth() + turretUpgradeHPAmount);
    minNeedCreditsAmountForTurretUpgrade += 15; // testinis variantas
    minNeedEnergonAmountForTurretUpgrade += 20; // testinis varinatas
    turretUpgradeHPAmount += 15;
    turretFire.setDamagePoints(turretFire.getDamage() + turretUpgradeDamageAmount);
    playerbase.GetComponent<PlayerScoring>().addScoreAfterStructureUpgrade("turret", turretLevel);
    }
    public int getTurretLevel()
    {
        return turretLevel;
    }
}
