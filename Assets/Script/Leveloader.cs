using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Leveloader : MonoBehaviour{
    public Transform Wall,wall1_2,wall1_3,wall1_X;
    void Start(){
    }
    void Update(){
    }
    public void LevelPush(){
        GameObject.Find("Main Camera").GetComponent<CamaraMover>().LevelPush();
        GameObject.Find("TWall").GetComponent<WallMover>().LevelPush();
        GameObject.Find("Player").GetComponent<PlayerController>().LevelPush();
    }
}
