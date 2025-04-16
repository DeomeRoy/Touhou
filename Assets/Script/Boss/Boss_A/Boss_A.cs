using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// [HideInInspector]

class Boss_A : MonoBehaviour{
    public GameObject Bullet,Bullet_A,Bullet_C,Bullet_D,BOSS;
    public Transform Target,BossTransform,Transform_SATK_D;
    public Sprite Idle,Walk;
    public bool AutoAttackTimer,NATK_A,NATK_B,SATK_A,SATK_B,SATK_C,SATK_D,OnMove,OnAttack,End;
    public float GapA,GapB;
    public float SkillTime,BulletSpeed,MoveSpeed,PositionX,PositionY,BossHP;
    Vector3 SetUPosition,MovePosition,LastPosition;

    public bool NA,NB,SA,SB,SC,SD;
    public float AttackTimes;
    public void Start(){
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GapA = 0;GapB = 0;
        SetUPosition = BOSS.transform.position;
        LastPosition = SetUPosition;
        AutoAttackTimer = false;
        AttackTimes = 0;
        OnAttack = false;
        BossHP = 10;
        End = false;
    }
    public void Update(){
        SkillTime += Time.deltaTime;
        //--------------------------------------------------------------自動攻擊邏輯
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
        if(NA){
                NA = false;
                OnMove = true;
                NATK_A = true;
                SkillTime = 0f;
                MoveSpeed = Mathf.Abs(BOSS.transform.position.x-SetUPosition.x);
                OnAttack = true;
            }
        if(NATK_A == true){
                BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, SetUPosition, MoveSpeed * Time.deltaTime);
                if(BOSS.transform.position == SetUPosition){
                    OnMove = false;
                }
                if(SkillTime - GapA > 1f && BOSS.transform.position == SetUPosition){
                    GapA = SkillTime;
                    float x = Random.Range(1,21);
                    for(int i=0;i<8;i++){
                        BulletSpeed = 3f;
                        transform.rotation = Quaternion.Euler(0, 0, i*45+5*x);
                        GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                        bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    }
                    if(SkillTime>5){
                        NATK_A = false;
                        GapA = 0;
                        OnMove = false;
                        OnAttack = false;
                        SkillTime = 0;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
            }
        if(NB){
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
                PositionY = Random.Range(SetUPosition.y+0.5f,SetUPosition.y-3.5f);
                MovePosition = new Vector3(PositionX, PositionY, BOSS.transform.position.z);
                MoveSpeed = Mathf.Abs(BOSS.transform.position.x-PositionX)/4f;
                NB = false;
                OnAttack = true;
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
                        OnAttack = false;
                        SkillTime = 0;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
            }
        if(SA){
                OnMove = true;
                SATK_A = true;
                SkillTime = 0f;
                MoveSpeed = 15;
                SA = false;
                OnAttack = true;
            }
        if(SATK_A == true){
                if(SkillTime < 5){
                    BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position,new Vector3(SetUPosition.x-8,SetUPosition.y,0), MoveSpeed * Time.deltaTime);
                }
                if(SkillTime > 5){
                    BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position,new Vector3(SetUPosition.x+8,SetUPosition.y,0), MoveSpeed * Time.deltaTime);
                }
                if(BOSS.transform.position == new Vector3(SetUPosition.x-8,SetUPosition.y,0)||BOSS.transform.position == new Vector3(SetUPosition.x+8,SetUPosition.y,0)){
                    OnMove = false;
                }
                else{
                    OnMove = true;
                }
                if(SkillTime - GapA > 1f && (BOSS.transform.position == new Vector3(SetUPosition.x-8,SetUPosition.y,0)||BOSS.transform.position == new Vector3(SetUPosition.x+8,SetUPosition.y,0))){
                    GapA = SkillTime;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    Instantiate(Bullet_A, BossTransform.position, BossTransform.rotation);
                }
                if(SkillTime>10){
                        OnMove = false;
                        SATK_A = false;
                        GapA = 0;
                        OnAttack = false;
                        SkillTime = 0;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        if(SB){
                BulletSpeed = 1f;
                SATK_B = true;
                SkillTime = 0f;
                SB = false;
                OnAttack = true;
                MoveSpeed = Mathf.Abs(BOSS.transform.position.x-SetUPosition.x);
                OnMove = true;
            }
        if(SATK_B == true){
                if(BOSS.transform.position == SetUPosition){
                    OnMove = false;
                }
                BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position, SetUPosition, MoveSpeed * Time.deltaTime);
                transform.Rotate(0, 0, 180 * Time.deltaTime);
                if(SkillTime - GapA > 0.05f && BOSS.transform.position == SetUPosition){
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    GapA = SkillTime;
                }
                if(SkillTime>12){
                    SATK_B = false;
                    GapA = 0;
                    OnAttack = false;
                    SkillTime = -4;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        if(SC){
                OnMove = true;
                SATK_C = true;
                SkillTime = 0f;
                MoveSpeed = Mathf.Abs(BOSS.transform.position.x-SetUPosition.x);
                SC = false;
                OnAttack = true;
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
                    OnAttack = false;
                    SkillTime = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        if(SD){
                OnMove = true;
                BulletSpeed = 2f;
                SATK_D = true;
                SkillTime = 0f;
                MoveSpeed = Mathf.Abs(BOSS.transform.position.x-8);
                SD = false;
                OnAttack = true;
            }
        if(SATK_D == true){
                BOSS.transform.position = Vector3.MoveTowards(BOSS.transform.position,new Vector3(SetUPosition.x+8,SetUPosition.y,0), MoveSpeed * Time.deltaTime);
                if(BOSS.transform.position == new Vector3(8,3,0)){
                    OnMove = false;
                }
                if(SkillTime - GapA > 0.2f && BOSS.transform.position == new Vector3(SetUPosition.x+8,SetUPosition.y,0)){
                    GameObject bulletA = Instantiate(Bullet_D, Transform_SATK_D.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    GapA = SkillTime;
                }
                if(SkillTime>15){
                    SATK_D = false;
                    GapA = 0;
                    OnAttack = false;
                    SkillTime = 0;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        if(BossHP <= 0 && !End){
            StartCoroutine(TriggerStoryByDistance(1));
            AutoAttackTimer = false;
            BallBehavior Ball = FindObjectOfType<BallBehavior>();
            Ball.LevelChanging();
            End = true;
        }
    }
    public void ChatEnd(){
        AutoAttackTimer = !AutoAttackTimer;
    }
    public void LoseLife(int x){
        switch(x){
            case 1:
                BossHP -= 5;
                break;
            case 2:
                BossHP -= 30;
                break;
        }
    }
    private IEnumerator TriggerStoryByDistance(float totalTransitionTime)//進入劇情的引用
    {
        BOSS.GetComponent<SpriteRenderer>().enabled = false;
        float fadeOutTime = totalTransitionTime * 0.7f;//關卡淡出
        float fadeInTime = totalTransitionTime * 0.3f;//劇情淡入

        SceneAudioManager.Instance.FadeOutSceneMusic(fadeOutTime);
        yield return new WaitForSeconds(fadeOutTime);
        SceneAudioManager.Instance.PlayStoryMusicWithFadeIn(fadeInTime);

        string storyID = "BossEndFight";

        StoryController storyCtrl = FindObjectOfType<StoryController>();
        storyCtrl.StartStory(storyID);
    }
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ball")){
            LoseLife(1);
        }
    }
}
