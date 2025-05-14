using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class StageSceneHandler : MonoBehaviour
{
    private bool hasTriggeredStart = false;
    private bool hasTriggeredReturn = false;
    private bool waitingForStoryController = false;
    public float fadeDuration = 1f;

    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Stage0" && !hasTriggeredStart)
        {
            StoryController story = FindObjectOfType<StoryController>();
            if (story != null)
            {
                hasTriggeredStart = true;
                story.StartStory("start");
                Debug.Log("start");
            }
            else
            {
                if (!waitingForStoryController)
                {
                    waitingForStoryController = true;
                    Debug.Log("等待 StoryController...");
                }
            }
        }

        // ✅ Stage4 按 Enter 觸發淡出 + 回 TitleScene
        if (sceneName == "Stage4" && Input.GetKeyDown(KeyCode.Return) && !hasTriggeredReturn)
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

        // ✅ 音樂與畫面一起淡出
        GlobalAudioManager.Instance.FadeOutMusic(fadeDuration);
        cg.blocksRaycasts = true;
        cg.DOFade(1f, fadeDuration);

        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene("TitleScene");
    }
}
