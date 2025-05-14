using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneAudioManager : MonoBehaviour
{
    public static SceneAudioManager Instance { get; private set; }
    public float stageFadeOutDuration = 2f;
    public float stageFadeInDuration = 2f;

    // 儲存切場景之前的場景名稱
    public static string lastSceneName = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedMusicPlay(scene));
        RegisterButtonListeners();
    }

    private IEnumerator DelayedMusicPlay(Scene scene)
    {
        yield return null;

        GameSaveData data = SaveManager.Instance.LoadGame();
        int gsd = data != null ? data.masterCase : 0;

        string localSceneName = scene.name;

        if (localSceneName == "TitleScene")
        {
            // 只有從 Stage0~4 回到 TitleScene 才播放主選單音樂
            if (lastSceneName == "" || lastSceneName == "Stage0" ||
                lastSceneName == "Stage1" ||
                lastSceneName == "Stage2" ||
                lastSceneName == "Stage3" ||
                lastSceneName == "Stage4")
            {
                GlobalAudioManager.Instance.StopAllMusic();
                GlobalAudioManager.Instance.PlayMainMenuMusic();
            }
        }
        else if (localSceneName == "Stage0")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.stage0Music, GlobalAudioManager.Instance.stage0Volume);
        }
        else if (localSceneName == "Stage4")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.stage4Music, GlobalAudioManager.Instance.stage4Volume);
        }
        else if (GameManager.isContinue && gsd == 4 &&
                 (localSceneName == "Stage1" || localSceneName == "Stage2" || localSceneName == "Stage3"))
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.bossMusic, GlobalAudioManager.Instance.bossVolume);
        }
        else if (localSceneName == "Stage1")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.stage1Music, GlobalAudioManager.Instance.stage1Volume);
        }
        else if (localSceneName == "Stage2")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.stage2Music, GlobalAudioManager.Instance.stage2Volume);
        }
        else if (localSceneName == "Stage3")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.stage3Music, GlobalAudioManager.Instance.stage3Volume);
        }

        // 最後記錄這次進入的是哪個場景
        lastSceneName = localSceneName;
    }

    void RegisterButtonListeners()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveListener(PlayButtonSound);
            btn.onClick.AddListener(PlayButtonSound);
        }
    }

    void PlayButtonSound()
    {
        GlobalAudioManager.Instance.PlayButtonSound();
    }

    // DeathUIController, WallMover, ContinueButtonController, StoryController, TitlesceneController, Boss_A
    public void FadeOutSceneMusic(float sec)
    {
        GlobalAudioManager.Instance.FadeOutMusic(sec);
    }

    // WallMover, Boss_A
    public void PlayStoryMusicWithFadeIn(float x)
    {
        GlobalAudioManager.Instance.PlayStoryMusicWithFadeIn(x);
    }

    // StoryController
    public void FadeToBoss()
    {
        GlobalAudioManager.Instance.PlayBossMusicDirectly();
    }
}
