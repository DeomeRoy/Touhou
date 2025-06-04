using UnityEngine;
using UnityEngine.UI;            // 如果你用的是 UnityEngine.UI.Text
using UnityEngine.SceneManagement;
using DG.Tweening;              // DOTween 用来做淡入／淡出
using System.Collections;
using TMPro;

public class ThankYouSingleText : MonoBehaviour
{
    public TMP_Text thankYouText;
    private CanvasGroup textCanvasGroup;
    public CanvasGroup cutscenePanel;
    public float fadeInDuration = 1f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 1f;
    public float cameraSpeed = 2f;
    public float detectY = -5f;
    public float waitBeforeFade = 5f;
    public float fadeToBlackDuration = 1f;
    public string line1 = "感谢你一路的陪伴！";
    public string line2 = "有你相伴，旅程更美好。";
    public string line3 = "期待再次相遇，祝好！";

    private bool hasFadedToBlack = false;

    void Start()
    {
        textCanvasGroup = thankYouText.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null)
            textCanvasGroup = thankYouText.gameObject.AddComponent<CanvasGroup>();

        textCanvasGroup.alpha = 0f;
        thankYouText.text = "";
        if (cutscenePanel != null)
            cutscenePanel.alpha = 0f;
        StartCoroutine(RunThankYouSequence());
    }

    private IEnumerator RunThankYouSequence()
    {
        yield return FadeInAndOut(line1);
        yield return FadeInAndOut(line2);
        yield return FadeInAndOut(line3);

        yield return StartCoroutine(MoveCameraAndDetect());
    }

    private IEnumerator FadeInAndOut(string line)
    {
        thankYouText.text = line;
        textCanvasGroup.alpha = 0f;

        textCanvasGroup.DOFade(1f, fadeInDuration);
        yield return new WaitForSeconds(fadeInDuration);

        yield return new WaitForSeconds(displayDuration);

        textCanvasGroup.DOFade(0f, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);

        thankYouText.text = "";
    }

    private IEnumerator MoveCameraAndDetect()
    {
        // 确保文字隐藏
        textCanvasGroup.alpha = 0f;

        while (!hasFadedToBlack)
        {
            // 摄像机 Y 坐标 ≤ detectY，也触发淡出（并且等待 waitBeforeFade）
            if (Camera.main != null && Camera.main.transform.position.y <= detectY)
            {
                StartCoroutine(FadeToBlackAndReturn());
                yield break;
            }

            // 持续让摄像机往下移动
            if (Camera.main != null)
                Camera.main.transform.position += Vector3.down * (cameraSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator FadeToBlackAndReturn()
    {
        if (hasFadedToBlack) yield break;
        hasFadedToBlack = true;

        // 1. 先等待 waitBeforeFade 秒
        yield return new WaitForSeconds(waitBeforeFade);

        // 2. 淡出音乐（可选）
        GlobalAudioManager.Instance.FadeOutMusic(fadeToBlackDuration);

        // 3. 黑幕淡入
        if (cutscenePanel != null)
        {
            // 拦截点击
            cutscenePanel.blocksRaycasts = true;
            // alpha 0→1
            cutscenePanel.DOFade(1f, fadeToBlackDuration);
        }

        // 4. 等待黑幕完全淡入
        yield return new WaitForSeconds(fadeToBlackDuration);

        // 5. 切回主菜单
        SceneManager.LoadScene("TitleScene");
    }
}