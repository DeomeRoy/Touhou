using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DG.Tweening;
// [HideInInspector]

class Boss_A : MonoBehaviour{
    public GameObject Bullet,Bullet_A,Bullet_C,Bullet_D,BOSS,B_BlockPrefab,E_BlockPrefab;
    public Transform Target,BossTransform,Transform_SATK_D;
    public Sprite Idle,Walk,ORG_Ammo;
    public bool AutoAttackTimer,NATK_A,NATK_B,SATK_A,SATK_B,SATK_C,SATK_D,OnMove,OnAttack,End,ColorChange;
    public float GapA,GapB,GapSpeed,AttackTimes;
    public float SkillTime,BulletSpeed,MoveSpeed,PositionX,PositionY,BossHP,PlayerHP,DropTimer;

    public bool NA,NB,SA,SB,SC,SD,AHP;
    //定義Boss中心點,下次移動位置,上個移動位置
    Vector3 SetUPosition,MovePosition,LastPosition;
    //Boss碰撞箱
    private CircleCollider2D CircleCollider;
    private CapsuleCollider2D CapsuleCollider;

    public void Start(){
        //Boss初始位置設定,攻擊角度初始化,技能間隔初始化,計時歸零,顏色轉換初始化
        SetUPosition = BOSS.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GapA = 0;GapB = 0;
        AttackTimes = 0;
        ColorChange = false;
        //初始上個位置為初始位置,預設自動攻擊關(對話後會開)
        LastPosition = SetUPosition;
        AutoAttackTimer = false;
        //正在攻擊預設關
        OnAttack = false;
        //Boss血量與戰鬥是否結束(預設否
        BossHP = 50;
        End = false;
        //初始化碰撞箱
        CircleCollider = GetComponent<CircleCollider2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        CircleCollider.enabled = false;
        CapsuleCollider.enabled = true;
    }
    public void Update(){
        if(AHP){
            AHP = false;
            LoseLife(1);
        }
        if(AutoAttackTimer){
            DropTimer += Time.deltaTime;
        }
        SkillTime += Time.deltaTime;
        {//--------------------------------------------------------------自動攻擊邏輯
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
            }
        {//--------------------------------------------------------------Boss動畫
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
        }
        {//--------------------------------------------------------------Boss招式
        if(NA){
                SkillStart(ref NA,ref OnMove,ref OnAttack,ref NATK_A,0f);
            }
        if(NATK_A == true){
                BOSS.transform.DOMove(new Vector3(SetUPosition.x,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
                if(SkillTime > 1f || BOSS.transform.position.x ==0){
                    OnMove = false;
                }
                if(SkillTime - GapA > 1f && SkillTime > 1f){
                    GapA = SkillTime;
                    float x = Random.Range(1,21);
                    for(int i=0;i<8;i++){
                        BulletSpeed = 3f;
                        transform.rotation = Quaternion.Euler(0, 0, i*45+5*x);
                        GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                        bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    }
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    if(SkillTime>5){
                        OnMove = false;
                        SkillEnd(ref NATK_A,0);
                    }
                }
            }
        if(NB){
                SkillStart(ref NB,ref OnMove,ref OnAttack,ref NATK_B,0f);
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
                PositionY = Random.Range(SetUPosition.y+0.5f,SetUPosition.y-2.5f);
                MovePosition = new Vector3(PositionX, PositionY, BOSS.transform.position.z);
                MoveSpeed = Mathf.Abs(BOSS.transform.position.x-PositionX)/4f;
            }
        if(NATK_B == true){
                MoveCollderChange();
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
                    MoveCollderChange();
                    if(SkillTime>4f){
                        OnMove = false;
                        SkillEnd(ref NATK_B,0);
                    }
                }
            }
        if(SA){
                SkillStart(ref SA,ref OnMove,ref OnAttack,ref SATK_A,0f);
            }
        if(SATK_A == true){
                if(SkillTime < 5){
                    BOSS.transform.DOMove(new Vector3(SetUPosition.x-8,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
                }
                if(SkillTime > 5){
                    BOSS.transform.DOMove(new Vector3(SetUPosition.x+8,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
                }
                if((5f > SkillTime && SkillTime > 1f) || (11f > SkillTime && SkillTime > 6f)){
                    OnMove = false;
                }
                else{
                    OnMove = true;
                }
                if(SkillTime - GapA > 1f && ((5f > SkillTime && SkillTime > 1f) || (11f > SkillTime && SkillTime > 6f))){
                    GapA = SkillTime;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    Instantiate(Bullet_A, BossTransform.position, BossTransform.rotation);
                }
                if(SkillTime>10){
                    SkillEnd(ref SATK_A,0);
                }
            }
        if(SB){
                SkillStart(ref SB,ref OnMove,ref OnAttack,ref SATK_B,0f);
                BulletSpeed = 1f;
                CircleCollider.enabled = true;
                CapsuleCollider.enabled = false;
            }
        if(SATK_B == true){
                BOSS.transform.DOMove(new Vector3(SetUPosition.x,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
                if(SkillTime > 1f || BOSS.transform.position.x ==0){
                    OnMove = false;
                }
                transform.Rotate(0, 0, 180 * Time.deltaTime);
                if(SkillTime - GapA > 0.05f && SkillTime > 1f){
                    GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    ColorChange = !ColorChange;
                    if(ColorChange){
                        bulletA.GetComponent<SpriteRenderer>().sprite = ORG_Ammo;
                    }
                    GapA = SkillTime;
                }
                if(SkillTime>12){
                    SkillEnd(ref SATK_B,-4);
                }
            }
        if(SC){
                SkillStart(ref SC,ref OnMove,ref OnAttack,ref SATK_C,0f);
            }
        if(SATK_C == true){
                BOSS.transform.DOMove(new Vector3(SetUPosition.x,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
                if(SkillTime > 1f || BOSS.transform.position.x ==0){
                    OnMove = false;
                }
                if(SkillTime - GapA > 0.15f && SkillTime > 1f){
                    GapA = SkillTime;
                    for(int i=-1;i<2;i++){
                        BulletSpeed = 3f;
                        transform.rotation = Quaternion.Euler(0, 0, 0 + i*45);
                        GameObject bulletA = Instantiate(Bullet, BossTransform.position, BossTransform.rotation);
                        bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    }
                    if(SkillTime - GapB > 1f){
                        GapB = SkillTime;
                        float x = Random.Range(-20,20);
                        for(int i=-6;i<7;i++){
                            transform.rotation = Quaternion.Euler(0, 0, 0 + i*15+x);
                            GameObject bulletA = Instantiate(Bullet_C, BossTransform.position, BossTransform.rotation);
                            bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                        }
                    }
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if(SkillTime>10){
                    SkillEnd(ref SATK_C,0);
                }
            }
        if(SD){
                SkillStart(ref SD,ref OnMove,ref OnAttack,ref SATK_D,0f);
                BulletSpeed = 2f;
            }
        if(SATK_D == true){
                BOSS.transform.DOMove(new Vector3(SetUPosition.x+8,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
                if(SkillTime > 1f){
                    OnMove = false;
                }
                if(SkillTime - GapA > 0.2f && SkillTime > 1f){
                    GameObject bulletA = Instantiate(Bullet_D, Transform_SATK_D.position, BossTransform.rotation);
                    bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                    GapA = SkillTime;
                }
                if(SkillTime>15){
                    SkillEnd(ref SATK_D,0);
                }
            }
        }
        {//Boss血量歸零的判定程式(開始對話+關閉計時器+隱藏球+玩家無敵+強制結束招式+避免重複判定
        if(BossHP <= 0 && !End){
            StartCoroutine(TriggerStoryByDistance(1));
            AutoAttackTimer = false;
            BallBehavior Ball = FindObjectOfType<BallBehavior>();
            PlayerController Player = FindObjectOfType<PlayerController>();
            Ball.LevelChanging();
            Player.invincible = true;
            SkillTime = 100;
            End = true;
        }
        }
        {//Boss掉落物程式
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHP = Player.GetComponent<PlayerController>().life;
        if(DropTimer > 10f){
            if(PlayerHP < 50){
                if(Random.value < 0.5f){
                    Instantiate(E_BlockPrefab, new Vector3(Random.Range(-8f, 8f),Transform_SATK_D.position.y-1.8f), Quaternion.identity);
                    DropTimer = 0;
                }
                else{
                    Instantiate(B_BlockPrefab, new Vector3(Random.Range(-8f, 8f),Transform_SATK_D.position.y-1.8f), Quaternion.identity);
                    DropTimer = 0;
                }
            }
            else{
                    Instantiate(B_BlockPrefab, new Vector3(Random.Range(-8f, 8f),Transform_SATK_D.position.y-1.8f), Quaternion.identity);
                    DropTimer = 0;
            }
        }
        }
    }
    //Boss移動時的碰撞箱轉換
    public void MoveCollderChange(){
        if(MovePosition.x > 0){
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0.5f,0.3f);
            transform.rotation = Quaternion.Euler(0, 0, -12.5f);
        }
        else if(MovePosition.x < 0){
            GetComponent<CapsuleCollider2D>().offset = new Vector2(-0.5f,0.3f);
            transform.rotation = Quaternion.Euler(0, 0, 12.5f);
        }
    }
    //對話結束恢復計時器
    public void ChatEnd(){
        AutoAttackTimer = !AutoAttackTimer;
    }
    //進入勝利劇情
    private IEnumerator TriggerStoryByDistance(float totalTransitionTime)
    {
        BOSS.GetComponent<SpriteRenderer>().enabled = false;
        float fadeOutTime = totalTransitionTime * 0.7f;//關卡淡出
        float fadeInTime = totalTransitionTime * 0.3f;//劇情淡入

        SceneAudioManager.Instance.FadeOutSceneMusic(fadeOutTime);
        yield return new WaitForSeconds(fadeOutTime + 0.167f);
        SceneAudioManager.Instance.PlayStoryMusicWithFadeIn(fadeInTime);

        StoryController storyCtrl = FindObjectOfType<StoryController>();
        storyCtrl.StartStory("BossEndFight");
    }
    //Boss碰到球的判定
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ball")){
            LoseLife(1);
        }
    }
    //Boss的扣血判定
    public void LoseLife(int x){
        switch(x){
            case 1:
                BossHP -= 5;
                break;
            case 2:
                BossHP -= 30;
                break;
        }
        Boss_HP_Bar_Follow UpdateBossHP = FindObjectOfType<Boss_HP_Bar_Follow>();
        UpdateBossHP.UpdateBossHP(BossHP);
    }
    //技能啟動
    public void SkillStart(ref bool AutoSkill,ref bool Move,ref bool Attack,ref bool SkillName,float Time){
        AutoSkill = false;
        Move = true;
        Attack = true;
        SkillName = true;
        SkillTime = Time;
    }
    //技能結束
    public void SkillEnd(ref bool SkillName,float Time){
        SkillName = false;
        SkillTime = Time;

        OnAttack = false;
        GapA = 0;
        GapB = 0;
        GetComponent<CapsuleCollider2D>().offset = new Vector2(0,0.3f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        CircleCollider.enabled = false;
        CapsuleCollider.enabled = true;
    }
}
