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
        ShowStep(step);
        PlayerController.OnStep += HandlePlayerStep; // 监听玩家移动
    }

    private void ShowStep(int index)
    {
        tutorialPanel.SetActive(true);

        switch (index)
        {
            case 0:
                tutorialText.text = "Welcome to the game! Press W/A/S/D to move one step.";
                break;
            case 1:
                tutorialText.text = "Great! You learned how to move! Now try pressing space to set fire 🔥";
                break;
            case 2:
                tutorialText.text = "Perfect! You've mastered the basics. Good luck!";
                break;
            default:
                tutorialPanel.SetActive(false); // 隐藏提示
                break;
        }
    }

    private void HandlePlayerStep()
    {
        if (step == 0)
        {
            step++;
            ShowStep(step);
        }
    }

    public void OnPlayerUsedFire()
    {
        if (step == 1)
        {
            step++;
            ShowStep(step);
        }
    }
}
