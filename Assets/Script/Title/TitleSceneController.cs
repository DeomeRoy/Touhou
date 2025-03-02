using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleSceneController : MonoBehaviour{
    [HideInInspector]public bool Mainpanel,Savepanel,Settingpanel;
    [HideInInspector]public GameObject Background_001,Background_002,Options;
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
                        print("Start");
                        FadeOut();
                        StartCoroutine(Cutscene());
                        break;
                    case "Information":
                        print("Information");
                        SceneManager.LoadScene("Information");
                        break;
                    case "Continue":
                        print("Continue");
                        SwitchBackground(1);
                        break;
                    case "Setting":
                        print("Setting");
                        SwitchBackground(1);
                        break;
                    case "Exit":
                        print("GameExit");
                        Application.Quit();
                        break;
                    case "Stage 2":
                    case "Stage 3":
                        break;
                    case "Back":
                        SwitchBackground(2);
                        break;
                }
            }
        }
    }
    void SwitchBackground(int x){
        switch(x){
            case 1:
            Background_001.SetActive(false);
            Background_002.SetActive(true);
            Options.SetActive(false);
            break;
            case 2:
            Background_001.SetActive(true);
            Background_002.SetActive(false);
            Options.SetActive(true);
            break;
        }
    }
    void FadeOut(){
        GameObject obj = GameObject.Find("CutscenePanel");
        CutscenePanel = obj.GetComponent<CanvasGroup>();
        CutscenePanel.DOFade(1f,150f*Time.deltaTime);
    }
    IEnumerator Cutscene(){
        yield return new WaitForSeconds(160f*Time.deltaTime);
        print("GameStart");
        SceneManager.LoadScene("Stage1");
    }
}

    