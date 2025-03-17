using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testplayer : MonoBehaviour{
    void Start(){
        
    }
    void Update(){
        if(Input.GetKey(KeyCode.A)){
            transform.Translate(-10f*Time.deltaTime,0,0);
        }
        if(Input.GetKey(KeyCode.D)){
            transform.Translate(10f*Time.deltaTime,0,0);
        }
    }
}
