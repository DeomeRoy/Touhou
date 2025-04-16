using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BossA_SATK_A : MonoBehaviour{
    public GameObject Player;
    public float Speed = 0.2f,FallSpeed = 0.3f;
    void Update(){
        Player = GameObject.FindGameObjectWithTag("Player");
        transform.Translate(0,-FallSpeed*Time.deltaTime,0);
        if(transform.position.x > Player.transform.position.x){
            transform.Translate(-Speed*Time.deltaTime,0,0);
        }
        else if(transform.position.x < Player.transform.position.x){
            transform.Translate(Speed*Time.deltaTime,0,0);
        }
    }
}
