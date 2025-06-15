using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject gameOverPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowGameOver()
    {
        gameOverPanel?.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏
    }
}

