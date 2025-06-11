using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
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
    public string line1 = "";
    public string line2 = "";
    public string line3 = "";
    public SpriteRenderer imageToFade;
    public float postFadeInWait = 3f;

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

        yield return StartCoroutine(FadeInObject());
        yield return new WaitForSeconds(postFadeInWait);

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
        textCanvasGroup.alpha = 0f;

        while (!hasFadedToBlack)
        {
            if (Camera.main != null && Camera.main.transform.position.y <= detectY)
            {
                StartCoroutine(FadeToBlackAndReturn());
                yield break;
            }

            if (Camera.main != null)
                Camera.main.transform.position += Vector3.down * (cameraSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator FadeToBlackAndReturn()
    {
        if (hasFadedToBlack) yield break;
        hasFadedToBlack = true;

        yield return new WaitForSeconds(waitBeforeFade);
        GlobalAudioManager.Instance.FadeOutMusic(fadeToBlackDuration);

        if (cutscenePanel != null)
        {
            cutscenePanel.blocksRaycasts = true;
            cutscenePanel.DOFade(1f, fadeToBlackDuration);
        }

        yield return new WaitForSeconds(fadeToBlackDuration);
        SceneManager.LoadScene("TitleScene");
    }

    private IEnumerator FadeInObject()
    {
        if (imageToFade == null)
            yield break;

        Color c = imageToFade.color;
        c.a = 0f;
        imageToFade.color = c;
        imageToFade.gameObject.SetActive(true);

        imageToFade.DOFade(1f, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);
    }

}