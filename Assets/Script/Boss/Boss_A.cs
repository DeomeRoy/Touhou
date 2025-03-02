using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [HideInInspector]

class Boss_A : MonoBehaviour{
    public GameObject Bullet,Bullet_C,Bullet_D;
    public Transform Target,BossTransform,Transform_SATK_D;
    public bool NATK_A,NATK_B,SATK_A,SATK_B,SATK_C,SATK_D;
    [HideInInspector]public float GapA,GapB;
    public float SkillTime;
    public float BulletSpeed;
    public void Start(){
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GapA = 0;GapB = 0;
    }
    public void Update(){
        SkillTime += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Q)){
            NATK_A = true;
            SkillTime = 0f;
        }
        if(NATK_A == true){
            if(SkillTime - GapA > 0.3f){
                GapA = SkillTime + 0.3f;
                for(int i=0;i<13;i++){
                    BulletSpeed = 3f;
                    transform.rotation = Quaternion.Euler(0, 0, 0 + i*30);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                }
                if(SkillTime>10){
                    NATK_A = false;
                    GapA = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.E)){
            NATK_B = true;
            SkillTime = 0f;
        }
        if(NATK_B == true){
            if(SkillTime - GapA > 0.15f){
                GapA = SkillTime + 0.15f;
                for(int i=-1;i<2;i++){
                    BulletSpeed = 5f;
                    transform.rotation = Quaternion.Euler(0, 0, 0 + i*10);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                }
                if(SkillTime>0.8){
                    NATK_B = false;
                    GapA = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        // if(Input.GetKeyDown(KeyCode.A))
        // if(SATK_A == true)
        if(Input.GetKeyDown(KeyCode.B)){
            BulletSpeed = 1f;
            SATK_B = true;
            SkillTime = 0f;
        }
        if(SATK_B == true){
            transform.Rotate(0, 0, 180 * Time.deltaTime);
            if(SkillTime - GapA > 0.05f){
                GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                GapA = SkillTime + 0.05f;
            }
            if(SkillTime>12){
                SATK_B = false;
                GapA = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.C)){
            SATK_C = true;
            SkillTime = 0f;
        }
        if(SATK_C == true){
            if(SkillTime - GapA > 0.15f){
                GapA = SkillTime + 0.15f;
                for(int i=-1;i<2;i++){
                    BulletSpeed = 3f;
                    transform.rotation = Quaternion.Euler(0, 0, 0 + i*45);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                }
                if(SkillTime - GapB > 0.5f){
                    GapB = SkillTime + 0.5f;
                    for(int i=-6;i<7;i++){
                        transform.rotation = Quaternion.Euler(0, 0, 0 + i*15);
                        GameObject bulletA = Instantiate(Bullet_C, BossTransform.position, BossTransform.rotation);
                        bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    }
                }
            }
            if(SkillTime>10){
                SATK_C = false;
                GapA = 0;
                GapB = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.D)){
            BulletSpeed = 2f;
            SATK_D = true;
            SkillTime = 0f;
        }
        if(SATK_D == true){
            if(SkillTime - GapA > 0.1f){
                GameObject bulletA = Instantiate(Bullet_D, Transform_SATK_D.position, BossTransform.rotation);
                bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                GapA = SkillTime + 0.1f;
            }
            if(SkillTime>15){
                SATK_D = false;
                GapA = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
