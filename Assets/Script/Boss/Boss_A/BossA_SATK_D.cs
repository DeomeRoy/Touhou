using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossA_SATK_D : MonoBehaviour{
    void Start(){
        float Bullet_Position = Random.Range(-8f, 8f);
        transform.position += new Vector3(Bullet_Position,0,0);
        float Bullet_Size = Random.Range(0.01f, 0.15f);
        transform.localScale += new Vector3(Bullet_Size,Bullet_Size,Bullet_Size);
    }
}
