using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [HideInInspector]

class Boss_B : MonoBehaviour{
    public GameObject BOSS,Bullet,Bullet_NC,Bullet_NC1,Bullet_ND,Bullet_SD;
    //,,Bullet_C,Bullet_D;
    public Transform Target,BossTransform,Transform_SATK_C,Transform_SATK_D;
    public Sprite Idle,Walk;
    public bool AutoAttackTimer,OnMove,OnAttack;
    public bool NATK_A,NATK_B,NATK_C,NATK_D,SATK_A,SATK_B,SATK_C,SATK_D;
    public float GapA,GapB,angle;
    public float SkillTime,BulletSpeed,MoveSpeed,PositionX,PositionY;
    Vector3 SetUPosition,MovePosition,LastPosition,Direction;

    public bool NA,NB,NC,ND,SA,SB,SC,SD;
    public float AttackTimes;
    public void Start(){
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GapA = 0;GapB = 0;
        SetUPosition = BOSS.transform.position;
        LastPosition = SetUPosition;
        AutoAttackTimer = false;
        AttackTimes = 0;
        OnAttack = false;
    }
    public void Update(){
        SkillTime += Time.deltaTime;
        //--------------------------------------------------------------自動攻擊邏輯
        if(Input.GetKeyDown(KeyCode.F)){
            AutoAttackTimer = !AutoAttackTimer;
        }
        if(AutoAttackTimer && !OnAttack){
            if(SkillTime > 4){
                if(AttackTimes<2){
                    int x = Random.Range(0,2);
                    Debug.Log("N:" + x);
                    switch(x){
                        case 0:
                        NA = true;
                        break;
                        case 1:
                        NB = true;
                        break;
                    }
                    AttackTimes += 1;
                }
                else{
                    AttackTimes = 0;
                    int x = Random.Range(0,4);
                    Debug.Log("S:" + x);
                    switch(x){
                        case 0:
                        SA = true;
                        break;
                        case 1:
                        SB = true;
                        break;
                        case 2:
                        SC = true;
                        break;
                        case 3:
                        SD = true;
                        break;
                    }
                }
            }
        }
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
        if(Input.GetKeyDown(KeyCode.Q)||NA){
            NATK_A = true;
            NA = false;
            OnMove = true;
            OnAttack = true;
            SkillTime = -1f;
            Move();
        }
        if(NATK_A == true){
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, MovePosition, MoveSpeed * Time.deltaTime);
            if(BOSS.transform.position == MovePosition){
                OnMove = false;
            }
            if(SkillTime > 0f && SkillTime < 0.2f){
                Aim();
            }
            if(SkillTime - GapA > 0.1f){
                GapA = SkillTime;
                for(int i=0;i<4;i++){
                    BulletSpeed = 5f;
                    float x = Random.Range(-15f,16f);
                    transform.rotation = Quaternion.Euler(0, 0, 0 + angle + x + 90);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                }
                if(SkillTime>2f){
                    OnMove = false;
                    OnAttack = false;
                    NATK_A = false;
                    GapA = 0;
                    SkillTime = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.E)||NB){
            NATK_B = true;
            NB = false;
            OnMove = true;
            OnAttack = true;
            SkillTime = -1f;
            Move();
        }
        if(NATK_B == true){
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, MovePosition, MoveSpeed * Time.deltaTime);
            if(BOSS.transform.position == MovePosition){
                OnMove = false;
            }
            if(SkillTime - GapA > 0.2f){
                GapA = SkillTime;
                for(int i=0;i<2;i++){
                    BulletSpeed = 5f;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    GameObject bulletA = Instantiate(Bullet, new Vector3(BossTransform.position.x-0.2f+0.4f*i,BossTransform.position.y), BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                }
                if(SkillTime>1.6f){
                    OnMove = false;
                    OnAttack = false;
                    NATK_B = false;
                    GapA = 0;
                    SkillTime = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.R)||NC){
            NATK_C = true;
            NC = false;
            OnMove = true;
            OnAttack = true;
            SkillTime = -1f;
            Move();
        }
        if(NATK_C == true){
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, MovePosition, MoveSpeed * Time.deltaTime);
            if(BOSS.transform.position == MovePosition){
                OnMove = false;
            }
            if(SkillTime - GapA > 0.2f){
                GapA = SkillTime;
                for(int i=0;i<2;i++){
                    BulletSpeed = 5f;
                    transform.rotation = Quaternion.Euler(0, 0, -25+50*i);
                    GameObject bulletA = Instantiate(Bullet_NC, new Vector3(BossTransform.position.x-0.2f+0.4f*i,BossTransform.position.y-0.5f), BossTransform.rotation);
                    GameObject bulletB = Instantiate(Bullet_NC1, new Vector3(BossTransform.position.x-0.2f+0.4f*i,BossTransform.position.y-0.5f), BossTransform.rotation);
                }
                if(SkillTime>1.6f){
                    OnMove = false;
                    OnAttack = false;
                    NATK_C = false;
                    GapA = 0;
                    SkillTime = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.T)||ND){
            NATK_D = true;
            ND = false;
            OnMove = true;
            OnAttack = true;
            SkillTime = -1.2f;
            back();
        }
        if(NATK_D == true){
            if(BOSS.transform.position == SetUPosition){
                OnMove = false;
            }
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, SetUPosition, MoveSpeed * Time.deltaTime);
            if(SkillTime - GapA > 0.25f){
                GapA = SkillTime;
                float x = Random.Range(0, 2) == 0 ? -1 : 1;
                transform.rotation = Quaternion.Euler(0, 0, 100*x);
                GameObject bulletA = Instantiate(Bullet_ND, BossTransform.position, BossTransform.rotation);
                if(SkillTime>2f){
                    OnMove = false;
                    OnAttack = false;
                    NATK_D = false;
                    GapA = 0;
                    SkillTime = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }

        }
        if(Input.GetKeyDown(KeyCode.Y)||SA){
            SATK_A = true;
            SA = false;
            OnMove = true;
            OnAttack = true;
            SkillTime = -1f;
            back();
        }
        if(SATK_A == true){
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, SetUPosition, MoveSpeed * Time.deltaTime);
            if(BOSS.transform.position == SetUPosition){
                OnMove = false;
            }
            if(SkillTime - GapA > 1f){
                GapA = SkillTime;
                float x = Random.Range(1,21);
                for(int i=0;i<8;i++){
                    BulletSpeed = 3f;
                    transform.rotation = Quaternion.Euler(0, 0, i*45+5*x);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    bulletA.GetComponent<Transform>().transform.localScale += new Vector3(0.2f,0.2f,0.2f);
                }
                if(SkillTime>2f){
                    SATK_A = false;
                    GapA = 0;
                    OnMove = false;
                    OnAttack = false;
                    SkillTime = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.U)||SB){
            SATK_B = true;
            SB = false;
            OnMove = true;
            OnAttack = true;
            SkillTime = -1f;
            back();
        }
        if(SATK_B == true){
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, MovePosition, MoveSpeed * Time.deltaTime);
            if(SkillTime - GapA > 0.1f){
                GapA = SkillTime;
                if(SkillTime<1){
                    for(int i=0;i<3;i++){
                    BulletSpeed = 5f;
                    transform.rotation = Quaternion.Euler(0, 0, -10 + i*10);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    }
                }
                else if(SkillTime>1&&SkillTime<2){
                    for(int i=0;i<3;i++){
                    BulletSpeed = 5f;
                    transform.rotation = Quaternion.Euler(0, 0, -25 + i*25);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    }
                }
                else{
                    for(int i=0;i<3;i++){
                    BulletSpeed = 5f;
                    transform.rotation = Quaternion.Euler(0, 0, -40 + i*40);
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    }
                }
                if(SkillTime>3.1f){
                    OnMove = false;
                    SATK_B = false;
                    GapA = 0;
                    OnAttack = false;
                    SkillTime = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.I)||SC){
            SATK_C = true;
            SC = false;
            OnMove = true;
            OnAttack = true;
            SkillTime = -1.2f;
            back();
        }
        if(SATK_C == true){
            if(BOSS.transform.position == SetUPosition){
                OnMove = false;
            }
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, SetUPosition, MoveSpeed * Time.deltaTime);
            if(SkillTime - GapA > 0.1f){
                GameObject bulletA = Instantiate(Bullet, Transform_SATK_C.position, BossTransform.rotation);
                bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                bulletA.GetComponent<Rigidbody2D>().gravityScale = 1f;
                float Bullet_Position = Random.Range(-8f + SkillTime, -5f + SkillTime);
                bulletA.GetComponent<Transform>().transform.position += new Vector3(Bullet_Position,0,0);
                GapA = SkillTime;
            }
            if(SkillTime>13){
                SATK_C = false;
                GapA = 0;
                OnAttack = false;
                SkillTime = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.O)||SD){
            SATK_D = true;
            SD = false;
            OnMove = true;
            OnAttack = true;
            SkillTime = -1.2f;
            back();
        }
        if(SATK_D == true){
            if(BOSS.transform.position == SetUPosition){
                OnMove = false;
            }
            BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, SetUPosition, MoveSpeed * Time.deltaTime);
            if(SkillTime - GapA > 0.5f){
                if(SkillTime < 9.1){
                    BulletSpeed = 1f;
                    GapA = SkillTime;
                    GameObject bulletA = Instantiate(Bullet_SD, Transform_SATK_D.position, BossTransform.rotation);
                    float BulletA_Position = (9f - SkillTime)*-1;
                    bulletA.GetComponent<Transform>().transform.position = new Vector3(BulletA_Position,-5,0);
                    GameObject bulletB = Instantiate(Bullet_SD, Transform_SATK_D.position, BossTransform.rotation);
                    float BulletB_Position = 9f - SkillTime;
                    bulletB.GetComponent<Transform>().transform.position = new Vector3(BulletB_Position,-5,0);
                    Debug.Log(bulletA.GetComponent<Transform>().transform.position);
                    Debug.Log(bulletB.GetComponent<Transform>().transform.position);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * BulletSpeed;
                    bulletB.GetComponent<Rigidbody2D>().velocity = BossTransform.up * BulletSpeed;
                }
                else{
                    BulletSpeed = 1f;
                    GapA = SkillTime;
                    GameObject bulletC = Instantiate(Bullet_SD, Transform_SATK_D.position, BossTransform.rotation);
                    float BulletC_Position = (0 - SkillTime+9)*-1;
                    bulletC.GetComponent<Transform>().transform.position = new Vector3(BulletC_Position,-5,0);
                    GameObject bulletD = Instantiate(Bullet_SD, Transform_SATK_D.position, BossTransform.rotation);
                    float BulletD_Position = 0 - SkillTime+9;
                    bulletD.GetComponent<Transform>().transform.position = new Vector3(BulletD_Position,-5,0);
                    Debug.Log(bulletC.GetComponent<Transform>().transform.position);
                    Debug.Log(bulletD.GetComponent<Transform>().transform.position);
                    bulletC.GetComponent<Rigidbody2D>().velocity = BossTransform.up * BulletSpeed;
                    bulletD.GetComponent<Rigidbody2D>().velocity = BossTransform.up * BulletSpeed;
                }
            }
            if(SkillTime>18){
                SATK_D = false;
                GapA = 0;
                OnAttack = false;
                SkillTime = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    void Move(){
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
            MoveSpeed = Mathf.Abs(BOSS.transform.position.x-PositionX)*2;
    }
    void back(){
        MoveSpeed = Mathf.Abs(BOSS.transform.position.x-SetUPosition.x);
    }
    void Aim(){
        Direction = Target.position - BOSS.transform.position;
        angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
    }
}

// {
//     OnMove = true;
//             NATK_A = true;
//             SkillTime = 0f;
//             MoveSpeed = Mathf.Abs(BOSS.transform.position.x-SetUPosition.x);
//             NA = false;
//             OnAttack = true;
    
//             BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, SetUPosition, MoveSpeed * Time.deltaTime);
//             if(BOSS.transform.position == SetUPosition){
//                 OnMove = false;
//             }
//             if(SkillTime - GapA > 0.5f && BOSS.transform.position == SetUPosition){
//                 GapA = SkillTime;
//                 float x = Random.Range(5,25);
//                 for(int i=0;i<13;i++){
//                     BulletSpeed = 3f;
//                     transform.rotation = Quaternion.Euler(0, 0, 0 + i*30+x);
//                     GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
//                     bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
//                 }
//                 if(SkillTime>5){
//                     NATK_A = false;
//                     GapA = 0;
//                     OnMove = false;
//                     OnAttack = false;
//                     SkillTime = 0;
//                     transform.rotation = Quaternion.Euler(0, 0, 0);
//             }
//         }
// }
