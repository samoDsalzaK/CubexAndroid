using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Achievement : System.Object
{
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] int researchLevel;
    [SerializeField] int troopsAmount;
    [SerializeField] int workerAmount;
    [SerializeField] int achievementNumber;
    [SerializeField] bool isCompleted;
    [SerializeField] bool isDisplayed;
    [SerializeField] int creditsPrize;

    public string Name { get { return name; } }
    public string Descrption { get { return description; } }
    public int ResearchLevel { get { return researchLevel; } }
    public int TroopsAmount { get { return troopsAmount; } }
    public int WorkerAmount { get { return workerAmount; } }
    public int AchievementNumber { get { return achievementNumber; } }
    public bool IsCompleted { get { return isCompleted; } set { isCompleted = value; } }
    public bool IsDisplayed { get { return isDisplayed; } set { isDisplayed = value; }}
    public int CreditsPrize { get { return creditsPrize; } }
}
[CreateAssetMenu(fileName = "AchievementsData", menuName = "Achievements ", order = 0)]
public class AchievementData : ScriptableObject
{
    [SerializeField] List<Achievement> achievementsList;
    public List<Achievement> AchievementsList { get { return achievementsList; } }

}






