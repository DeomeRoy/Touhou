using UnityEngine;

public class HackSaveEditor : MonoBehaviour
{
    private int? stageNum = null;
    private int? caseNum = null;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject); // 保留物件跨場景存在
    }

    void Update()
    {
        // 只在同時按住 LeftShift + LeftCtrl 時才進入輸入模式
        if (!(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl)))
        {
            stageNum = null;
            caseNum = null;
            return;
        }

        if (Input.GetKey(KeyCode.RightShift))
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
            player.life = 10000;
            player.score = 10000;
            player.HP.GetComponent<HP>().HP_Change(player.life);
            player.GetScore();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameSaveData data = SaveManager.Instance.LoadGame();
            if (data != null)
            {
                data.hasVisitedStage0 = !data.hasVisitedStage0;
                SaveManager.Instance.SaveGame(data);
                Debug.Log($"[Hack] visited0 狀態切換為 {(data.hasVisitedStage0 ? "✅ true" : "❌ false")}");
            }
            else
            {
                Debug.LogWarning("[Hack] 無存檔，無法切換 visited0");
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Boss_A boss = FindObjectOfType<Boss_A>();
            if (boss != null)
            {
                boss.BossHP = 0;
                Debug.Log("[Hack] Boss 血量已設為 0");
            }
            else
            {
                Debug.LogWarning("[Hack] 找不到 Boss_A");
            }
        }

        // 專門偵測小鍵盤數字（Keypad0 ~ Keypad9）
        for (KeyCode key = KeyCode.Keypad0; key <= KeyCode.Keypad9; key++)
        {
            if (Input.GetKeyDown(key))
            {
                int num = key - KeyCode.Keypad0;
                Debug.Log($"[Hack] 小鍵盤輸入：{num}");

                if (stageNum == null)
                {
                    stageNum = num;
                    Debug.Log("[Hack] 已輸入 Stage：" + stageNum);
                }
                else if (caseNum == null)
                {
                    caseNum = num;
                    Debug.Log("[Hack] 已輸入 Case：" + caseNum);

                    if (stageNum >= 0 && stageNum <= 3 && caseNum >= 1 && caseNum <= 4)
                    {
                        string sceneName = "Stage" + stageNum;
                        GameSaveSystem.SaveFixedStoryProgress(sceneName, caseNum.Value, 100, 50);
                        Debug.Log($"✅ [Hack] 存檔強制設為 {sceneName}, Case {caseNum}");
                    }
                    else
                    {
                        Debug.LogWarning($"[Hack] ❌ 輸入錯誤：Stage 限 0~3，Case 限 1~4（{stageNum}, {caseNum}）");
                    }

                    // 重設輸入
                    stageNum = null;
                    caseNum = null;
                }
            }
        }
    }
}
