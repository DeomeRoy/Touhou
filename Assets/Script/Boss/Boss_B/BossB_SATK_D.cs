using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossB_SATK_D : MonoBehaviour{
    Rigidbody2D rb;
    public float skilltime,Gap;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        Gap = 0.5f;
    }
    public void Update(){
        skilltime += Time.deltaTime;
        if(skilltime > Gap){
            Gap += skilltime;
            rb.gravityScale -= 0.1f;
        }
    }
}
