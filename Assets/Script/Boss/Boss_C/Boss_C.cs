using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;

// [HideInInspector]
// CircleCollider.enabled = true;
// CapsuleCollider.enabled = false;
// BOSS.transform.DOMove(MovePosition, 1f).SetEase(Ease.OutQuad);
class Boss_C : MonoBehaviour
{
    public GameObject BOSS, Bullet, Fire_Ball, Follow_Fire_Ball, Laser_Bullet, B_BlockPrefab, E_BlockPrefab;
    public GameObject Laser_A,Laser_B,Laser_C,Laser_D,Laser_E;
    public Transform Target, BossTransform, SATK_C_Transform;
    public Sprite Idle, Walk;
    public bool AutoAttackTimer, OnMove, OnAttack, End,gggMove;
    [HideInInspector] public bool NATK_A, NATK_B, NATK_C, NATK_D, SATK_A, SATK_B, SATK_C, SATK_D;
    public float GapA, GapB, angle;
    public float SkillTime, BulletSpeed, MoveSpeed, PositionX, BossHP, PlayerHP, DropTimer;
    //定義Boss中心點,下次移動位置,上個移動位置
    Vector3 SetUPosition, MovePosition, LastPosition, Direction;

    public bool NA, NB, NC, ND, SA, SB, SC, SD, AHP, BHP;
    public float AttackTimes;
    //Boss碰撞箱
    CircleCollider2D CircleCollider;
    CapsuleCollider2D CapsuleCollider;

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
        CircleCollider.enabled = true;
        CapsuleCollider.enabled = false;
    }
    public void Update()
    {
        if (gggMove){
            gggMove = false;
            Move();
        }
        if (AHP)
        {
            AHP = false;
            LoseLife(1);
        }
        if (AutoAttackTimer)
        {
            DropTimer += Time.deltaTime;
        }
        SkillTime += Time.deltaTime;
        //--------------------------------------------------------------自動攻擊邏輯
        if (Input.GetKeyDown(KeyCode.F))
        {
            AutoAttackTimer = !AutoAttackTimer;
        }
        if (AutoAttackTimer && !OnAttack)
        {
            if (SkillTime > 4)
            {
                if (AttackTimes < 2)
                {
                    int x = Random.Range(0, 4);
                    Debug.Log("N:" + x);
                    switch (x)
                    {
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
                else
                {
                    AttackTimes = 0;
                    int x = Random.Range(0, 4);
                    Debug.Log("S:" + x);
                    switch (x)
                    {
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
        if (OnMove)
        {
            BOSS.GetComponent<SpriteRenderer>().sprite = Walk;
            if (BOSS.transform.position.x - LastPosition.x < 0)
            {
                BOSS.GetComponent<SpriteRenderer>().flipX = true;
                LastPosition = BOSS.transform.position;
            }
            else if (BOSS.transform.position.x - LastPosition.x > 0)
            {
                BOSS.GetComponent<SpriteRenderer>().flipX = false;
                LastPosition = BOSS.transform.position;
            }
        }
        else
        {
            BOSS.GetComponent<SpriteRenderer>().sprite = Idle;
            BOSS.GetComponent<SpriteRenderer>().flipX = false;
        }
        //--------------------------------------------------------------BOSS招式
        if (Input.GetKeyDown(KeyCode.Q) || NA)
        {
            SkillStart(ref NA, ref OnMove, ref OnAttack, ref NATK_A, -1f);
            CircleCollider.enabled = true;
            CapsuleCollider.enabled = false;
        }
        if (NATK_A == true)
        {
            BOSS.transform.DOMove(SetUPosition, 1f).SetEase(Ease.OutQuad);
            if (SkillTime > 0f)
            {
                OnMove = false;
            }
            if (SkillTime - GapA > 2f)
            {
                GapA = SkillTime;
                BulletSpeed = 5f;
                transform.rotation = Quaternion.Euler(0, 0, 110);
                GameObject bulletA = Instantiate(Follow_Fire_Ball, new Vector3(BossTransform.position.x,BossTransform.position.y,0.5f), BossTransform.rotation);
                bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                transform.rotation = Quaternion.Euler(0, 0, -110);
                GameObject bulletB = Instantiate(Follow_Fire_Ball, new Vector3(BossTransform.position.x,BossTransform.position.y,0.5f), BossTransform.rotation);
                bulletB.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
            }
            if (SkillTime > 7f)
            {
                SkillEnd(ref NATK_A, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.E) || NB)
        {
            SkillStart(ref NB, ref OnMove, ref OnAttack, ref NATK_B, -1f);
            Move();
            MoveCollderChange();
            CircleCollider.enabled = false;
            CapsuleCollider.enabled = true;
        }
        if (NATK_B == true)
        {
            BOSS.transform.DOMove(MovePosition, 1f).SetEase(Ease.OutQuad);
            Laser_A.GetComponent<SpriteRenderer>().enabled = true;
            Laser_B.GetComponent<SpriteRenderer>().enabled = true;
            Laser_A.transform.DOMove(new Vector3(7,32,0), 1f).SetEase(Ease.OutQuad);
            Laser_B.transform.DOMove(new Vector3(-7,32,0), 1f).SetEase(Ease.OutQuad);
            Laser_A.transform.rotation = Quaternion.Euler(0f, 0f, -155f);
            Laser_B.transform.rotation = Quaternion.Euler(0f, 0f, -25f);
            if (SkillTime > 0f)
            {
                OnMove = false;
                GetComponent<CapsuleCollider2D>().offset = new Vector2(0.3f, -4.85f);
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = true;
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = true;
            }
            if (SkillTime > 1.6f)
            {
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_A.GetComponent<Laser_Tutorial>().Back();
                Laser_B.GetComponent<Laser_Tutorial>().Back();
                SkillEnd(ref NATK_B, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.R) || NC)
        {
            SkillStart(ref NC, ref OnMove, ref OnAttack, ref NATK_C, -1f);
            Move();
            MoveCollderChange();
            CircleCollider.enabled = false;
            CapsuleCollider.enabled = true;
        }
        if (NATK_C == true)
        {
            BOSS.transform.DOMove(MovePosition, 1f).SetEase(Ease.OutQuad);
            Laser_A.GetComponent<SpriteRenderer>().enabled = true;
            Laser_A.transform.DOMove(new Vector3(-0.5f,33,0), 1f).SetEase(Ease.OutQuad);
            Laser_A.transform.rotation = Quaternion.Euler(0f, 0f, -140f);
            if (SkillTime > 0f && SkillTime < 3f)
            {
                OnMove = false;
                GetComponent<CapsuleCollider2D>().offset = new Vector2(0.3f, -4.85f);
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = true;
                Laser_A.transform.DORotate(new Vector3(0, 0, -55f), 3f).SetEase(Ease.Linear);
            }
            if (SkillTime > 3f && SkillTime < 4f)
            {
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_B.GetComponent<SpriteRenderer>().enabled = true;
                Laser_B.transform.DOMove(new Vector3(0.5f,33,0), 1f).SetEase(Ease.OutQuad);
                Laser_B.transform.rotation = Quaternion.Euler(0f, 0f, -40f);
            }
            if (SkillTime > 4f && SkillTime < 7f)
            {
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = true;
                Laser_B.transform.DORotate(new Vector3(0, 0, -125), 3f).SetEase(Ease.Linear);
            }
            if (SkillTime > 7f && SkillTime < 9f)
            {
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_A.transform.DORotate(new Vector3(0, 0, -140f), 1f).SetEase(Ease.OutQuad);
                Laser_B.transform.DORotate(new Vector3(0, 0, -40f), 1f).SetEase(Ease.OutQuad);
            }
            if (SkillTime > 9f && SkillTime < 12f)
            {
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = true;
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = true;
                Laser_A.transform.DORotate(new Vector3(0, 0, -100f), 3f).SetEase(Ease.Linear);
                Laser_B.transform.DORotate(new Vector3(0, 0, -80f), 3f).SetEase(Ease.Linear);
            }
            if (SkillTime > 13f)
            {
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_A.GetComponent<Laser_Tutorial>().Back();
                Laser_B.GetComponent<Laser_Tutorial>().Back();
                SkillEnd(ref NATK_C, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.T) || ND)
        {
            SkillStart(ref ND, ref OnMove, ref OnAttack, ref NATK_D, -1f);
            Move();
            MoveCollderChange();
            CircleCollider.enabled = false;
            CapsuleCollider.enabled = true;
        }
        if (NATK_D == true)
        {
            BOSS.transform.DOMove(MovePosition, 1f).SetEase(Ease.OutQuad);
            Laser_A.GetComponent<SpriteRenderer>().enabled = true;
            Laser_B.GetComponent<SpriteRenderer>().enabled = true;
            Laser_C.GetComponent<SpriteRenderer>().enabled = true;
            Laser_D.GetComponent<SpriteRenderer>().enabled = true;
            Laser_A.transform.DOMove(new Vector3(-7,32,0), 1f).SetEase(Ease.OutQuad);
            Laser_B.transform.DOMove(new Vector3(-4,33,0), 1f).SetEase(Ease.OutQuad);
            Laser_C.transform.DOMove(new Vector3(4,33,0), 1f).SetEase(Ease.OutQuad);
            Laser_D.transform.DOMove(new Vector3(7,32,0), 1f).SetEase(Ease.OutQuad);
            Laser_A.transform.rotation = Quaternion.Euler(0f, 0f, -36f);
            Laser_B.transform.rotation = Quaternion.Euler(0f, 0f, -80f);
            Laser_C.transform.rotation = Quaternion.Euler(0f, 0f, -100f);
            Laser_D.transform.rotation = Quaternion.Euler(0f, 0f, -144f);
            if (SkillTime > 1.5f)
            {
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = true;
                OnMove = false;
                GetComponent<CapsuleCollider2D>().offset = new Vector2(0.3f, -4.85f);
            }
            if (SkillTime > 3f){Laser_C.GetComponent<Laser_Tutorial>().StartShoot = true;}
            if (SkillTime > 4.5f){Laser_A.GetComponent<Laser_Tutorial>().StartShoot = true;}
            if (SkillTime > 6f){Laser_D.GetComponent<Laser_Tutorial>().StartShoot = true;}
            if (SkillTime > 8f)
            {
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_C.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_D.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_A.GetComponent<Laser_Tutorial>().Back();
                Laser_B.GetComponent<Laser_Tutorial>().Back();
                Laser_C.GetComponent<Laser_Tutorial>().Back();
                Laser_D.GetComponent<Laser_Tutorial>().Back();
                SkillEnd(ref NATK_D, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Y) || SA)
        {
            SkillStart(ref SA, ref OnMove, ref OnAttack, ref SATK_A, -1f);
            CircleCollider.enabled = true;
            CapsuleCollider.enabled = false;
        }
        if (SATK_A == true)
        {
            BOSS.transform.DOMove(new Vector3(SetUPosition.x, SetUPosition.y,SetUPosition.z), 1f).SetEase(Ease.OutQuad);
            Laser_A.GetComponent<SpriteRenderer>().enabled = true;
            Laser_B.GetComponent<SpriteRenderer>().enabled = true;
            Laser_C.GetComponent<SpriteRenderer>().enabled = true;
            Laser_D.GetComponent<SpriteRenderer>().enabled = true;
            Laser_E.GetComponent<SpriteRenderer>().enabled = true;
            Laser_A.transform.DOMove(new Vector3(-7, 32, 0), 1f).SetEase(Ease.OutQuad);
            Laser_B.transform.DOMove(new Vector3(-5, 33, 0), 1f).SetEase(Ease.OutQuad);
            Laser_C.transform.DOMove(new Vector3(2, 32, 0), 1f).SetEase(Ease.OutQuad);
            Laser_D.transform.DOMove(new Vector3(6, 33, 0), 1f).SetEase(Ease.OutQuad);
            Laser_E.transform.DOMove(new Vector3(7.5f, 32, 0), 1f).SetEase(Ease.OutQuad);
            Laser_A.transform.rotation = Quaternion.Euler(0f, 0f, -65f);
            Laser_B.transform.rotation = Quaternion.Euler(0f, 0f, -45f);
            Laser_C.transform.rotation = Quaternion.Euler(0f, 0f, -135f);
            Laser_D.transform.rotation = Quaternion.Euler(0f, 0f, -105f);
            Laser_E.transform.rotation = Quaternion.Euler(0f, 0f, -140f);
            if (SkillTime > 0f)
                OnMove = false;
            if (SkillTime - GapA > 1f)
            {
                GapA = SkillTime;
                float x = Random.Range(0, 5);
                BulletSpeed = 20f;
                switch (x)
                {
                    case 0:
                        GameObject bulletA = Instantiate(Laser_Bullet, Laser_A.transform.position, Laser_A.transform.rotation* Quaternion.Euler(0, 0, 90));
                        bulletA.GetComponent<Rigidbody2D>().velocity = Laser_A.transform.right * BulletSpeed;
                        break;
                    case 1:
                        GameObject bulletB = Instantiate(Laser_Bullet, Laser_B.transform.position, Laser_B.transform.rotation* Quaternion.Euler(0, 0, 90));
                        bulletB.GetComponent<Rigidbody2D>().velocity = Laser_B.transform.right * BulletSpeed;
                        break;
                    case 2:
                        GameObject bulletC = Instantiate(Laser_Bullet, Laser_C.transform.position, Laser_C.transform.rotation* Quaternion.Euler(0, 0, 90));
                        bulletC.GetComponent<Rigidbody2D>().velocity = Laser_C.transform.right * BulletSpeed;
                        break;
                    case 3:
                        GameObject bulletD = Instantiate(Laser_Bullet, Laser_D.transform.position, Laser_D.transform.rotation* Quaternion.Euler(0, 0, 90));
                        bulletD.GetComponent<Rigidbody2D>().velocity = Laser_D.transform.right * BulletSpeed;
                        break;
                    case 4:
                        GameObject bulletE = Instantiate(Laser_Bullet, Laser_E.transform.position, Laser_E.transform.rotation* Quaternion.Euler(0, 0, 90));
                        bulletE.GetComponent<Rigidbody2D>().velocity = Laser_E.transform.right * BulletSpeed;
                        break;
                }
            }
            if (SkillTime - GapB > 0.35f)
            {
                GapB = SkillTime;
                float x = Random.Range(-8, 9);
                BulletSpeed = 8f;
                GameObject bulletA = Instantiate(Fire_Ball, new Vector3(x,SATK_C_Transform.position.y,SATK_C_Transform.position.z), BossTransform.rotation);
                bulletA.GetComponent<Rigidbody2D>().velocity = SATK_C_Transform.up * -BulletSpeed;
            }
            if (SkillTime > 15f)
            {
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_C.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_D.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_E.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_A.GetComponent<Laser_Tutorial>().Back();
                Laser_B.GetComponent<Laser_Tutorial>().Back();
                Laser_C.GetComponent<Laser_Tutorial>().Back();
                Laser_D.GetComponent<Laser_Tutorial>().Back();
                Laser_E.GetComponent<Laser_Tutorial>().Back();
                SkillEnd(ref SATK_A, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.U) || SB)
        {
            SkillStart(ref SB, ref OnMove, ref OnAttack, ref SATK_B, -1f);
            CircleCollider.enabled = true;
            CapsuleCollider.enabled = false;
        }
        if (SATK_B == true)
        {
            BOSS.transform.DOMove(new Vector3(SetUPosition.x, SetUPosition.y,SetUPosition.z), 1f).SetEase(Ease.OutQuad);
            Laser_A.GetComponent<SpriteRenderer>().enabled = true;
            Laser_B.GetComponent<SpriteRenderer>().enabled = true;
            Laser_A.transform.DOMove(new Vector3(-0.5f,33,0), 1f).SetEase(Ease.OutQuad);
            Laser_B.transform.DOMove(new Vector3(0.5f,33,0), 1f).SetEase(Ease.OutQuad);
            if (SkillTime > 0f)
            {
                OnMove = false;
                Vector2 dirA = Target.transform.position - Laser_A.transform.position;
                float Aangle = Mathf.Atan2(dirA.y, dirA.x) * Mathf.Rad2Deg;
                Vector2 dirB = Target.transform.position - Laser_B.transform.position;
                float Bangle = Mathf.Atan2(dirB.y, dirB.x) * Mathf.Rad2Deg;
                Laser_A.transform
                    .DORotate(new Vector3(0, 0, Aangle), 0.5f)
                    .SetEase(Ease.Linear);
                Laser_B.transform
                    .DORotate(new Vector3(0, 0, Bangle), 0.5f)
                    .SetEase(Ease.Linear);
            }
            if (SkillTime - GapA > 4f)
            {
                GapA = SkillTime;
                BulletSpeed = 20f;
                GameObject bulletA = Instantiate(Laser_Bullet, Laser_A.transform.position, Laser_A.transform.rotation * Quaternion.Euler(0, 0, 90));
                bulletA.GetComponent<Rigidbody2D>().velocity = Laser_A.transform.right * BulletSpeed;
                GameObject bulletB = Instantiate(Laser_Bullet, Laser_B.transform.position, Laser_B.transform.rotation* Quaternion.Euler(0, 0, 90));
                bulletB.GetComponent<Rigidbody2D>().velocity = Laser_B.transform.right * BulletSpeed;
            }
            if (SkillTime - GapB > 0.75f)
            {
                GapB = SkillTime;
                float x = Random.Range(-9, -7);
                BulletSpeed = 8f;
                GameObject bulletA = Instantiate(Fire_Ball, new Vector3(x + SkillTime, SATK_C_Transform.position.y, SATK_C_Transform.position.z), BossTransform.rotation);
                bulletA.GetComponent<Rigidbody2D>().velocity = SATK_C_Transform.up * -BulletSpeed;
            }
            if (SkillTime > 16)
            {
                Laser_A.GetComponent<Laser_Tutorial>().Back();
                Laser_B.GetComponent<Laser_Tutorial>().Back();
                SkillEnd(ref SATK_B, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.I) || SC)
        {
            SkillStart(ref SC, ref OnMove, ref OnAttack, ref SATK_C, -1f);
            CircleCollider.enabled = true;
            CapsuleCollider.enabled = false;
        }
        if (SATK_C == true)
        {
            BOSS.transform.DOMove(new Vector3(SetUPosition.x, SetUPosition.y,SetUPosition.z), 1f).SetEase(Ease.OutQuad);
            Laser_A.GetComponent<SpriteRenderer>().enabled = true;
            Laser_A.transform.DOMove(new Vector3(0,33,0), 1f).SetEase(Ease.OutQuad);
            if (SkillTime > 0f)
            {
                OnMove = false;
                Vector2 dirA = Target.transform.position - Laser_A.transform.position;
                float Aangle = Mathf.Atan2(dirA.y, dirA.x) * Mathf.Rad2Deg;
                Laser_A.transform
                    .DORotate(new Vector3(0, 0, Aangle), 0.5f)
                    .SetEase(Ease.Linear);
            }
            transform.Rotate(0, 0, 180 * Time.deltaTime);
            if(SkillTime - GapA > 0.05f && SkillTime > 1f){
                BulletSpeed = 1f;
                GameObject bulletA = Instantiate(Bullet, new Vector3(BossTransform.position.x,BossTransform.position.y,0.5f), BossTransform.rotation);
                bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
                GapA = SkillTime;
            }
            if (SkillTime - GapB > 5f)
            {
                GapB = SkillTime;
                BulletSpeed = 20f;
                GameObject bulletA = Instantiate(Laser_Bullet, Laser_A.transform.position, Laser_A.transform.rotation * Quaternion.Euler(0, 0, 90));
                bulletA.GetComponent<Rigidbody2D>().velocity = Laser_A.transform.right * BulletSpeed;
            }
            if (SkillTime > 12)
            {
                Laser_A.GetComponent<Laser_Tutorial>().Back();
                SkillEnd(ref SATK_C, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.O) || SD)
        {
            SkillStart(ref SD, ref OnMove, ref OnAttack, ref SATK_D, -1f);
        }
        if (SATK_D == true)
        {
            BOSS.transform.DOMove(new Vector3(SetUPosition.x, SetUPosition.y, SetUPosition.z), 1f).SetEase(Ease.OutQuad);
            Laser_A.GetComponent<SpriteRenderer>().enabled = true;
            Laser_B.GetComponent<SpriteRenderer>().enabled = true;
            Laser_C.GetComponent<SpriteRenderer>().enabled = true;
            Laser_D.GetComponent<SpriteRenderer>().enabled = true;
            Laser_E.GetComponent<SpriteRenderer>().enabled = true;
            Laser_A.transform.DOMove(new Vector3(0, 33, 0), 1f).SetEase(Ease.OutQuad);
            Laser_B.transform.DOMove(new Vector3(0, 33, 0), 1f).SetEase(Ease.OutQuad);
            Laser_C.transform.DOMove(new Vector3(0, 33, 0), 1f).SetEase(Ease.OutQuad);
            Laser_D.transform.DOMove(new Vector3(0, 33, 0), 1f).SetEase(Ease.OutQuad);
            Laser_E.transform.DOMove(new Vector3(0, 33, 0), 1f).SetEase(Ease.OutQuad);
            Laser_A.transform.rotation = Quaternion.Euler(0f, 0f, -40f);
            Laser_B.transform.rotation = Quaternion.Euler(0f, 0f, -65f);
            Laser_C.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Laser_D.transform.rotation = Quaternion.Euler(0f, 0f, -115f);
            Laser_E.transform.rotation = Quaternion.Euler(0f, 0f, -140f);
            if (SkillTime > 0f)
                OnMove = false;
            if (SkillTime - GapA > 0.15f)
            {
                GapA = SkillTime;
                BulletSpeed = 3f;
                float x = Random.Range(-45, 46);
                transform.rotation = Quaternion.Euler(0, 0, x);
                GameObject bulletA = Instantiate(Bullet, new Vector3(BossTransform.position.x,BossTransform.position.y,0.5f), BossTransform.rotation);
                bulletA.GetComponent<Rigidbody2D>().velocity = BossTransform.up * -BulletSpeed;
            }
            BulletSpeed = 20f;
            if (SkillTime > 2f)
            {
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = true;
            }
            if (SkillTime > 4f)
            {
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = true;
            }
            if (SkillTime > 6f)
            {
                Laser_C.GetComponent<Laser_Tutorial>().StartShoot = true;
            }
            if (SkillTime > 8f)
            {
                Laser_D.GetComponent<Laser_Tutorial>().StartShoot = true;
            }
            if (SkillTime > 10f)
            {
                Laser_E.GetComponent<Laser_Tutorial>().StartShoot = true;
            }
            if (SkillTime > 11)
            {
                Laser_A.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_B.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_C.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_D.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_E.GetComponent<Laser_Tutorial>().StartShoot = false;
                Laser_A.GetComponent<Laser_Tutorial>().Back();
                Laser_B.GetComponent<Laser_Tutorial>().Back();
                Laser_C.GetComponent<Laser_Tutorial>().Back();
                Laser_D.GetComponent<Laser_Tutorial>().Back();
                Laser_E.GetComponent<Laser_Tutorial>().Back();
                SkillEnd(ref SATK_D, 0);
            }
        }
        {//Boss血量歸零的判定程式(開始對話+關閉計時器+隱藏球+玩家無敵+強制結束招式+避免重複判定
            if (BossHP <= 0 && !End)
            {
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
            if (DropTimer > 10f)
            {
                if (PlayerHP < 50)
                {
                    if (Random.value < 0.5f)
                    {
                        // Instantiate(E_BlockPrefab, new Vector3(Random.Range(-8f, 8f), Transform_SATK_C.position.y - 1.8f), Quaternion.identity);
                        DropTimer = 0;
                    }
                    else
                    {
                        // Instantiate(B_BlockPrefab, new Vector3(Random.Range(-8f, 8f), Transform_SATK_C.position.y - 1.8f), Quaternion.identity);
                        DropTimer = 0;
                    }
                }
                else
                {
                    // Instantiate(B_BlockPrefab, new Vector3(Random.Range(-8f, 8f), Transform_SATK_C.position.y - 1.8f), Quaternion.identity);
                    DropTimer = 0;
                }
            }
        }
    }
    //對話結束恢復計時器
    public void ChatEnd()
    {
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            LoseLife(1);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("boom") && BHP == false)
        {
            BHP = true;
            LoseLife(2);
        }
        if (!collision.gameObject.CompareTag("boom") && BHP == true)
        {
            BHP = false;
        }
    }
    //Boss的扣血判定
    public void LoseLife(int x)
    {
        switch (x)
        {
            case 1:
                BossHP -= 5;
                break;
            case 2:
                if (BossHP >= 30)
                {
                    BossHP -= 30;
                }
                else if (BossHP < 30)
                {
                    BossHP = 0;
                }
                break;
        }
        Boss_HP_Bar_Follow UpdateBossHP = FindObjectOfType<Boss_HP_Bar_Follow>();
        UpdateBossHP.UpdateBossHP(BossHP);
    }
    //Boss移動時的碰撞箱轉換
    public void MoveCollderChange()
    {
        if (MovePosition.x > 0)
        {
            GetComponent<CapsuleCollider2D>().offset = new Vector2(4.05f, -1.1f);
            transform.rotation = Quaternion.Euler(0, 0, 0f);
        }
        else if (MovePosition.x < 0)
        {
            GetComponent<CapsuleCollider2D>().offset = new Vector2(-3.9f, -1.1f);
            transform.rotation = Quaternion.Euler(0, 0, 0f);
        }
    }
    //Boss招式的瞄準判定
    void Aim()
    {
        Direction = Target.position - BOSS.transform.position;
        angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
    }
    //Boss放招式的隨機移動
    public void Move()
    {
        if (transform.position.x > 0)
        {
            PositionX = Random.Range(-7.5f, -4f);
        }
        else if (transform.position.x < 0)
        {
            PositionX = Random.Range(7.5f, 4f);
        }
        else if (transform.position.x == 0)
        {
            while (true)
            {
                PositionX = Random.Range(-7.5f, 7.5f);
                if (PositionX < -4f || PositionX > 4f) break;
            }
        }
        MovePosition = new Vector3(PositionX, BOSS.transform.position.y, BOSS.transform.position.z);
    }
    //技能啟動
    public void SkillStart(ref bool AutoSkill, ref bool Move, ref bool Attack, ref bool SkillName, float Time)
    {
        AutoSkill = false;
        Move = true;
        Attack = true;
        SkillName = true;
        SkillTime = Time;
    }
    //技能結束
    public void SkillEnd(ref bool SkillName, float Time)
    {
        SkillName = false;
        SkillTime = Time;

        OnAttack = false;
        GapA = 0;
        GapB = 0;
        GetComponent<CapsuleCollider2D>().offset = new Vector2(0.3f, -1.1f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        CircleCollider.enabled = false;
        CapsuleCollider.enabled = true;
    }
}


