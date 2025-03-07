using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleSceneController : MonoBehaviour{
    [HideInInspector]public bool Mainpanel,Savepanel,Settingpanel;
    public GameObject Background_001,Background_002,Options,ContinuePane,SettingPane;
    [HideInInspector]public CanvasGroup CutscenePanel;
    
    void Start(){
        GameObject obj = GameObject.Find("CutscenePanel");
        CutscenePanel = obj.GetComponent<CanvasGroup>();
        CutscenePanel.DOFade(1f,0f);
        CutscenePanel.DOFade(0f,100f*Time.deltaTime);
        Mainpanel    = true;
        Savepanel    = false;
        Settingpanel = false;
        Background_001.SetActive(true);
        Background_002.SetActive(false);
        ContinuePane.SetActive(false);
        SettingPane.SetActive(false);
        Options.SetActive(true);
    }
    void Update(){
        if (Input.GetMouseButtonUp(0)){
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (RaycastResult result in results){
                switch(result.gameObject.name){
                    case "Start":
                        FadeOut();
                        StartCoroutine(Cutscene());
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
                        print("GameExit");
                        Application.Quit();
                        break;
                }
            }
        }
    }
    void FadeOut(){
        GameObject obj = GameObject.Find("CutscenePanel");
        CutscenePanel = obj.GetComponent<CanvasGroup>();
        CutscenePanel.DOFade(1f,150f*Time.deltaTime);
    }
    void Continue(){
        Background_001.SetActive(false);
        Background_002.SetActive(true);
        Options.SetActive(false);
        ContinuePane.SetActive(true);
    }
    void Setting(){
        Background_001.SetActive(false);
        Background_002.SetActive(true);
        Options.SetActive(false);
        SettingPane.SetActive(true);
    }
    public void BackToTitle(){
        Debug.Log("111");
        Background_001.SetActive(true);
        Background_002.SetActive(false);
        Options.SetActive(true);
        SettingPane.SetActive(false);
        ContinuePane.SetActive(false);
    }
    IEnumerator Cutscene(){
        yield return new WaitForSeconds(160f*Time.deltaTime);
        print("GameStart");
        SceneManager.LoadScene("Stage1");
    }
}

    