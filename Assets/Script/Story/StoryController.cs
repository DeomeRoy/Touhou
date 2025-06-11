using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Stage0Step
{
    public List<GameObject> objectsToShow;
}

public class StoryController : MonoBehaviour
{
    public List<Stage0Step> stage0Steps = new List<Stage0Step>();
    private List<GameObject> previousStage0Objects = new List<GameObject>();
    //連結ui的變數
    public GameObject storyPanel;
    public CanvasGroup storyCanvasGroup;
    public Button chatButton;
    public Button skipButton;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;

    //儲存角色的地方，連結到CharacterData腳本
    public CharacterData[] allCharacters;

    //對話效果
    public float jumpDistance = 20f;//上下彈跳的距離
    public Color brightColor = Color.white;//非對話者的亮度
    public Color darkColor = Color.gray;//說話者的亮度

    //儲存劇情，連結到StorySequence腳本
    public StorySequence[] allStories;

    //音樂淡出我做成跟物件淡入不同步的，音樂淡入的詳情在wallmover腳本
    public float fadeInDelay = 1f;//關卡音樂淡出後延遲淡入劇情物件的時間
    public float fadeInDuration = 1f;//劇情物件淡入的總時長

    //音樂跟物件一起淡出的時間
    public float endFadeDuration = 1f;

    private bool isPlaying = false;//是否正在播放劇情
    private int currentStepIndex = 0;//目前進行的劇情步驟
    private StorySequence currentSequence;//目前的劇情內容
    private string previousSpeakerID = null;//上一句說話者的id
    private Dictionary<string, bool> currentlyVisible;//目前顯示的角色

    private float fadeDuration = 0.2f;//角色在劇情淡入淡出的時間
    private float moveDuration = 0.2f;//角色在劇情上下彈的時間
    private Dictionary<string, bool> hasBeenDisplayed = new Dictionary<string, bool>();

    void Awake()
    {
        currentlyVisible = new Dictionary<string, bool>();//初始化，不用管
    }

    void Start()
    {
        //在被特定角色呼叫前這些劇情物件都不顯示
        currentlyVisible["Player"] = false;
        currentlyVisible["Enemy"] = false;
        storyPanel.SetActive(false);

        //讓按鈕被點擊時觸發的函數
        chatButton.onClick.AddListener(OnChatClicked);
        skipButton.onClick.AddListener(OnSkipClicked);
    }


    //透過其他腳本呼叫，收到傳入劇情ID再播放對應劇情
    public void StartStory(string storyID)
    {
        currentSequence = allStories.FirstOrDefault(s => s.storyID == storyID);

        //開始劇情時禁用玩家行動
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.controlEnabled = false;
            GlobalAudioManager.Instance.StopRunSound();
            player.SetSprite(player.idleSprite, player.idleSize, player.idleOffset);
        }

        // 初始化
        previousSpeakerID = null;
        currentStepIndex = 0;
        isPlaying = true;
        if (SceneManager.GetActiveScene().name == "stage0")
        {
            previousStage0Objects.Clear();
            foreach (var step in stage0Steps)
            {
                if (step.objectsToShow != null)
                {
                    foreach (var obj in step.objectsToShow)
                    {
                        if (obj != null) obj.SetActive(false);
                    }
                }
            }
        }

        StartCoroutine(WaitAndStartStory());
    }

    // 等待淡出音樂 > 等 fadeInDelay > 面板淡入
    private IEnumerator WaitAndStartStory()
    {
        //yield return new WaitForSeconds(GlobalAudioManager.Instance.storyFadeDuration);//這裡原本用來淡出，但是音樂改到wallmover，暫時註解

        yield return new WaitForSeconds(fadeInDelay);
        storyPanel.SetActive(true);
        storyCanvasGroup.alpha = 0f;
        yield return StartCoroutine(
            FadeCanvasGroup(storyCanvasGroup, 0f, 1f, fadeInDuration)
        );

        //GlobalAudioManager.Instance.PlayStoryMusicWithFadeIn();//此段也移到wallmover
        UpdateStoryStep();//顯示對話
    }

    void OnChatClicked()
    {
        currentStepIndex++;
        if (currentSequence == null) return;

        if (currentStepIndex >= currentSequence.steps.Length)
        {
            StartCoroutine(EndStory());
        }
        else
        {
            UpdateStoryStep();
        }
    }

    void OnSkipClicked()
    {
        StartCoroutine(EndStory());
    }

    void UpdateStoryStep()
    {
        if (SceneManager.GetActiveScene().name == "stage0")
        {
            HandleStage0Step();
        }

        if (currentSequence == null || currentSequence.steps.Length == 0) return;

        StoryStep step = currentSequence.steps[currentStepIndex];
        dialogueText.text = step.dialogue;
        nameText.text = step.speakerName;

        //顯示或隱藏角色
        var playerData = allCharacters.FirstOrDefault(c => c.characterID == "Player");
        var enemyData = allCharacters.FirstOrDefault(c => c.characterID == "Enemy");

        ShowOrHideCharacter(playerData, step.showPlayer);
        ShowOrHideCharacter(enemyData, step.showEnemy);

        var prevSpeakerData = allCharacters.FirstOrDefault(c => c.characterID == previousSpeakerID);//player down
        if (prevSpeakerData != null && previousSpeakerID != step.speakerID)
        {
            var rtPrev = prevSpeakerData.characterImage.GetComponent<RectTransform>();
            Vector2 fromPos = rtPrev.anchoredPosition;
            Vector2 toPos = prevSpeakerData.basePosition + new Vector2(0, -jumpDistance);
            StartCoroutine(SmoothMove(rtPrev, fromPos, toPos));

            prevSpeakerData.characterImage.color = darkColor;
        }

        var speakerData = allCharacters.FirstOrDefault(c => c.characterID == step.speakerID);//player up
        if (speakerData != null)
        {
            if (!string.IsNullOrEmpty(step.photoID))
            {
                var photo = speakerData.photos.FirstOrDefault(p => p.photoID == step.photoID);
                if (photo != null)
                    speakerData.characterImage.sprite = photo.sprite;
            }

            var rtSpeaker = speakerData.characterImage.GetComponent<RectTransform>();
            if (previousSpeakerID == null)
            {
                speakerData.characterImage.color = brightColor;
            }
            else
            {
                if (previousSpeakerID == step.speakerID)
                {
                    if (step.keepBrightIfSameSpeaker)
                        speakerData.characterImage.color = brightColor;
                    else
                        speakerData.characterImage.color = darkColor;
                }
                else
                {
                    Vector2 fromPos2 = rtSpeaker.anchoredPosition;
                    Vector2 toPos2 = speakerData.basePosition + new Vector2(0, jumpDistance);
                    StartCoroutine(SmoothMove(rtSpeaker, fromPos2, toPos2));
                    speakerData.characterImage.color = brightColor;
                }
            }
        }
        previousSpeakerID = step.speakerID;
    }

    // 故事結束後故事音樂跟劇情物件一起淡出 > 播Boss音樂 > 恢復玩家行動
    private IEnumerator EndStory()
    {
        if (!isPlaying) yield break;

        bool autoNext = currentSequence != null && currentSequence.autoLoadNextScene;

        // 音樂與面板淡出
        StartCoroutine(FadeCanvasGroup(storyCanvasGroup, 1f, 0f, endFadeDuration));
        SceneAudioManager.Instance.FadeOutSceneMusic(endFadeDuration);

        yield return new WaitForSeconds(endFadeDuration);

        storyPanel.SetActive(false);
        isPlaying = false;

        if (autoNext)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string nextSceneName = GetNextSceneName(currentSceneName);

            if (nextSceneName != "Stage4")
            {
                GameSaveSystem.SaveFixedStoryProgress(nextSceneName, 1, 100, 50);
            }

            GameObject obj = GameObject.Find("CutscenePanel");
            CanvasGroup CutscenePanel = obj.GetComponent<CanvasGroup>();

            CutscenePanel.blocksRaycasts = true;
            CutscenePanel.DOFade(1f, endFadeDuration);

            yield return new WaitForSeconds(endFadeDuration);

            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.controlEnabled = true;
            }
            LevelImageFader fader = FindObjectOfType<LevelImageFader>();
            StartCoroutine(fader.Fadeway());

        }
    }

    private void HandleStage0Step()
    {
        if (stage0Steps == null || stage0Steps.Count == 0) return;
        if (currentStepIndex < 0 || currentStepIndex >= stage0Steps.Count) return;

        var currentObjects = stage0Steps[currentStepIndex].objectsToShow ?? new List<GameObject>();

        foreach (var obj in previousStage0Objects)
        {
            if (obj != null && !currentObjects.Contains(obj))
                obj.SetActive(false);
        }
        foreach (var obj in currentObjects)
        {
            if (obj != null && !previousStage0Objects.Contains(obj))
                obj.SetActive(true);
        }
        previousStage0Objects = new List<GameObject>(currentObjects);
    }


    private string GetNextSceneName(string currentName)
    {
        int index = currentName.Length - 1;
        while (index >= 0 && char.IsDigit(currentName[index]))
        {
            index--;
        }
        index++;
        if (index < currentName.Length)
        {
            string prefix = currentName.Substring(0, index);
            string numberPart = currentName.Substring(index);
            int number;
            if (int.TryParse(numberPart, out number))
            {
                number++;
                return prefix + number.ToString();
            }
        }
        return currentName + "1";
    }

    //Coroutines
    private void ShowOrHideCharacter(CharacterData charData, bool wantShow)
    {
        if (charData == null) return;

        bool wasShowing;
        if (!currentlyVisible.TryGetValue(charData.characterID, out wasShowing))
        {
            wasShowing = false;
            currentlyVisible[charData.characterID] = false;
        }
        if (!hasBeenDisplayed.ContainsKey(charData.characterID))
        {
            hasBeenDisplayed[charData.characterID] = true;
            if (wantShow)
            {
                charData.characterImage.gameObject.SetActive(true);
                Color c = charData.characterImage.color;
                charData.characterImage.color = new Color(c.r, c.g, c.b, 1f);
                currentlyVisible[charData.characterID] = true;
            }
            else
            {
                charData.characterImage.gameObject.SetActive(false);
                Color c = charData.characterImage.color;
                charData.characterImage.color = new Color(c.r, c.g, c.b, 0f);
                currentlyVisible[charData.characterID] = false;
            }
            return;
        }

        if (wantShow && !wasShowing)
        {
            StartCoroutine(FadeIn(charData));
            currentlyVisible[charData.characterID] = true;
        }
        else if (!wantShow && wasShowing)
        {
            StartCoroutine(FadeOut(charData));
            currentlyVisible[charData.characterID] = false;
        }
    }

    private IEnumerator FadeIn(CharacterData charData)
    {
        var image = charData.characterImage;
        image.gameObject.SetActive(true);
        Color c = image.color;
        image.color = new Color(c.r, c.g, c.b, 0f);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            image.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        image.color = new Color(c.r, c.g, c.b, 1f);
    }

    private IEnumerator FadeOut(CharacterData charData)
    {
        var image = charData.characterImage;
        Color c = image.color;
        float startAlpha = c.a;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            image.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        image.color = new Color(c.r, c.g, c.b, 0f);
        image.gameObject.SetActive(false);
    }

    private IEnumerator SmoothMove(RectTransform rt, Vector2 fromPos, Vector2 toPos)
    {
        float t = 0f;
        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float progress = t / moveDuration;
            rt.anchoredPosition = Vector2.Lerp(fromPos, toPos, progress);
            yield return null;
        }
        rt.anchoredPosition = toPos;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float elapse = 0f;
        while (elapse < duration)
        {
            elapse += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapse / duration);
            yield return null;
        }
        cg.alpha = to;
    }
}