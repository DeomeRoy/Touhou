using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

// [HideInInspector]

class Boss_B : MonoBehaviour{
    public GameObject BOSS,Bullet,Bullet_NC,Bullet_NC1,Bullet_ND,Bullet_SD,B_BlockPrefab,E_BlockPrefab;
    public Transform Target,BossTransform,Transform_SATK_C,Transform_SATK_D;
    public Sprite Idle,Walk;
    public bool AutoAttackTimer,OnMove,OnAttack,End;
    public bool NATK_A,NATK_B,NATK_C,NATK_D,SATK_A,SATK_B,SATK_C,SATK_D;
    public float GapA,GapB,angle;
    public float SkillTime,BulletSpeed,MoveSpeed,PositionX,PositionY,BossHP,PlayerHP,DropTimer;
    //定義Boss中心點,下次移動位置,上個移動位置
    Vector3 SetUPosition,MovePosition,LastPosition,Direction;

    public bool NA,NB,NC,ND,SA,SB,SC,SD,AHP;
    public float AttackTimes;
    //Boss碰撞箱
    private CircleCollider2D CircleCollider;
    private CapsuleCollider2D CapsuleCollider;
    
    public void Start()
    {
        //Boss初始位置設定,攻擊角度初始化,技能間隔初始化,計時歸零,顏色轉換初始化
        SetUPosition = BOSS.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GapA = 0; GapB = 0;
        AttackTimes = 0;
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
        //--------------------------------------------------------------自動攻擊邏輯
        if(Input.GetKeyDown(KeyCode.F)){
            AutoAttackTimer = !AutoAttackTimer;
        }
        if(AutoAttackTimer && !OnAttack){
            if(SkillTime > 4){
                if(AttackTimes<2){
                    int x = Random.Range(0,4);
                    Debug.Log("N:" + x);
                    switch(x){
                        case 0:
                        NA = true;
                        break;
                        case 1:
                        NB = true;
                        break;
                        case 2:
                        NC = true;
                        break;
                        case 3:
                        ND = true;
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
            SkillStart(ref NA,ref OnMove,ref OnAttack,ref NATK_A,-1f);
            Move();
            CircleCollider.enabled = true;
            CapsuleCollider.enabled = false;
        }
        if(NATK_A == true){
            BOSS.transform.DOMove(MovePosition, 1f).SetEase(Ease.OutQuad);
            if(SkillTime > 0f){
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
                    SkillEnd(ref NATK_A,0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.E)||NB){
            SkillStart(ref NB,ref OnMove,ref OnAttack,ref NATK_B,-1f);
            Move();
        }
        if(NATK_B == true){
            BOSS.transform.DOMove(MovePosition, 1f).SetEase(Ease.OutQuad);
            if(SkillTime > 0f){
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
                    SkillEnd(ref NATK_B,0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.R)||NC){
            SkillStart(ref NC,ref OnMove,ref OnAttack,ref NATK_C,-1f);
            Move();
            CircleCollider.enabled = true;
            CapsuleCollider.enabled = false;
        }
        if(NATK_C == true){
            BOSS.transform.DOMove(MovePosition, 1f).SetEase(Ease.OutQuad);
            if(SkillTime > 0f){
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
                    SkillEnd(ref NATK_C,0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.T)||ND){
            SkillStart(ref ND,ref OnMove,ref OnAttack,ref NATK_D,-1f);
            CircleCollider.enabled = true;
            CapsuleCollider.enabled = false;
        }
        if(NATK_D == true){
            BOSS.transform.DOMove(new Vector3(SetUPosition.x,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
            if(SkillTime > 0f || BOSS.transform.position == SetUPosition){
                OnMove = false;
            }
            if(SkillTime - GapA > 0.25f){
                GapA = SkillTime;
                float x = Random.Range(0, 2) == 0 ? -1 : 1;
                transform.rotation = Quaternion.Euler(0, 0, 100*x);
                GameObject bulletA = Instantiate(Bullet_ND, BossTransform.position, BossTransform.rotation);
                if(SkillTime>2f){
                    SkillEnd(ref NATK_D,0);
                }
            }

        }
        if(Input.GetKeyDown(KeyCode.Y)||SA){
            SkillStart(ref SA,ref OnMove,ref OnAttack,ref SATK_A,-1f);
            CircleCollider.enabled = true;
            CapsuleCollider.enabled = false;
        }
        if(SATK_A == true){
            BOSS.transform.DOMove(new Vector3(SetUPosition.x,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
            if(SkillTime > 0f || BOSS.transform.position == SetUPosition){
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
                    SkillEnd(ref SATK_A,0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.U)||SB){
            SkillStart(ref SB,ref OnMove,ref OnAttack,ref SATK_B,-1f);
            CircleCollider.enabled = true;
            CapsuleCollider.enabled = false;
        }
        if(SATK_B == true){
            BOSS.transform.DOMove(new Vector3(SetUPosition.x,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
            if(SkillTime > 0f || BOSS.transform.position == SetUPosition){
                OnMove = false;
            }
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
                    SkillEnd(ref SATK_B,0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.I)||SC){
            SkillStart(ref SC,ref OnMove,ref OnAttack,ref SATK_C,-1f);
        }
        if(SATK_C == true){
            BOSS.transform.DOMove(new Vector3(SetUPosition.x,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
            if(SkillTime > 0f || BOSS.transform.position == SetUPosition){
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
                SkillEnd(ref SATK_C,0);
            }
        }
        if(Input.GetKeyDown(KeyCode.O)||SD){
            SkillStart(ref SD,ref OnMove,ref OnAttack,ref SATK_D,-1f);
        }
        if(SATK_D == true){
            BOSS.transform.DOMove(new Vector3(SetUPosition.x,SetUPosition.y), 1f).SetEase(Ease.OutQuad);
            if(SkillTime > 0f || BOSS.transform.position == SetUPosition){
                OnMove = false;
            }
            if(SkillTime - GapA > 0.5f){
                if(SkillTime < 9.1){
                    BulletSpeed = 1f;
                    GapA = SkillTime;
                    GameObject bulletA = Instantiate(Bullet_SD, Transform_SATK_D.position, BossTransform.rotation);
                    float BulletA_Position = (9f - SkillTime)*-1;
                    bulletA.GetComponent<Transform>().transform.position = new Vector3(BulletA_Position,24.36f,0);
                    GameObject bulletB = Instantiate(Bullet_SD, Transform_SATK_D.position, BossTransform.rotation);
                    float BulletB_Position = 9f - SkillTime;
                    bulletB.GetComponent<Transform>().transform.position = new Vector3(BulletB_Position,24.36f,0);
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
                    bulletC.GetComponent<Transform>().transform.position = new Vector3(BulletC_Position,24.36f,0);
                    GameObject bulletD = Instantiate(Bullet_SD, Transform_SATK_D.position, BossTransform.rotation);
                    float BulletD_Position = 0 - SkillTime+9;
                    bulletD.GetComponent<Transform>().transform.position = new Vector3(BulletD_Position,24.36f,0);
                    Debug.Log(bulletC.GetComponent<Transform>().transform.position);
                    Debug.Log(bulletD.GetComponent<Transform>().transform.position);
                    bulletC.GetComponent<Rigidbody2D>().velocity = BossTransform.up * BulletSpeed;
                    bulletD.GetComponent<Rigidbody2D>().velocity = BossTransform.up * BulletSpeed;
                }
            }
            if(SkillTime>18){
                SkillEnd(ref SATK_D,0);
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
                    Instantiate(E_BlockPrefab, new Vector3(Random.Range(-8f, 8f),Transform_SATK_C.position.y-1.8f), Quaternion.identity);
                    DropTimer = 0;
                }
                else{
                    Instantiate(B_BlockPrefab, new Vector3(Random.Range(-8f, 8f),Transform_SATK_C.position.y-1.8f), Quaternion.identity);
                    DropTimer = 0;
                }
            }
            else{
                    Instantiate(B_BlockPrefab, new Vector3(Random.Range(-8f, 8f),Transform_SATK_C.position.y-1.8f), Quaternion.identity);
                    DropTimer = 0;
            }
        }
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
    //Boss招式的瞄準判定
    void Aim(){
        Direction = Target.position - BOSS.transform.position;
        angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
    }
    //Boss放招式的隨機移動
    public void Move(){
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
            PositionY = Random.Range(SetUPosition.y,SetUPosition.y-1f);
            MovePosition = new Vector3(PositionX, PositionY, BOSS.transform.position.z);
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


