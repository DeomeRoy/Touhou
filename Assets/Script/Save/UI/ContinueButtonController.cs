using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ContinueButtonController : MonoBehaviour
{
    [System.Serializable]
    public class StageButtonMapping
    {
        public string sceneName;//場景名稱
        public int caseNumber;//case編號
        public Sprite normalSprite;//未點擊按鈕
        public Sprite pressedSprite;
    }

    public List<StageButtonMapping> mappings;
    public Image continueButtonImage; //Continue Button point to the image component

    void OnEnable()
    {
        //default button sprite
        GameSaveData data = SaveManager.Instance.LoadGame();
        if (data != null)
        {
            Sprite targetSprite = GetNormalButtonSprite(data.sceneName, data.masterCase);
            if (targetSprite != null)
            {
                continueButtonImage.sprite = targetSprite;
            }
        }
    }

    Sprite GetNormalButtonSprite(string sceneName, int masterCase)
    {
        foreach (var mapping in mappings)
        {
            if (mapping.sceneName == sceneName && mapping.caseNumber == masterCase)
                return mapping.normalSprite;
        }
        return null;
    }

    Sprite GetPressedButtonSprite(string sceneName, int masterCase)
    {
        foreach (var mapping in mappings)
        {
            if (mapping.sceneName == sceneName && mapping.caseNumber == masterCase)
                return mapping.pressedSprite;
        }
        return null;
    }

    public void OnContinueButtonClick()
    {
        GameManager.isContinue = true;
        GameSaveData data = SaveManager.Instance.LoadGame();
        if (data != null)
        {
            Sprite pressed = GetPressedButtonSprite(data.sceneName, data.masterCase);
            if (pressed != null)
                continueButtonImage.sprite = pressed;
        }
        //fade out & load scene
        StartCoroutine(ContinueSequence());
    }

    private System.Collections.IEnumerator ContinueSequence()
    {
        float fadeDuration = 1.2f;//fade out duration

        StartCoroutine(SimpleFadeOut(fadeDuration));
        StartCoroutine(SimpleMusicFadeOut(GlobalAudioManager.Instance.musicSource1, fadeDuration));

        yield return new WaitForSeconds(fadeDuration);
        GameSaveData data = SaveManager.Instance.LoadGame();
        if (data != null)
        {
            SceneManager.LoadScene(data.sceneName);
        }
    }


    public System.Collections.IEnumerator SimpleFadeOut(float duration)
    {
        CanvasGroup cg = TitleSceneController.Instance.CutscenePanel;
        if (cg == null)
        {
            Debug.LogWarning("null");
            yield break;
        }
        float startAlpha = cg.alpha;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 1f, t / duration);
            yield return null;
        }
        cg.alpha = 1f;
    }


    public System.Collections.IEnumerator SimpleMusicFadeOut(AudioSource source, float duration)
    {
        if (source == null)
        {
            Debug.LogWarning("null87");
            yield break;
        }
        float startVol = source.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, 0f, t / duration);
            yield return null;
        }
        source.volume = 0f;
        source.Stop();
    }

}
