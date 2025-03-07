using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP : MonoBehaviour{
    public void MP_Change(){
        if(GetComponent<RectTransform>().localScale != new Vector3(1f,1f,1f)){
            GetComponent<RectTransform>().localScale += new Vector3(0.1f,0f,0f);
        }
    }
    public void MP_return0(){
        GetComponent<RectTransform>().localScale = new Vector3(0f,0f,0f);
    }
}
