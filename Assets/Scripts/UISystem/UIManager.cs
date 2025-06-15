using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowGameOver()
    {
        gameOverPanel?.SetActive(true);
        //Time.timeScale = 0f; // 暂停游戏
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void ShowVictory()
    {
        victoryPanel.SetActive(true);
    }
}

