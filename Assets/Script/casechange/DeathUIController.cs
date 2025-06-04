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
        GameSaveData data = SaveManager.Instance.LoadGame();
        if (data.sceneName == "Stage1" && data.masterCase == 1)
        {
            continueButton.gameObject.SetActive(false);
        }
        else
        {
            deathPanel.SetActive(false);
            GameManager.isContinue = true;
            StartCoroutine(FadeOutAndLoadScene(data.sceneName));
        }
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float fD = 150f * Time.deltaTime;//from TitleSceneController
        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.DOFade(1f, fD);
        SceneAudioManager.Instance.FadeOutSceneMusic(fD);
        yield return new WaitForSeconds(fD);
        SceneManager.LoadScene(sceneName);

        GlobalAudioManager.Instance.StopAllMusic();
        GlobalAudioManager.Instance.PlayMainMenuMusic();
    }
}
