using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ContinueButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable]
    public class StageButtonMapping
    {
        public string sceneName;
        public int caseNumber;
        public Sprite normalSprite;
        public Sprite pressedSprite;
    }

    public List<StageButtonMapping> mappings;
    public Image continueButtonImage;

    void OnEnable()
    {
        GameSaveData data = SaveManager.Instance.LoadGame();
        if (data != null)
        {
            Sprite normal = GetNormalButtonSprite(data.sceneName, data.masterCase);
            if (normal != null)
            {
                continueButtonImage.sprite = normal;
            }
        }
    }

    Sprite GetNormalButtonSprite(string sceneName, int masterCase)
    {
        foreach (var mapping in mappings)
        {
            if (mapping.sceneName == sceneName && masterCase >= 1 && masterCase <= 3 && mapping.caseNumber == 1)//case1~3都視為case1，有需要可刪除
            {
                return mapping.normalSprite;
            }
            else if (mapping.sceneName == sceneName && mapping.caseNumber == masterCase)
            {
                return mapping.normalSprite;
            }
        }
        return null;
    }

    Sprite GetPressedButtonSprite(string sceneName, int masterCase)
    {
        foreach (var mapping in mappings)
        {
            if (mapping.sceneName == sceneName && masterCase >= 1 && masterCase <= 3 && mapping.caseNumber == 1)//case1~3都視為case1，有需要可刪除
            {
                return mapping.pressedSprite;
            }
            else if (mapping.sceneName == sceneName && mapping.caseNumber == masterCase)
            {
                return mapping.pressedSprite;
            }
        }
        return null;
    }

    public void OnContinueButtonClick()
    {
        GameSaveData data = SaveManager.Instance.LoadGame();
        if (data != null) 
        {
            if (data.masterCase == 4 || (data.sceneName != "Stage1" && data.masterCase == 1))//特殊關卡載入判斷，有需要可刪除或修改
            {
                GameManager.isContinue = true;
                StartCoroutine(ContinueSequence());
            }
        }
    }

    private System.Collections.IEnumerator ContinueSequence()
    {
        float fadeDuration = 1.2f;

        StartCoroutine(SimpleFadeOut(fadeDuration));
        StartCoroutine(SimpleMusicFadeOut(GlobalAudioManager.Instance.musicSource1, fadeDuration));

        yield return new WaitForSeconds(fadeDuration);
        GameSaveData data = SaveManager.Instance.LoadGame();
        SceneManager.LoadScene(data.sceneName);
    }

    public System.Collections.IEnumerator SimpleFadeOut(float duration)
    {
        CanvasGroup cg = TitleSceneController.Instance.CutscenePanel;
        if (cg == null)
            yield break;

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
            yield break;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        GameSaveData data = SaveManager.Instance.LoadGame();
        if (data != null)
        {
            Sprite pressed = GetPressedButtonSprite(data.sceneName, data.masterCase);
            if (pressed != null)
                continueButtonImage.sprite = pressed;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameSaveData data = SaveManager.Instance.LoadGame();
        if (data != null)
        {
            Sprite normal = GetNormalButtonSprite(data.sceneName, data.masterCase);
            if (normal != null)
                continueButtonImage.sprite = normal;
        }
    }
}
