using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject BOOM;
    public GameObject HP, MP;
    private BallBehavior ballScript;
    public bool attack_flag = false;
    public bool slide_attack_flag = false;
    public bool bullet_flag = false;
    public bool boom_flag = false;
    public bool slide_l_or_r;
    public bool bullet_exists = false;
    public bool controlEnabled = true;

    float attack_time = 0.3f;
    float slide_attack_time = 0.3f;
    float bullet_attack_time = 0.3f;
    float boom_attack_time = 0.3f;
    float boom_attack_cool = 5.0f;
    float attack_cnt = 0.0f;
    float slide_attack_cnt = 0.0f;
    float bullet_attack_cnt = 0.0f;
    float boom_attack_cnt = 0.0f;

    public float playermove = 9.0f;
    public float playerslide = 27.0f;
    public int score;
    public int life;
    public bool invincible = false;
    public float invincibilitytime = 0.5f;
    public bool die = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Rigidbody2D rb;
    public float strikeForceMultiplier = 25.0f;
    public float angleRangeModifier = 1.0f;
    public float angularForceMultiplier = 8.0f;
    private float lastExtraHit2Time = 10f;

    public Sprite idleSprite;
    public Sprite deadSprite;
    public Sprite[] walkSprites;
    public Sprite[] attackSprites;
    public Sprite[] bulletSprites;
    public Sprite[] slideSprites;
    public Sprite[] transitionRunSprites;

    public Vector2 idleSize = Vector2.one;
    public Vector2 deadSize = Vector2.one;
    public Vector2 walkSize = Vector2.one;
    public Vector2 attackSize = Vector2.one;
    public Vector2 slideSize = Vector2.one;
    public Vector2 bulletSize = Vector2.one;
    public Vector2 transitionRunSize = Vector2.one;

    public Vector2 idleOffset = Vector2.zero;
    public Vector2 deadOffset = Vector2.zero;
    public Vector2 walkOffset = Vector2.zero;
    public Vector2 attackOffset = Vector2.zero;
    public Vector2 slideOffset = Vector2.zero;
    public Vector2 bulletOffset = Vector2.zero;
    public Vector2 transitionRunOffset = Vector2.zero;

    private float walkTimer = 0.0f;
    public float walkswitch = 0.25f;
    public float transitionSwitch = 0.15f;

    private CircleCollider2D CircleCollider;
    private PolygonCollider2D PolygonCollider;
    private BoxCollider2D BoxCollider;
    private bool isFlipped = false;
    Vector3 targetPosition;
    int level;
    bool CheakPosition;

    //三角碰撞
    private float hypotenuseAngleRight = 45f;
    private float hypotenuseAngleLeft = 135f;
    private float angleTolerance = 15f;

    public void Start()
    {
        attack_cnt = 0.0f;
        slide_attack_cnt = 0.0f;
        bullet_attack_cnt = 0.0f;
        boom_attack_cnt = 0.0f;
        life = 100;
        score = 50;
        level = 1;
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer = transform.Find("SpriteContainer").GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        SetSprite(idleSprite, idleSize, idleOffset);

        if (GameManager.isContinue)
        {
            GameSaveData saveData = SaveManager.Instance.LoadGame();
            if (saveData != null && saveData.sceneName == SceneManager.GetActiveScene().name)
            {
                life = saveData.playerHP;
                score = saveData.playerMP;
                float yPos = GetYPositionForCase(saveData.masterCase);
                transform.position = new Vector3(0, yPos, transform.position.z);
                HP.GetComponent<HP>().HP_Change(life);
                MP.GetComponent<MP>().MP_Change(score);
                level = saveData.masterCase;
            }
        }
        else
        {
            //重新開始
            transform.position = new Vector3(0, -4.146f, transform.position.z);
        }

        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            ballScript = ball.GetComponent<BallBehavior>();
        }

        CircleCollider = GetComponent<CircleCollider2D>();
        PolygonCollider = GetComponent<PolygonCollider2D>();
        BoxCollider = GetComponent<BoxCollider2D>();
        BoxCollider.enabled = true;
        CircleCollider.enabled = false;
        PolygonCollider.enabled = false;
    }

    void Update()
    {
        if (Time.timeScale == 1)
        {
            attack_cnt += Time.deltaTime;
            slide_attack_cnt += Time.deltaTime;
            bullet_attack_cnt += Time.deltaTime;
            boom_attack_cnt += Time.deltaTime;
            bool hasLaunched = ballScript != null && ballScript.hasLaunched;

            if (die)
            {
                SetSprite(deadSprite, deadSize, deadOffset);
                controlEnabled = false; //禁用行動
                invincible = true;
                walkTimer = 0;
                rb.velocity = Vector2.zero;
            }

            if (attack_cnt >= attack_time)
            {
                attack_flag = false;
            }
            if (slide_attack_cnt >= slide_attack_time)
            {
                slide_attack_flag = false;
            }
            if (bullet_attack_cnt >= bullet_attack_time)
            {
                bullet_flag = false;
            }
            if (boom_attack_cnt >= boom_attack_cool)
            {
                boom_flag = false;
            }

            // 子彈 > 滑行 > 拍球 > 移動
            if (controlEnabled)
            {
                if (!bullet_flag)
                {
                    if (hasLaunched && Input.GetKey(KeyCode.Z) && !bullet_exists)
                    {
                        rb.velocity = Vector2.zero;
                        attack_flag = false;
                        slide_attack_flag = false;
                        bullet_flag = true;
                        bullet_exists = true;
                        bullet_attack_cnt = 0.0f;
                        GlobalAudioManager.Instance.PlayBulletSound();
                        if (bulletPrefab != null)
                        {
                            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                        }
                    }
                    else if (!slide_attack_flag)
                    {
                        if (Input.GetKey(KeyCode.LeftArrow))
                        {
                            if (hasLaunched && Input.GetKeyDown(KeyCode.X))
                            {
                                attack_flag = false;
                                slide_l_or_r = true;
                                slide_attack_flag = true;
                                slide_attack_cnt = 0.0f;
                                spriteRenderer.flipX = true;
                                GlobalAudioManager.Instance.PlaySlideSoundOnce();
                                FlipCollider(1);
                            }
                            else if (!attack_flag)
                            {
                                if (Input.GetKeyDown(KeyCode.LeftArrow))
                                {
                                    walkTimer = 0;
                                }
                                walkTimer += Time.deltaTime;
                                spriteRenderer.flipX = true;
                                rb.velocity = new Vector2(-playermove, rb.velocity.y);
                            }
                        }

                        if (Input.GetKey(KeyCode.RightArrow))
                        {
                            if (hasLaunched && Input.GetKeyDown(KeyCode.X))
                            {
                                attack_flag = false;
                                slide_l_or_r = false;
                                slide_attack_flag = true;
                                slide_attack_cnt = 0.0f;
                                spriteRenderer.flipX = false;
                                GlobalAudioManager.Instance.PlaySlideSoundOnce();
                                FlipCollider(0);
                            }
                            else if (!attack_flag)
                            {
                                if (Input.GetKeyDown(KeyCode.RightArrow))
                                {
                                    walkTimer = 0;
                                }
                                walkTimer += Time.deltaTime;
                                spriteRenderer.flipX = false;
                                rb.velocity = new Vector2(playermove, rb.velocity.y);
                            }
                        }

                        if (Input.GetKey(KeyCode.X) && !attack_flag && !slide_attack_flag)
                        {
                            rb.velocity = Vector2.zero;
                            attack_flag = true;
                            attack_cnt = 0.0f;
                            GameObject ball = GameObject.FindWithTag("Ball");
                            GlobalAudioManager.Instance.PlayAttackSound();
                            if (ball != null)
                            {
                                float playerX = transform.position.x;
                                float ballX = ball.transform.position.x;
                                spriteRenderer.flipX = playerX > ballX;
                            }
                        }
                    }
                }

                if (slide_attack_flag)
                {
                    if (slide_l_or_r)
                    {
                        rb.velocity = new Vector2(-playerslide, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(playerslide, rb.velocity.y);
                    }
                }

                if (hasLaunched && Input.GetKeyDown(KeyCode.Space) && !boom_flag && score >= 100)
                {
                    boom_flag = true;
                    boom_attack_cnt = 0.0f;
                    GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
                    GlobalAudioManager.Instance.PlayExplosionSound();
                    foreach (GameObject ball in balls)
                    {
                        if (BOOM != null)
                        {
                            GameObject tempBall = Instantiate(BOOM, ball.transform.position, Quaternion.identity);
                            Destroy(tempBall, boom_attack_time);
                        }
                    }
                    score = 0;
                    GetScore();
                }

                if (bullet_flag)
                {
                    int frame = Mathf.FloorToInt((bullet_attack_cnt / bullet_attack_time) * bulletSprites.Length);
                    if (frame >= bulletSprites.Length) frame = bulletSprites.Length - 1;
                    GlobalAudioManager.Instance.StopRunSound();                  
                    SetSprite(bulletSprites[frame], bulletSize, bulletOffset);
                }
                else if (slide_attack_flag)
                {
                    int frame = Mathf.FloorToInt((slide_attack_cnt / slide_attack_time) * slideSprites.Length);
                    if (frame >= slideSprites.Length) frame = slideSprites.Length - 1;
                    GlobalAudioManager.Instance.StopRunSound();
                    SetSprite(slideSprites[frame], slideSize, slideOffset);
                }
                else if (attack_flag)
                {
                    int frame = Mathf.FloorToInt((attack_cnt / attack_time) * attackSprites.Length);
                    if (frame >= attackSprites.Length) frame = attackSprites.Length - 1;
                    GlobalAudioManager.Instance.StopRunSound();
                    SetSprite(attackSprites[frame], attackSize, attackOffset);
                }
                else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    int cycle = ((int)(walkTimer / walkswitch)) % 4;
                    int index = (cycle == 3) ? 1 : cycle;
                    GlobalAudioManager.Instance.StartRunSound();
                    SetSprite(walkSprites[index], walkSize, walkOffset);
                }
                else
                {
                    walkTimer = 0;
                    rb.velocity = Vector2.zero;
                    GlobalAudioManager.Instance.StopRunSound();
                    SetSprite(idleSprite, idleSize, idleOffset);
                }
            }

            if (slide_attack_flag == true)
            {
                CircleCollider.enabled = false;
                BoxCollider.enabled = false;
                PolygonCollider.enabled = true;
            }
            else if (attack_flag == true)
            {
                CircleCollider.enabled = true;
                BoxCollider.enabled = false;
                PolygonCollider.enabled = false;
            }
            else
            {
                CircleCollider.enabled = false;
                BoxCollider.enabled = true;
                PolygonCollider.enabled = false;
            }

            switch (level)
            {
                case 2:
                    if (transform.position.y != 5.9f)
                    {
                        targetPosition = new Vector3(transform.position.x, 5.9f, transform.position.z);
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2.0f * Time.deltaTime);
                    }
                    else if (transform.position.y == 5.9f && CheakPosition)
                    {
                        CheakPosition = false;
                        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                        GlobalAudioManager.Instance.StopRunSound();
                        player.controlEnabled = true;//啟用行動
                    }
                    break;
                case 3:
                    if (transform.position.y != 15.9f)
                    {
                        targetPosition = new Vector3(transform.position.x, 15.9f, transform.position.z);
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2.0f * Time.deltaTime);
                    }
                    else if (transform.position.y == 15.9f && CheakPosition)
                    {
                        CheakPosition = false;
                        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                        GlobalAudioManager.Instance.StopRunSound();
                        player.controlEnabled = true;//啟用行動
                    }
                    break;
                case 4:
                    if (transform.position.y != 25.95f)
                    {
                        targetPosition = new Vector3(transform.position.x, 25.95f, transform.position.z);
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2.0f * Time.deltaTime);
                    }
                    else if (transform.position.y == 25.95f && CheakPosition)
                    {
                        CheakPosition = false;
                        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                        GlobalAudioManager.Instance.StopRunSound();
                    }
                    break;
            }
        }
    }

    public void LevelPush()
    {
        level += 1;
        CheakPosition = true;
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.controlEnabled = false;//禁用行動
        StartCoroutine(player.AutoMoveToZero());//移動到x=0
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool hasLaunched = ballScript != null && ballScript.hasLaunched;
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (collision.otherCollider is PolygonCollider2D)//三角碰撞邏輯
            {
                ContactPoint2D contact = collision.contacts[0];
                float contactAngle = Mathf.Atan2(contact.normal.y, contact.normal.x) * Mathf.Rad2Deg;
                float targetAngle = isFlipped ? hypotenuseAngleLeft : hypotenuseAngleRight;
                Debug.Log(contactAngle);
                float diffAngle = Mathf.Abs(Mathf.DeltaAngle(contactAngle, targetAngle));
                if (diffAngle > angleTolerance)
                {
                    return;
                }
            }
            Rigidbody2D ballRb = collision.rigidbody;

            if (attack_flag || slide_attack_flag)
            {
                Vector2 collisionPoint = collision.contacts[0].point;
                Vector2 playerPosition = transform.position;
                Vector2 collisionDirection = (collisionPoint - playerPosition).normalized;

                float angle = Mathf.Atan2(collisionDirection.y, collisionDirection.x) * Mathf.Rad2Deg;
                float horizontalFactor = Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad));
                float verticalFactor = Mathf.Abs(Mathf.Cos(angle * Mathf.Deg2Rad));

                float horizontalStrength = Mathf.Lerp(1.0f, 3.0f, horizontalFactor * angleRangeModifier);
                float verticalStrength = Mathf.Lerp(3.0f, 1.0f, verticalFactor * angleRangeModifier);

                Vector2 forceDirection = new Vector2(collisionDirection.x * horizontalStrength, verticalStrength).normalized;

                float ballSpeed = ballRb.velocity.magnitude;
                float speedMultiplier = Mathf.Clamp(ballSpeed / 5.0f, 0.7f, 1.6f);
                if (ballSpeed < 2.0f)
                {
                    speedMultiplier = 1.2f;
                }

                float normalizedRotationFactor = Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad));
                float rotationSpeed = Mathf.Lerp(10f, 80f, normalizedRotationFactor);
                float angularForce = rotationSpeed * angularForceMultiplier * -Mathf.Sign(collisionDirection.x);

                ballRb.velocity = Vector2.zero;
                ballRb.AddForce(forceDirection * strikeForceMultiplier * speedMultiplier, ForceMode2D.Impulse);
                ballRb.angularVelocity = angularForce * 2.0f;
            }

            if (collision.otherCollider is BoxCollider2D)
            {
                if (hasLaunched && !attack_flag && !slide_attack_flag)
                {
                    LoseLife();
                    if (ballRb.velocity.magnitude > 11.0f)
                    {
                        ballRb.velocity *= 0.6f;
                        ballRb.angularVelocity *= 0.6f;
                    }
                }
            }
        }
    }

    void SetSprite(Sprite newSprite, Vector2 size, Vector2 offset)
    {
        spriteRenderer.sprite = newSprite;
        spriteRenderer.transform.localScale = size;
        spriteRenderer.transform.localPosition = (Vector3)offset;
    }

    public void LoseLife()
    {
        if (invincible)
            return;

        if (life > 0)
        {
            life -= 5;
            HP.GetComponent<HP>().HP_Change(life);
            Debug.Log(life);
            GlobalAudioManager.Instance.PlayDamageSound();

            if (life < 50 && life > 25){
                if (life == 45){
                    GlobalAudioManager.Instance.PlayExtraHitSound1();
                }
                else if (Random.value < (2f / 3f)){
                    GlobalAudioManager.Instance.PlayExtraHitSound1();
                }
            }
            else if (life <= 25){
                if (life == 25)
                {
                    GlobalAudioManager.Instance.PlayExtraHitSound2();
                }
                else if ((Time.time - lastExtraHit2Time < 2.5f)){
                    if (Random.value < 0.75f){
                        GlobalAudioManager.Instance.PlayExtraHitSound2();
                    }
                }
                else{
                    if (Random.value < 0.75f){
                        GlobalAudioManager.Instance.PlayExtraHitSound1();
                        lastExtraHit2Time = Time.time;
                    }
                }
            }
                StartCoroutine(InvincibilityCoroutine());
        }
        
        if (life <= 0)
        {
            HP.GetComponent<HP>().HP_Change(life);
            die = true;
            GlobalAudioManager.Instance.StopRunSound();
            GlobalAudioManager.Instance.PlayDeadSound();//音效
            Leveloader leveloader = GameObject.Find("System").GetComponent<Leveloader>();
            leveloader.Death();
            Debug.Log("die");
            //Time.timeScale = 0;
        }
    }

    IEnumerator InvincibilityCoroutine()//無敵時間
    {
        invincible = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(invincibilitytime);
        spriteRenderer.color = originalColor;
        invincible = false;
    }

    public void GetScore()
    {
        MP.GetComponent<MP>().MP_Change(score);
    }

    void FlipCollider(int x)
    {
        if (isFlipped == false)
        {
            switch (x)
            {
                case 0:
                    break;
                case 1:
                    Vector2[] points = PolygonCollider.points;
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = new Vector2(-points[i].x, points[i].y); // X 軸翻轉
                    }
                    PolygonCollider.points = points; // 套用新的點資料
                    isFlipped = true;
                    break;
            }
        }
        else if (isFlipped == true)
        {
            switch (x)
            {
                case 0:
                    Vector2[] points = PolygonCollider.points;
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = new Vector2(-points[i].x, points[i].y); // X 軸翻轉
                    }
                    PolygonCollider.points = points; // 套用新的點資料
                    isFlipped = false;
                    break;
                case 1:
                    break;
            }
        }
    }

    public IEnumerator AutoMoveToZero()//移動到x=0
    {
        GameObject.Find("Ball").GetComponent<BallBehavior>().LevelChanging();
        GlobalAudioManager.Instance.StartRunSound();
        while (Mathf.Abs(transform.position.x) > 0.1f)
        {
            float direction = transform.position.x > 0 ? -1f : 1f;
            if (direction == -1)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
            rb.velocity = new Vector2(direction * playermove, rb.velocity.y);
            int cycle = ((int)(walkTimer / walkswitch)) % 4;
            int index = (cycle == 3) ? 1 : cycle;
            SetSprite(walkSprites[index], walkSize, walkOffset);
            yield return null;
        }

        rb.velocity = Vector2.zero;
        float transitionTimer = 0f;
        while (!controlEnabled)
        {
            if (Mathf.Abs(transform.position.y - 25.95f) > 0.1f)
            {
                transitionTimer += Time.deltaTime;
                int runIndex = (int)(transitionTimer / transitionSwitch) % 3;
                SetSprite(transitionRunSprites[runIndex], transitionRunSize, transitionRunOffset);
                GlobalAudioManager.Instance.StartRunSound();
            }
            else
            {
                rb.velocity = Vector2.zero;
                GlobalAudioManager.Instance.StopRunSound();
                SetSprite(idleSprite, idleSize, idleOffset);
            }
            yield return null;
        }

        SetSprite(idleSprite, idleSize, idleOffset);
        yield break;
    }

    private float GetYPositionForCase(int caseNum)
    {
        switch (caseNum)
        {
            case 1: return -4.146f;
            case 2: return 5.9f;
            case 3: return 15.9f;
            case 4: return 25.95f;
            default: return -4.146f;
        }
    }

    public int GetCurrentCase()
    {
        float y = transform.position.y;
        //0.5=誤差
        if (Mathf.Abs(y - (-4.146f)) < 0.5f)
            return 1;
        else if (Mathf.Abs(y - 5.9f) < 0.5f)
            return 2;
        else if (Mathf.Abs(y - 15.9f) < 0.5f)
            return 3;
        else if (Mathf.Abs(y - 25.95f) < 0.5f)
            return 4;
        return 1;//預設case=1
    }

    void OnApplicationQuit()
    {
        SaveCurrentProgress();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveCurrentProgress();
    }

    void SaveCurrentProgress()
    {
        GameSaveData data = new GameSaveData();
        data.sceneName = SceneManager.GetActiveScene().name;
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        WallMover wall = GameObject.Find("TWall").GetComponent<WallMover>();
        CamaraMover cameraMover = GameObject.FindObjectOfType<CamaraMover>();
        data.masterCase = wall.GetCurrentCase();
        data.playerHP = player.life;
        data.playerMP = player.score;
        SaveManager.Instance.SaveGame(data);
    }
}
