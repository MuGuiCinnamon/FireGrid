using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop = false;
    }

    [Header("Music Settings")]
    public float fadeDuration = 1f;
    public string initialBGM;

    [Header("Sound Lists")]
    public List<Sound> bgmList = new List<Sound>();
    public List<Sound> meList = new List<Sound>();
    public List<Sound> seList = new List<Sound>();

    private AudioSource bgmSource;
    private AudioSource meSource;
    private AudioSource seSource;
    private string currentBGM;
    private string nextBGM;
    private string nextScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void InitializeAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        seSource = gameObject.AddComponent<AudioSource>();
        meSource = gameObject.AddComponent<AudioSource>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!string.IsNullOrEmpty(initialBGM) && string.IsNullOrEmpty(currentBGM))
        {
            PlayBGM(initialBGM);
        }
        if (ScreenFade.Instance != null)
        {
            ScreenFade.Instance.FadeIn();
        }
    }

    #region BGM Methods
    public void PlayBGM(string bgmName, bool fadeIn = true)
    {
        Sound bgm = bgmList.Find(s => s.name == bgmName);
        if (bgm == null)
        {
            Debug.LogWarning($"BGM not found: {bgmName}");
            return;
        }

        currentBGM = bgmName;
        bgmSource.clip = bgm.clip;
        bgmSource.volume = fadeIn ? 0f : bgm.volume;
        bgmSource.Play();

        if (fadeIn)
        {
            StartCoroutine(FadeAudio(bgmSource, 0f, bgm.volume, fadeDuration));
        }
    }

    public void StopBGM(bool fadeOut = true)
    {
        if (fadeOut)
        {
            StartCoroutine(FadeAndStop(bgmSource, fadeDuration));
        }
        else
        {
            bgmSource.Stop();
        }
    }

    public void ChangeSceneWithFade(string sceneName, string bgmName = null)
    {
        nextScene = sceneName;
        nextBGM = bgmName;

        //StartCoroutine(FadeAndLoadScene(bgmSource, fadeDuration));
        if (ScreenFade.Instance != null)
        {
            Debug.Log("ScreenFade available. Starting fade out...");
            ScreenFade.Instance.FadeOut(() =>
            {
                Debug.Log("FadeOut complete. Now loading scene...");
                StartCoroutine(FadeAndLoadScene(bgmSource, fadeDuration));
            });
        }
        else
        {
            Debug.Log("ScreenFade is null. Loading scene directly...");
            StartCoroutine(FadeAndLoadScene(bgmSource, fadeDuration));
        }

    }
    #endregion

    #region SE Methods
    public void PlaySE(string seName)
    {
        Sound se = seList.Find(s => s.name == seName);
        if (se == null)
        {
            Debug.LogWarning($"SE not found: {seName}");
            return;
        }

        seSource.PlayOneShot(se.clip, se.volume);
    }
    public void PlayME(string meName)
    {
        Sound me = meList.Find(s => s.name == meName);
        if (me == null)
        {
            Debug.LogWarning($"ME not found: {meName}");
            return;
        }

        meSource.clip = me.clip;
        meSource.volume = me.volume;
        meSource.loop = me.loop;
        meSource.Play();
    }
    public void StopME()
    {
        meSource.Stop();
    }



    // 开始拖拽音效
    public void PlayDragStartSE()
    {
        PlaySE("DragStart");
    }

    // 结束拖拽音效
    public void PlayDragEndSE()
    {
        PlaySE("DragEnd");
    }

    // 按钮点击音效
    public void PlayButtonClickSE()
    {
        PlaySE("ButtonClick");
    }
    public void PlayButtonClickFailSE()
    {
        PlaySE("ButtonClickFail");
    }

    // 测验通过音乐
    public void PlayQuizSuccessME()
    {
        PlayME("QuizSuccessME");
    }

    // 测验失败音乐
    public void PlayQuizFailME()
    {
        PlayME("QuizFailME");
    }
    #endregion

    #region Utility Methods
    private IEnumerator FadeAudio(AudioSource audioSource, float startVolume, float endVolume, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }

        audioSource.volume = endVolume;
    }

    private IEnumerator FadeAndStop(AudioSource audioSource, float duration)
    {
        yield return StartCoroutine(FadeAudio(audioSource, audioSource.volume, 0f, duration));
        audioSource.Stop();
    }

    private IEnumerator FadeAndLoadScene(AudioSource audioSource, float duration)
    {
        yield return StartCoroutine(FadeAndStop(audioSource, duration));

        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }

        if (!string.IsNullOrEmpty(nextBGM))
        {
            PlayBGM(nextBGM);
        }
    }
    #endregion
}