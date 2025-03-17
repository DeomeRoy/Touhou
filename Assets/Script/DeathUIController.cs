using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathUIController : MonoBehaviour
{
    public GameObject deathPanel;
    public Button mainMenuButton;
    public Button continueButton;

    public float fadeDuration = 1.5f;
    public CanvasGroup fadeCanvasGroup;

    void Start()
    {
        deathPanel.SetActive(false);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    public void OnMainMenuClicked()
    {
        deathPanel.SetActive(false);
        StartCoroutine(FadeOutAndLoadScene("TitleScene"));
    }

    public void OnContinueClicked()
    {
        deathPanel.SetActive(false);
        GameSaveData data = SaveManager.Instance.LoadGame();
        StartCoroutine(FadeOutAndLoadScene(data.sceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        fadeCanvasGroup = TitleSceneController.Instance.CutscenePanel;
        AudioSource musicSource = GlobalAudioManager.Instance.musicSource1;
        yield return StartCoroutine(CombinedFadeOut(fadeCanvasGroup, musicSource, fadeDuration));
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator CombinedFadeOut(CanvasGroup cg, AudioSource source, float duration)
    {
        if (cg == null || source == null)
        {
            yield break;
        }
        float startAlpha = cg.alpha;
        float startVol = source.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float factor = t / duration;
            cg.alpha = Mathf.Lerp(startAlpha, 1f, factor);
            source.volume = Mathf.Lerp(startVol, 0f, factor);
            yield return null;
        }
        cg.alpha = 1f;
        source.volume = 0f;
        source.Stop();
    }
}
