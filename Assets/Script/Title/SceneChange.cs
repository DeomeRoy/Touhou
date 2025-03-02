using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour{
    public void BackToInformation(){
        SceneManager.LoadScene("Information");
    }
    public void BackToTitle(){
        SceneManager.LoadScene("TitleScene");
    }
    public void BackToBossSelect(){
        SceneManager.LoadScene("BossSelect");
    }
    public void Boss_A(){
        SceneManager.LoadScene("Boss_A");
    }
    public void Boss_B(){
        SceneManager.LoadScene("Boss_B");
    }
    public void Boss_C(){
        SceneManager.LoadScene("Boss_C");
    }
    public void Character(){
        SceneManager.LoadScene("Character");
    }
}
