using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TMP_Text tutorialText;

    private int step = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("✅ TutorialManager 已初始化！");

        ShowStep(step);
        PlayerController.OnStep += HandlePlayerStep; // 监听玩家移动
    }

    private void ShowStep(int index)
    {
        tutorialPanel.SetActive(true);

        switch (index)
        {
            case 0:
                tutorialText.text = "Press W/A/S/D to move one step. Press space to set fire";
                break;
            case 1:
                tutorialText.text = "Some of the things here cannot be burned, while some can, such as you and zombies";
                break;
            case 2:
                tutorialText.text = "Time will flow along with your pace. Ok. Now kill all the zombies. Don't die.";
                break;
            default:
                tutorialPanel.SetActive(false); // 隐藏提示
                break;
        }
    }

    private void HandlePlayerStep()
    {
        if (step >= 0)
        {
            step = step + 1 ;
            ShowStep(step);
        }
    }

    public void OnPlayerUsedFire()
    {
        Debug.Log("🔥 OnPlayerUsedFire() 被调用了");
        if (step == 1)
        {
            step++;
            ShowStep(step);
        }
    }
}
