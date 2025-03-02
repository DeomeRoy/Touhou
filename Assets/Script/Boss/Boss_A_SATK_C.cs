using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_A_SATK_C : MonoBehaviour{
    public GameObject Bullet_C;
    public float Gap = 0;
    public float Clock = 0;
    public void Update(){
        Clock += Time.deltaTime;
        if(Clock - Gap > 0.05f){
            Gap += 0.05f;
            transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
