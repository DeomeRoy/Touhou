using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class LevelImageFader : MonoBehaviour
{
    public Image faderImage;//用來淡入淡出的圖片
    public CanvasGroup fadeCanvasGroup;//控制alpha

    //關卡圖片
    public Sprite stage1Sprite;
    public Sprite stage2Sprite;
    public Sprite stage3Sprite;
    public Sprite level2Sprite;
    public Sprite level3Sprite;
    public Sprite level4Sprite;
    bool shouldTriggerBoss = false;
    bool hasShownLevel3 = false;


    private float fadeDuration = 0.5f;//淡入&淡出時間
    private float holdDuration = 1.5f;//顯示時間

    public System.Action onFadeOutStart;


    void Start()
    {
        if (faderImage != null)
        {
            if (SceneManager.GetActiveScene().name == "Stage1")
            {
                faderImage.sprite = stage1Sprite;
                StartCoroutine(FadeInOut());
            }
            if (SceneManager.GetActiveScene().name == "Stage2")
            {
                faderImage.sprite = stage2Sprite;
                StartCoroutine(FadeInOut());
            }
            if (SceneManager.GetActiveScene().name == "Stage3")
            {
                faderImage.sprite = stage3Sprite;
                StartCoroutine(FadeInOut());
            }
        }
    }

    public void FadeForLevel(int levelCase)
    {
        if (faderImage == null)
            return;

        switch (levelCase)
        {
            case 2:
                faderImage.sprite = level2Sprite;
                StartCoroutine(FadeInOut());
                break;

            case 3:
                faderImage.sprite = level3Sprite;
                hasShownLevel3 = true;
                StartCoroutine(FadeInOut());
                break;

            case 4:
                if (hasShownLevel3)
                    return;

                faderImage.sprite = level4Sprite;
                shouldTriggerBoss = true;
                StartCoroutine(FadeInOut());
                break;

            default:
                return;
        }
    }


    public IEnumerator Fadeway()
    {
        
        faderImage.sprite = level4Sprite;
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.DOFade(1f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration + holdDuration);

        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Stage1":
                Boss_A bossA = FindObjectOfType<Boss_A>();
                bossA.ChatEnd();
                break;
            case "Stage2":
                Boss_B bossB = FindObjectOfType<Boss_B>();
                bossB.ChatEnd();
                break;
            case "Stage3":
                Boss_C bossC = FindObjectOfType<Boss_C>();
                bossC.ChatEnd();
                break;
        }

        PlayerController player = FindObjectOfType<PlayerController>();
        player.controlEnabled = true;

        SceneAudioManager.Instance.FadeToBoss();

        fadeCanvasGroup.DOFade(0f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
    }


    private IEnumerator FadeInOut()
    {
        
        fadeCanvasGroup.DOFade(1f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        yield return new WaitForSeconds(holdDuration);

        if (shouldTriggerBoss)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case "Stage1":
                    Boss_A bossA = FindObjectOfType<Boss_A>();
                    bossA.ChatEnd();
                    break;
                case "Stage2":
                    Boss_B bossB = FindObjectOfType<Boss_B>();
                    bossB.ChatEnd();
                    break;
                case "Stage3":
                    // Boss_C bossC = FindObjectOfType<Boss_B>();
                    // bossC.ChatEnd();
                    break;
            }

            PlayerController player = FindObjectOfType<PlayerController>();
            player.controlEnabled = true;

            shouldTriggerBoss = false; // reset
        }

        fadeCanvasGroup.DOFade(0f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
    }
}