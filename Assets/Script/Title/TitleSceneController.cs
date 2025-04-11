using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleSceneController : MonoBehaviour
{
    public static TitleSceneController Instance;
    public float fadeTime;

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
        fadeTime = 150f * Time.deltaTime;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (RaycastResult result in results)
            {
                switch (result.gameObject.name)
                {
                    case "Start":
                        FadeOut();
                        StartCoroutine(Cutscene());
                        GameManager.isContinue = false;
                        break;
                    case "Information":
                        SceneManager.LoadScene("Information");
                        break;
                    case "Continue":
                        Continue();
                        break;
                    case "Setting":
                        Setting();
                        break;
                    case "Exit":
                        Debug.Log("GameExit");
                        Application.Quit();
                        break;
                }
            }
        }
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
        GameManager.isContinue = false;
        yield return new WaitForSeconds(fadeTime);
        Debug.Log("GameStart");
        SceneManager.LoadScene("Stage1");
    }
}
