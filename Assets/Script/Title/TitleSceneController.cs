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
    public Button skipVideoButton;
    private bool skipVideoRequested;

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
        skipVideoRequested = false;
        skipVideoButton.onClick.AddListener(OnSkipVideoButton);

        RenderTexture rt = introVideo.targetTexture as RenderTexture;
        if (rt != null)
        {
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = null;
            rt.Release();
        }
    }

    public void OnSkipVideoButton()
    {
        skipVideoRequested = true;
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
        FadeOut();
        GameManager.isContinue = false;
        yield return new WaitForSeconds(fadeTime);

        GameSaveData data = SaveManager.Instance.LoadGame();

        if (!data.hasVisitedStage0)
        {
            if (introVideo != null)
            {
                if (!introVideo.isPrepared)
                {
                    introVideo.Prepare();
                    yield return new WaitUntil(() => introVideo.isPrepared);
                }
                introVideo.Play();
                yield return null;

                GameObject.Find("System").GetComponent<TitleMenuController>().ShowMenu();
                skipVideoRequested = false;
                bool finished = false;
                introVideo.loopPointReached += vp => finished = true;

                while (!finished && !skipVideoRequested)
                {
                    yield return null;
                }

                if (skipVideoRequested)
                {
                    introVideo.Stop();
                    var disp = GameObject.Find("IntroVideoDisplay").GetComponent<RawImage>();
                    disp.DOFade(0f, fadeTime).OnComplete(() =>
                    {
                        disp.texture = null;
                        var rt = introVideo.targetTexture as RenderTexture;
                        if (rt != null)
                        {
                            RenderTexture.active = rt;
                            GL.Clear(true, true, Color.clear);
                            RenderTexture.active = null;
                            rt.Release();
                        }
                    });
                }

            }
            FadeOut();
            yield return new WaitForSeconds(fadeTime);
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
