using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Mime;
public class CubexWindowManager : MonoBehaviour
{
    [SerializeField] int levelindex;
    public int returnLevelIndex {get{return levelindex;} set{levelindex = value;}}
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelindex);
    }
    public void ResetGameSession()
    {
        FindObjectOfType<GameSession>().ResetGame();
    }
    public void RestartLevel(){
        // find current level and restart game
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }
}
