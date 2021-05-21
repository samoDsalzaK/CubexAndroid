using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour
{
    [SerializeField] AchievementData data;
    [SerializeField] GameObject achievementWindow;
    [SerializeField] GameObject card;
    [SerializeField] float yOffset = 60f;
    private Helper helperTools = new Helper();
    [SerializeField] float yOffsetTemp = 0f;
    [SerializeField] List<GameObject> cards;
    void Update()
    {
        foreach (var d in data.AchievementsList)
        {
            if (!d.IsDisplayed && d.IsCompleted)
            {
                var newCard = Instantiate(card, card.transform.position, card.transform.rotation);
                card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y - yOffset, card.transform.position.z);
                var achievementName = helperTools.getChildGameObjectByName(newCard, "Name");
                achievementName.GetComponent<Text>().text = d.Name;
                var achievementStatus = helperTools.getChildGameObjectByName(newCard, "Status");
                achievementStatus.GetComponent<Text>().text = "Completed";
                newCard.transform.parent = achievementWindow.transform;
                d.IsDisplayed = true;
                newCard.SetActive(true);
                yOffsetTemp = yOffset;
                cards.Add(newCard);
            }
        }
    }
    public void clearProgress()
    {
        foreach (var c in cards)
        {
            Destroy(c);
        }
        foreach (var d in data.AchievementsList)
        {
            d.IsCompleted = false;
            d.IsDisplayed = false;
        }
    }
}
