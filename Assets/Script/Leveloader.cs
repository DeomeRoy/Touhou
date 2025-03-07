using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Leveloader : MonoBehaviour{
    public Transform Wall;
    public bool Open_flag,Stop_flag,Exit;
    public GameObject StopPanel,StopText,MenuPanel,StopButton,ExitButton;
    // public UnityEngine.UI.Image OnStop,UnStop;
    public void Start(){
        MenuPanel.SetActive(false);
        StopButton.SetActive(false);
        ExitButton.SetActive(false);
        StopPanel.SetActive(false);
        StopText.SetActive(false);
    }
    public void LevelPush(){
        GameObject.Find("Main Camera").GetComponent<CamaraMover>().LevelPush();
        GameObject.Find("TWall").GetComponent<WallMover>().LevelPush();
        GameObject.Find("Player").GetComponent<PlayerController>().LevelPush();
    }
    public void Openmenu(){
        if(Open_flag == false){
            Open_flag = true;
            MenuPanel.SetActive(true);
            StopButton.SetActive(true);
            ExitButton.SetActive(true);
        }
        else if(Open_flag == true){
            Open_flag = false;
            MenuPanel.SetActive(false);
            StopButton.SetActive(false);
            ExitButton.SetActive(false);
        }
    }
    public void Stop(){
        if(Stop_flag == false){
            Stop_flag = true;
            Time.timeScale = 0;
            StopPanel.SetActive(true);
            StopText.SetActive(true);
            StopButton.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("OnStop");
        }
        else if(Stop_flag == true){
            Stop_flag = false;
            Time.timeScale = 1;
            StopPanel.SetActive(false);
            StopText.SetActive(false);
            StopButton.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UnStop");
        }
    }
    public void ExitGame(){
        SceneManager.LoadScene("TitleScene");
    }
}
