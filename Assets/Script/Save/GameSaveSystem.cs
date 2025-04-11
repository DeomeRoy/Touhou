using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSaveSystem
{
    // 通用存檔引用
    public static void SaveCurrentProgress()
    {
        GameSaveData data = new GameSaveData();
        data.sceneName = SceneManager.GetActiveScene().name;

        PlayerController player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        WallMover wall = GameObject.FindObjectOfType<WallMover>();

        if (player != null)
        {
            data.playerHP = player.life;
            data.playerMP = player.score;
        }

        if (wall != null)
        {
            data.masterCase = wall.GetCurrentCase();
        }

        SaveManager.Instance.SaveGame(data);
    }

    // 載入存檔後直接套用到場景
    public static void ApplySavedGameToScene()
    {
        GameSaveData data = SaveManager.Instance.LoadGame();
        if (data == null) return;

        if (data.sceneName != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
            return;

        PlayerController player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        WallMover wall = GameObject.FindObjectOfType<WallMover>();
        CamaraMover cam = GameObject.FindObjectOfType<CamaraMover>();

        // 套用角色血量、魔力與位置
        if (player != null)
        {
            player.life = data.playerHP;
            player.score = data.playerMP;

            float yPos = player.GetYPositionForCase(data.masterCase);
            player.transform.position = new Vector3(0, yPos, player.transform.position.z);
            player.HP.GetComponent<HP>().HP_Change(data.playerHP);
            player.MP.GetComponent<MP>().MP_Change(data.playerMP);
        }

        // 套用關卡進度
        wall?.ApplySaveData(data.masterCase);
        cam?.ApplySaveData(data.masterCase);
    }

    // 給Story使用的重置玩家狀態
    public static void SaveFixedStoryProgress(string sceneName, int caseNum, int hp, int mp)
    {
        GameSaveData data = new GameSaveData
        {
            sceneName = sceneName,
            masterCase = caseNum,
            playerHP = hp,
            playerMP = mp
        };

        SaveManager.Instance.SaveGame(data);
    }

    // 只回傳資料
    public static GameSaveData LoadSaveData()
    {
        return SaveManager.Instance.LoadGame();
    }
}
