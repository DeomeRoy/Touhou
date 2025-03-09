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
            GlobalAudioManager.Instance.CrossfadeToStage1Music(stageFadeOutDuration, stageFadeInDuration);
        }
        else if (scene.name == "Stage2")
        {
            GlobalAudioManager.Instance.CrossfadeToStage2Music(stageFadeOutDuration, stageFadeInDuration);
        }
        else if (scene.name == "Stage3")
        {
            GlobalAudioManager.Instance.CrossfadeToStage3Music(stageFadeOutDuration, stageFadeInDuration);
        }

        previousScene = scene.name;

        RegisterButtonListeners();
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
