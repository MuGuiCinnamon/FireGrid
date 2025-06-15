using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class AutoSceneLoader : MonoBehaviour
{
    public string targetSceneName;  // 可直接指定目标场景名
    public string targetBGMName;    // 可选：切换过去后的 BGM 名

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => 
        {
            string sceneNameToLoad = string.IsNullOrEmpty(targetSceneName)
                ? GetComponentInChildren<TMP_Text>()?.text
                : targetSceneName;
             Debug.Log("1Restart button clicked!");
            if (!string.IsNullOrEmpty(sceneNameToLoad))
            {
                AudioManager.Instance.PlayButtonClickSE();

                // 使用 AudioManager 的渐隐切换
                AudioManager.Instance.ChangeSceneWithFade(sceneNameToLoad, targetBGMName);
                Debug.Log("Restart button clicked!");
            }
            else
            {
                AudioManager.Instance.PlayButtonClickFailSE();
                Debug.LogWarning("Scene name is null or empty!");
            }
        });
    }
}
