using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject panel;

    public void ShowPanel()
    {
        
        AudioManager.Instance.PlayButtonClickSE();
        panel.SetActive(true);
    }

    public void HidePanel()
    {
        AudioManager.Instance.PlayButtonClickSE();
        panel.SetActive(false);
    }
}

