using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [HideInInspector]

class Boss_A : MonoBehaviour{
    public GameObject Bullet,Bullet_A,Bullet_C,Bullet_D,BOSS;
    public Transform Target,BossTransform,Transform_SATK_D;
    public Sprite Idle,Walk;
    public bool NATK_A,NATK_B,SATK_A,SATK_B,SATK_C,SATK_D,OnMove;
    [HideInInspector]public float GapA,GapB;
    public float SkillTime,BulletSpeed,MoveSpeed,PositionX,PositionY;
    Vector3 SetUPosition,MovePosition,LastPosition;
    public void Start(){
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GapA = 0;GapB = 0;
        SetUPosition = BOSS.transform.position;
        LastPosition = SetUPosition;
    }
    public void Update(){
        SkillTime += Time.deltaTime;
        //--------------------------------------------------------------BOSS動畫
        if(OnMove){
            BOSS.GetComponent<SpriteRenderer>().sprite = Walk;
            if(BOSS.transform.position.x-LastPosition.x < 0){
                BOSS.GetComponent<SpriteRenderer>().flipX = true;
                LastPosition = BOSS.transform.position;
            }
            else if(BOSS.transform.position.x-LastPosition.x > 0){
                BOSS.GetComponent<SpriteRenderer>().flipX = false;
                LastPosition = BOSS.transform.position;
            }
        }
        else{
            BOSS.GetComponent<SpriteRenderer>().sprite = Idle;
        }
        //--------------------------------------------------------------BOSS招式
        if(Input.GetKeyDown(KeyCode.Q)){
            OnMove = true;
            NATK_A = true;
            SkillTime = 0f;
            MoveSpeed = Mathf.Abs(BOSS.transform.position.x-SetUPosition.x);
        }
        if(NATK_A == true){
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, SetUPosition, MoveSpeed * Time.deltaTime);
            if(BOSS.transform.position == SetUPosition){
                OnMove = false;
            }
            if(SkillTime - GapA > 0.5f && BOSS.transform.position == SetUPosition){
                GapA = SkillTime;
                float x = Random.Range(5,25);
                for(int i=0;i<13;i++){
                    BulletSpeed = 3f;
                    transform.rotation = Quaternion.Euler(0, 0, 0 + i*30+x);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                }
                if(SkillTime>5){
                    NATK_A = false;
                    GapA = 0;
                    OnMove = false;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.E)){
            OnMove = true;
            NATK_B = true;
            SkillTime = 0f;
            if(transform.position.x > 0){
                PositionX = Random.Range(-7.5f,-4f);
                }
            else if(transform.position.x < 0){
                PositionX = Random.Range(7.5f,4f);
                }
            else if(transform.position.x == 0){
                while(true){
                    PositionX = Random.Range(-7.5f,7.5f);
                    if(PositionX < -4f || PositionX > 4f)break;
                }
            }
            PositionY = Random.Range(2.7f,4f);
            MovePosition = new Vector3(PositionX, PositionY, BOSS.transform.position.z);
            MoveSpeed = Mathf.Abs(BOSS.transform.position.x-PositionX)/4f;
        }
        if(NATK_B == true){
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, MovePosition, MoveSpeed * Time.deltaTime);
            if(SkillTime - GapA > 1f){
                GapA = SkillTime;
                for(int i=-1;i<2;i++){
                    BulletSpeed = 5f;
                    Vector3 Direction = Target.position - BOSS.transform.position;
                    float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, 0 + angle+i*10+90);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                }
                if(SkillTime>4f){
                    OnMove = false;
                    NATK_B = false;
                    GapA = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Y)){
            OnMove = true;
            SATK_A = true;
            SkillTime = 0f;
            MoveSpeed = Mathf.Abs(BOSS.transform.position.x-(-8));
        }
        if(SATK_A == true){
            if(SkillTime < 5){
                BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position,new Vector3(-8,3,0), MoveSpeed * Time.deltaTime);
            }
            if(SkillTime > 5){
                BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position,new Vector3(8,3,0), MoveSpeed * Time.deltaTime);
            }
            if(BOSS.transform.position == new Vector3(-8,3,0)||BOSS.transform.position == new Vector3(8,3,0)){
                OnMove = false;
            }
            else{
                OnMove = true;
            }
            if(SkillTime - GapA > 1f && (BOSS.transform.position == new Vector3(-8,3,0)||BOSS.transform.position == new Vector3(8,3,0))){
                GapA = SkillTime;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Instantiate(Bullet_A, BossTransform.position, BossTransform.rotation);
            }
            if(SkillTime>10){
                    OnMove = false;
                    SATK_A = false;
                    GapA = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
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
                GapA = SkillTime;
            }
            if(SkillTime>12){
                SATK_B = false;
                GapA = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.C)){
            OnMove = true;
            SATK_C = true;
            SkillTime = 0f;
            MoveSpeed = Mathf.Abs(BOSS.transform.position.x-SetUPosition.x);
        }
        if(SATK_C == true){
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, SetUPosition, MoveSpeed * Time.deltaTime);
            if(BOSS.transform.position == SetUPosition){
                OnMove = false;
            }
            if(SkillTime - GapA > 0.15f && BOSS.transform.position == SetUPosition){
                GapA = SkillTime;
                for(int i=-1;i<2;i++){
                    BulletSpeed = 3f;
                    transform.rotation = Quaternion.Euler(0, 0, 0 + i*45);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                }
                if(SkillTime - GapB > 1f && BOSS.transform.position == SetUPosition){
                    GapB = SkillTime;
                    float x = Random.Range(-20,20);
                    for(int i=-6;i<7;i++){
                        transform.rotation = Quaternion.Euler(0, 0, 0 + i*15+x);
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
        if(Input.GetKeyDown(KeyCode.U)){
            OnMove = true;
            BulletSpeed = 2f;
            SATK_D = true;
            SkillTime = 0f;
            MoveSpeed = Mathf.Abs(BOSS.transform.position.x-8);
        }
        if(SATK_D == true){
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position,new Vector3(8,3,0), MoveSpeed * Time.deltaTime);
            if(BOSS.transform.position == new Vector3(8,3,0)){
                OnMove = false;
            }
            if(SkillTime - GapA > 0.2f && BOSS.transform.position == new Vector3(8,3,0)){
                GameObject bulletA = Instantiate(Bullet_D, Transform_SATK_D.position, BossTransform.rotation);
                bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                GapA = SkillTime;
            }
            if(SkillTime>15){
                SATK_D = false;
                GapA = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
