using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Video;

public class TitleSceneController : MonoBehaviour
{
    public static TitleSceneController Instance;
    public float fadeTime;
    public VideoPlayer introVideo;

    [HideInInspector] public bool Mainpanel, Savepanel, Settingpanel;
    public GameObject Background_001, Background_002, Options, ContinuePane, SettingPane;
    [HideInInspector] public CanvasGroup CutscenePanel;

    void Start()
    {
        Instance = this;
        GameObject obj = GameObject.Find("CutscenePanel");
        CutscenePanel = obj.GetComponent<CanvasGroup>();
        CutscenePanel.DOFade(1f, 0f);
        CutscenePanel.DOFade(0f, 100f * Time.deltaTime);
        Mainpanel = true;
        Savepanel = false;
        Settingpanel = false;
        Background_001.SetActive(true);
        Background_002.SetActive(false);
        ContinuePane.SetActive(false);
        SettingPane.SetActive(false);
        Options.SetActive(true);
        fadeTime = 2f;

        RenderTexture rt = introVideo.targetTexture as RenderTexture;
        if (rt != null)
        {
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = null;
            rt.Release();
        }
    }

    public void OnClickStart()
    {
        StartCoroutine(Cutscene());
    }

    public void OnClickInformation()
    {
        SceneManager.LoadScene("Information");
    }

    public void OnClickContinue()
    {
        Continue();
    }

    public void OnClickSetting()
    {
        Setting();
    }

    public void OnClickExit()
    {
        Debug.Log("GameExit");
        Application.Quit();
    }

    public void FadeOut()
    {
        GameObject obj = GameObject.Find("CutscenePanel");
        CutscenePanel = obj.GetComponent<CanvasGroup>();
        CutscenePanel.DOFade(1f, fadeTime);
        SceneAudioManager.Instance.FadeOutSceneMusic(fadeTime);
        Debug.Log("FadeOut");
    }

    void Continue()
    {
        Background_001.SetActive(false);
        Background_002.SetActive(true);
        Options.SetActive(false);
        ContinuePane.SetActive(true);
    }

    void Setting()
    {
        Background_001.SetActive(false);
        Background_002.SetActive(true);
        Options.SetActive(false);
        SettingPane.SetActive(true);
    }

    public void BackToTitle()
    {
        Debug.Log("BackToTitle");
        Background_001.SetActive(true);
        Background_002.SetActive(false);
        Options.SetActive(true);
        SettingPane.SetActive(false);
        ContinuePane.SetActive(false);
    }

    IEnumerator Cutscene()
    {
        // 1. 初次淡入黑幕（保留你原本的 FadeOut）
        FadeOut();
        GameManager.isContinue = false;
        yield return new WaitForSeconds(fadeTime);

        // 2. 读取存档
        GameSaveData data = SaveManager.Instance.LoadGame();

        if (!data.hasVisitedStage0)
        {
            // 4. 准备并播放视频
            if (introVideo != null)
            {
                // 4.1 准备 (载入 metadata)
                if (!introVideo.isPrepared)
                {
                    introVideo.Prepare();
                    yield return new WaitUntil(() => introVideo.isPrepared);
                }

                // 4.2 播放
                introVideo.Play();

                // 4.3 等一帧确保 isPlaying 变 true
                yield return null;

                // 4.4 等视频播完或按 Enter 跳过
                bool finished = false;
                introVideo.loopPointReached += vp => finished = true;
                while (introVideo.isPlaying && !finished)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        // 1. 停止视频播放
                        introVideo.Stop();

                        // 2. 找到 RawImage
                        var disp = GameObject.Find("IntroVideoDisplay").GetComponent<RawImage>();

                        disp.DOFade(0f, fadeTime)
                            .OnComplete(() =>
                            {
                                // 淡出完之后再清空纹理，彻底移除视频
                                disp.texture = null;

                                // （可选）如果你还要释放 RT 资源
                                var rt = introVideo.targetTexture as RenderTexture;
                                if (rt != null)
                                {
                                    RenderTexture.active = rt;
                                    GL.Clear(true, true, Color.clear);
                                    RenderTexture.active = null;
                                    rt.Release();
                                }
                            });

                        break;
                    }
                    yield return null;
                }
            }

            // 5. 视频结束后再一次淡入黑幕
            FadeOut();
            yield return new WaitForSeconds(fadeTime);

            // 6. 标记已看过、存档、切到 Stage0
            data.hasVisitedStage0 = true;
            data.sceneName = "Stage1";
            data.masterCase = 1;
            data.playerHP = 100;
            data.playerMP = 50;
            SaveManager.Instance.SaveGame(data);

            SceneManager.LoadScene("Stage0");
        }
        else
        {
            // 已经看过，直接跳 Stage1
            SceneManager.LoadScene("Stage1");
        }
    }

    public void OnClickTest()
    {
        SceneManager.LoadScene("Test");
    }

    void OnApplicationQuit()
    {
        RenderTexture rt = introVideo.targetTexture as RenderTexture;
        if (rt != null)
        {
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = null;
            rt.Release();
        }
    }
}
