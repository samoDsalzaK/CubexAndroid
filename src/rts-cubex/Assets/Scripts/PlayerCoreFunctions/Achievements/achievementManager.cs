using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class achievementManager : MonoBehaviour
{
    [SerializeField] GameObject achievementPanel;
    [SerializeField] Text achievementName;
    [SerializeField] Text achievementDescription;
    [SerializeField] AchievementData data;
    private List<Achievement> achievementsList;
    private Helper helperTools = new Helper();
    void Start()
    {
        achievementsList = data.AchievementsList;
    }
    void Update()
    {
        foreach (var a in achievementsList)
        {
            if (!a.IsCompleted)
            {
                switch (a.AchievementNumber)
                {
                    case 0:
                        var existingWorkerAmount = GameObject.FindGameObjectsWithTag("Worker");
                        if (existingWorkerAmount.Length > 0 && a.WorkerAmount == existingWorkerAmount.Length)
                        {
                            a.IsCompleted = true;
                            achievementName.text = a.Name;
                            achievementDescription.text = a.Descrption;
                            achievementPanel.SetActive(true);
                            var playerBase = FindObjectOfType<Base>();
                            if (playerBase != null)
                            {
                                if (playerBase.getCreditsAmount() + a.CreditsPrize >= playerBase.MaxBCredits)
                                {
                                    playerBase.setCreditsAmount(playerBase.getCreditsAmount() + playerBase.MaxBCredits - playerBase.getCreditsAmount());
                                    var popup = FindObjectOfType<createAnimatedPopUp>();
                                    popup.createAddCreditsPopUp(0);
                                }
                                else
                                {
                                    playerBase.setCreditsAmount(playerBase.getCreditsAmount() + a.CreditsPrize);
                                    var popup = FindObjectOfType<createAnimatedPopUp>();
                                    popup.createAddCreditsPopUp(a.CreditsPrize);
                                }
                            }
                        }
                        break;
                    case 1:
                        var existingTroopAmount = GameObject.FindGameObjectsWithTag("Unit");
                        if (existingTroopAmount.Length > 0 && a.TroopsAmount == existingTroopAmount.Length)
                        {
                            a.IsCompleted = true;
                            achievementName.text = a.Name;
                            achievementDescription.text = a.Descrption;
                            achievementPanel.SetActive(true);
                            var playerBase = FindObjectOfType<Base>();
                            if (playerBase != null)
                            {
                                if (playerBase.getCreditsAmount() + a.CreditsPrize >= playerBase.MaxBCredits)
                                {
                                    playerBase.setCreditsAmount(playerBase.getCreditsAmount() + playerBase.MaxBCredits - playerBase.getCreditsAmount());
                                    var popup = FindObjectOfType<createAnimatedPopUp>();
                                    popup.createAddCreditsPopUp(0);
                                }
                                else
                                {
                                    playerBase.setCreditsAmount(playerBase.getCreditsAmount() + a.CreditsPrize);
                                    var popup = FindObjectOfType<createAnimatedPopUp>();
                                    popup.createAddCreditsPopUp(a.CreditsPrize);
                                }
                            }
                        }
                        break;
                    case 2:
                        var researchLevel = GameObject.FindGameObjectWithTag("TroopsResearch");
                        if (researchLevel != null)
                        {
                            var child = helperTools.getChildGameObjectByName(researchLevel, "Research Troops center");
                            var researchLevelCount = child.GetComponent<Research>();
                            if (researchLevelCount != null)
                            {
                                if (researchLevelCount.OBGResearch.ResearchLevel == a.ResearchLevel)
                                {
                                    a.IsCompleted = true;
                                    achievementName.text = a.Name;
                                    achievementDescription.text = a.Descrption;
                                    achievementPanel.SetActive(true);
                                    var playerBase = FindObjectOfType<Base>();
                                    if (playerBase != null)
                                    {
                                        if (playerBase.getCreditsAmount() + a.CreditsPrize >= playerBase.MaxBCredits)
                                        {
                                            playerBase.setCreditsAmount(playerBase.getCreditsAmount() + playerBase.MaxBCredits - playerBase.getCreditsAmount());
                                            var popup = FindObjectOfType<createAnimatedPopUp>();
                                            popup.createAddCreditsPopUp(0);
                                        }
                                        else
                                        {
                                            playerBase.setCreditsAmount(playerBase.getCreditsAmount() + a.CreditsPrize);
                                            var popup = FindObjectOfType<createAnimatedPopUp>();
                                            popup.createAddCreditsPopUp(a.CreditsPrize);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Debug.Log("NERADOM RESEARCH");
                            }
                        }
                        break;
                }
            }
        }
    }
}
