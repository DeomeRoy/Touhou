using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class StageSceneHandler : MonoBehaviour
{
    private bool hasTriggeredStart = false;
    private bool hasTriggeredReturn = false;
    public float fadeDuration = 1f;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage0" && !hasTriggeredStart)
        {
            StoryController story = FindObjectOfType<StoryController>();
            if (story != null)
            {
                hasTriggeredStart = true;
                story.StartStory("start");
                Debug.Log("start (from OnSceneLoaded)");
            }
        }

        if (scene.name == "Stage4")
        {
            GameObject btnObj = GameObject.Find("ReturnButton");
            if (btnObj != null)
            {
                Button btn = btnObj.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveListener(OnReturnButtonClicked);
                    btn.onClick.AddListener(OnReturnButtonClicked);
                }
            }
        }
    }

    public void OnReturnButtonClicked()
    {
        if (!hasTriggeredReturn)
        {
            hasTriggeredReturn = true;
            StartCoroutine(FadeOutAndReturnToTitleLikeStory());
        }
    }

    private IEnumerator FadeOutAndReturnToTitleLikeStory()
    {
        GameObject obj = GameObject.Find("CutscenePanel");
        if (obj == null)
        {
            Debug.LogWarning("❗ 找不到 CutscenePanel");
            yield break;
        }

        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.LogWarning("❗ CutscenePanel 上缺少 CanvasGroup");
            yield break;
        }

        GlobalAudioManager.Instance.FadeOutMusic(fadeDuration);
        cg.blocksRaycasts = true;
        cg.DOFade(1f, fadeDuration);

        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene("TitleScene");
    }
}
