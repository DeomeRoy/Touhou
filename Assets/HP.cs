using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour{
    public void HP_Change(int x){
        if(GetComponent<RectTransform>().localScale != new Vector3(0f,1f,1f)){
            GetComponent<RectTransform>().localScale = new Vector3(x/100f,1f,1f);
        }
    }
}
