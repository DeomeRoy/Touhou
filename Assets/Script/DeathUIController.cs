using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class DeathUIController : MonoBehaviour
{
    public GameObject deathPanel;
    public Button mainMenuButton;
    public Button continueButton;

    public float fadeDuration = 2.5f;
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
        GameObject obj = GameObject.Find("CutscenePanel");
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();

        cg.blocksRaycasts = true;
        cg.DOFade(1f, fadeDuration);
        GlobalAudioManager.Instance.FadeOutMusic(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(sceneName);
    }
}
