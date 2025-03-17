using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallMover : MonoBehaviour
{
    public Transform wall1_2, wall1_3, wall1_X;
    public float speed = 2.0f;
    private Vector3 targetPosition;
    private int Level;

    void Start()
    {
        Level = 1;

        //讀檔
        GameSaveData saveData = SaveManager.Instance.LoadGame();
        if (GameManager.isContinue && saveData != null && saveData.sceneName == SceneManager.GetActiveScene().name)
        {
            int savedCase = saveData.masterCase;
            switch (savedCase)
            {
                case 2:
                    transform.position = new Vector3(wall1_2.position.x, wall1_2.position.y, transform.position.z);
                    Level = 2;
                    break;
                case 3:
                    transform.position = new Vector3(wall1_3.position.x, wall1_3.position.y, transform.position.z);
                    Level = 3;
                    break;
                case 4:
                    transform.position = new Vector3(wall1_X.position.x, wall1_X.position.y, transform.position.z);
                    Level = 4;
                    break;
                default:
                    break;
            }
        }
    }

    void Update()
    {
        switch (Level)
        {
            case 2:
                targetPosition = new Vector3(wall1_2.position.x, wall1_2.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                break;

            case 3:
                targetPosition = new Vector3(wall1_3.position.x, wall1_3.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                break;

            case 4:
                targetPosition = new Vector3(wall1_X.position.x, wall1_X.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                break;
        }
    }

    public void LevelPush()
    {
        Level += 1;
        if (Level == 4)
        {
            float totalTransitionTime = Vector3.Distance(transform.position, wall1_X.position) / speed;

            StartCoroutine(TriggerStoryByDistance(totalTransitionTime));//先淡出，再進劇情or Boss

            //存檔
            GameSaveData data = new GameSaveData();
            data.sceneName = SceneManager.GetActiveScene().name;
            data.masterCase = Level;

            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            data.playerHP = player.life;
            data.playerMP = player.score;

            SaveManager.Instance.SaveGame(data);
        }
    }

    private IEnumerator TriggerStoryByDistance(float totalTransitionTime)//進入劇情的引用
    {
        float fadeOutTime = totalTransitionTime * 0.7f;//關卡淡出
        float fadeInTime = totalTransitionTime * 0.3f;//劇情淡入

        GlobalAudioManager.Instance.FadeOutMusic(fadeOutTime);
        yield return new WaitForSeconds(fadeOutTime);
        GlobalAudioManager.Instance.PlayStoryMusicWithFadeIn(fadeInTime);

        //呼叫劇情
        StoryController storyCtrl = FindObjectOfType<StoryController>();
        storyCtrl.StartStory("BossPreFight");
    }

    /*
    IEnumerator WaitAndTriggerStory(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StoryController storyCtrl = FindObjectOfType<StoryController>();
        if (storyCtrl != null)
            storyCtrl.StartStory();
    }
    */

    public int GetCurrentCase()
    {
        return Level;
    }
}
