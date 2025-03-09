using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneAudioManager : MonoBehaviour
{
    public static SceneAudioManager Instance;
    public float titleFadeOutDuration = 3f;     
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
        if (scene.name == "TitleScene")
        {
            if (string.IsNullOrEmpty(previousScene) ||
                previousScene == "Stage1" || previousScene == "Stage2" || previousScene == "Stage3")
            {
                GlobalAudioManager.Instance.PlayMainMenuMusic();
            }
        }
        else if (scene.name == "Stage1")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(
            GlobalAudioManager.Instance.stage1Music,
            GlobalAudioManager.Instance.stage1Volume);
        }
        else if (scene.name == "Stage2")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(
            GlobalAudioManager.Instance.stage2Music,
            GlobalAudioManager.Instance.stage2Volume);
        }
        else if (scene.name == "Stage3")
        {
            GlobalAudioManager.Instance.PlayMusicDirectly(
            GlobalAudioManager.Instance.stage3Music,
            GlobalAudioManager.Instance.stage3Volume);
        }

        previousScene = scene.name;
        RegisterButtonListeners();
    }

    IEnumerator SwitchToMainMenuMusic()
    {
        float fadeOutDuration = titleFadeOutDuration;
        GlobalAudioManager.Instance.StopAllMusic();
        yield return new WaitForSeconds(fadeOutDuration);
        GlobalAudioManager.Instance.PlayMainMenuMusic();
    }

    void RegisterButtonListeners()
    {
        Button[] buttons = FindObjectsOfType<Button>();
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
}
