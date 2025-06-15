using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (menuPanel != null)
            {
                bool isActive = menuPanel.activeSelf;
                menuPanel.SetActive(!isActive);

                // 可选：播放点击音效
                AudioManager.Instance?.PlayButtonClickSE();
            }
        }
    }
}

