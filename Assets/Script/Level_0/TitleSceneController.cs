using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleSceneController : MonoBehaviour{
    [HideInInspector]
    public bool Mainpanel,Savepanel,Settingpanel;
    public CanvasGroup CutscenePanel;
    void Start(){
        GameObject obj = GameObject.Find("CutscenePanel");
        CutscenePanel = obj.GetComponent<CanvasGroup>();
        CutscenePanel.DOFade(1f,0f);
        CutscenePanel.DOFade(0f,100f*Time.deltaTime);
        Mainpanel    = true;
        Savepanel    = false;
        Settingpanel = false;
    }
    void Update(){
        if (Input.GetMouseButtonDown(0)){
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            var Options = FindObjectOfType<ChangePanel>();
            foreach (RaycastResult result in results){
                switch(result.gameObject.name){
                    case "Start":
                        FadeOut();
                        StartCoroutine(Cutscene());
                        break;
                    case "Continue":
                        print("Continue");
                        GameObject.Find("Stage 1").GetComponent<ChangePanel>().MoveInPanel();
                        GameObject.Find("Start").GetComponent<ChangePanel>().MoveOutPanel();
                        break;
                    case "Setting":
                        print("Setting");
                        // GameObject.Find("Stage 1").GetComponent<ChangePanel>().MoveInPanel();
                        // GameObject.Find("Start").GetComponent<ChangePanel>().MoveOutPanel();
                        break;
                    case "Exit":
                        print("GameExit");
                        Application.Quit();
                        break;
                    case "Stage 1":
                    case "Stage 2":
                    case "Stage 3":
                        break;
                    case "Back":
                        GameObject.Find("Stage 1").GetComponent<ChangePanel>().MoveOutPanel();
                        GameObject.Find("Start").GetComponent<ChangePanel>().MoveInPanel();
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
    IEnumerator Cutscene(){
        yield return new WaitForSeconds(160f*Time.deltaTime);
        print("GameStart");
        SceneManager.LoadScene("Stage1");
    }
}

    