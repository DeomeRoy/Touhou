using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;
using System.Data;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string connectionString;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            connectionString = "URI=file:" + Path.Combine(Application.persistentDataPath, "SaveGame.db");
            CreateTable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CreateTable()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "CREATE TABLE IF NOT EXISTS SaveGame (id INTEGER PRIMARY KEY, sceneName TEXT, masterCase INTEGER, playerHP INTEGER, playerMP INTEGER)";
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteNonQuery();
            }

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = "SELECT COUNT(*) FROM SaveGame WHERE id = 1";
                int count = int.Parse(dbCmd.ExecuteScalar().ToString());
                if (count == 0)
                {
                    using (IDbCommand insertCmd = dbConnection.CreateCommand())
                    {
                        string insertQuery = "INSERT INTO SaveGame (id, sceneName, masterCase, playerHP, playerMP) VALUES (1, 'Stage1', 1, 100, 50)";
                        insertCmd.CommandText = insertQuery;
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            dbConnection.Close();
        }
    }


    public void SaveGame(GameSaveData data)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "REPLACE INTO SaveGame (id, sceneName, masterCase, playerHP, playerMP) VALUES (1, @sceneName, @masterCase, @playerHP, @playerMP)";
                dbCmd.CommandText = sqlQuery;

                var paramScene = dbCmd.CreateParameter();
                paramScene.ParameterName = "@sceneName";
                paramScene.Value = data.sceneName;
                dbCmd.Parameters.Add(paramScene);

                var parammasterCase = dbCmd.CreateParameter();
                parammasterCase.ParameterName = "@masterCase";
                parammasterCase.Value = data.masterCase;
                dbCmd.Parameters.Add(parammasterCase);

                var paramHP = dbCmd.CreateParameter();
                paramHP.ParameterName = "@playerHP";
                paramHP.Value = data.playerHP;
                dbCmd.Parameters.Add(paramHP);

                var paramMP = dbCmd.CreateParameter();
                paramMP.ParameterName = "@playerMP";
                paramMP.Value = data.playerMP;
                dbCmd.Parameters.Add(paramMP);

                dbCmd.ExecuteNonQuery();
            }
            dbConnection.Close();
        }
        Debug.Log("scenes=" + data.sceneName + "case=" + data.masterCase +"hp="+data.playerHP+"mp="+data.playerMP);
    }

    public GameSaveData LoadGame()
    {
        GameSaveData data = null;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM SaveGame WHERE id = 1";
                dbCmd.CommandText = sqlQuery;
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        data = new GameSaveData();
                        data.sceneName = reader["sceneName"].ToString();
                        data.masterCase = int.Parse(reader["masterCase"].ToString());
                        //data.masterCase = 3;
                        data.playerHP = int.Parse(reader["playerHP"].ToString());
                        //data.playerHP = 100;
                        data.playerMP = int.Parse(reader["playerMP"].ToString());
                        //data.playerMP = 100;
                    }
                    reader.Close();
                }
            }
            dbConnection.Close();
        }
        if (data != null)
            Debug.Log("scenes=" + data.sceneName + "case=" + data.masterCase + "hp=" + data.playerHP + "mp=" + data.playerMP);
        return data;
    }
}
