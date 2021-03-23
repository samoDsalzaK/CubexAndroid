using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Mime;
public class CubexWindowManager : MonoBehaviour
{
    [SerializeField] int levelindex;
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelindex);
    }
    public void ResetGameSession()
    {
        FindObjectOfType<GameSession>().ResetGame();
    }
}
