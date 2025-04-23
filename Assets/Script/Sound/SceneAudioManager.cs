using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class SceneAudioManager : MonoBehaviour
{
    public static SceneAudioManager Instance { get; private set; }
    public float stageFadeOutDuration = 2f;
    public float stageFadeInDuration = 2f;

    private static string previousScene = "";

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
        GameSaveData data = SaveManager.Instance.LoadGame();
        int gsd = data != null ? data.masterCase : 0;

        if (scene.name == "TitleScene")
        {
            if (string.IsNullOrEmpty(previousScene) ||
                previousScene == "Stage1" || previousScene == "Stage2" || previousScene == "Stage3")
            {
                GlobalAudioManager.Instance.StopAllMusic();
                GlobalAudioManager.Instance.PlayMainMenuMusic();
            }
        }
        else if (GameManager.isContinue && gsd == 4 &&
                (scene.name == "Stage1" || scene.name == "Stage2" || scene.name == "Stage3"))
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.bossMusic, GlobalAudioManager.Instance.bossVolume);
        }
        else if (scene.name == "Stage1")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.stage1Music, GlobalAudioManager.Instance.stage1Volume);
            Debug.Log("播放 Stage1 音樂");
        }
        else if (scene.name == "Stage2")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.stage2Music, GlobalAudioManager.Instance.stage2Volume);
        }
        else if (scene.name == "Stage3")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(GlobalAudioManager.Instance.stage3Music, GlobalAudioManager.Instance.stage3Volume);
        }

        previousScene = scene.name;
        RegisterButtonListeners();
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

    //DeathUIController,WallMover,ContinueButtonController,StoryController,TitlesceneController,Boss_A
    public void FadeOutSceneMusic(float sec)
    {
        GlobalAudioManager.Instance.FadeOutMusic(sec);
    }

    //WallMover,Boss_A
    public void PlayStoryMusicWithFadeIn(float x)
    {
        GlobalAudioManager.Instance.PlayStoryMusicWithFadeIn(x);
    }

    //StoryController
    public IEnumerator FadeToBoss()
    {
        yield return GlobalAudioManager.Instance.CrossfadeToBossMusic();
    }
}
