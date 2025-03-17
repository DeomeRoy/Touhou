using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelImageFader : MonoBehaviour
{
    public Image faderImage;//用來淡入淡出的圖片

    //關卡圖片
    public Sprite stage1Sprite;
    public Sprite stage2Sprite;
    public Sprite stage3Sprite;
    public Sprite level2Sprite;
    public Sprite level3Sprite;
    public Sprite level4Sprite;

    private float fadeDuration = 0.5f;//淡入&淡出時間
    private float holdDuration = 1.5f;//顯示時間


    void Start()
    {
        faderImage.canvasRenderer.SetAlpha(0f);
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
                break;
            case 3:
                faderImage.sprite = level3Sprite;
                break;
            case 4:
                faderImage.sprite = level4Sprite;
                break;
            default:
                return;
        }
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        faderImage.CrossFadeAlpha(1f, fadeDuration, false);//alpha*fadetime
        yield return new WaitForSeconds(fadeDuration);

        yield return new WaitForSeconds(holdDuration);

        faderImage.CrossFadeAlpha(0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }
}
