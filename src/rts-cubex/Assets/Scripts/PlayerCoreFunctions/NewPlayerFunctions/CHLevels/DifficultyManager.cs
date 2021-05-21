using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DifficultyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider diffSlider;
    [SerializeField] Text difficultyInfo;
    [SerializeField] List<string> diffTexts;
    void Start()
    {
        diffSlider.value = (float)PlayerPrefs.GetInt("ChDiff");
    }

    // Update is called once per frame
    void Update()
    {
        var sliderValue = Mathf.Round(diffSlider.value);
        switch(sliderValue)
        {
            case 0:
               difficultyInfo.text = diffTexts[0];
               PlayerPrefs.SetInt("ChDiff", 0);
            break;
            case 1:
                difficultyInfo.text = diffTexts[1];
                PlayerPrefs.SetInt("ChDiff", 1);
            break;
            case 2:
                difficultyInfo.text = diffTexts[2];
                PlayerPrefs.SetInt("ChDiff", 2);
            break; 
        }
    }
}
