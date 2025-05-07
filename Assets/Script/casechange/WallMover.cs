using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallMover : MonoBehaviour
{
    public Transform wall1_2, wall1_3, wall1_X;
    public float speed = 2.0f;
    private Vector3 targetPosition;
    public int Level;

    void Start()
    {
        Level = 1;
        //讀檔
        if (GameManager.isContinue)
            GameSaveSystem.ApplySavedGameToScene();
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
        FindObjectOfType<LevelImageFader>().FadeForLevel(Level);
        //if (Level == 4)//僅限case4的存檔功能，如有需要可以移除判斷讓其他case也能存檔
        /*{ }*/
            GameSaveSystem.SaveCurrentProgress();
        
        if (Level == 4)
        {
            float totalTransitionTime = Vector3.Distance(transform.position, wall1_X.position) / speed;
            StartCoroutine(TriggerStoryByDistance(totalTransitionTime));//先淡出，再進劇情or Boss
        }
    }

    public void ApplySaveData(int savedCase)
    {
        FindObjectOfType<LevelImageFader>().FadeForLevel(savedCase);
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
        }
    }


    private IEnumerator TriggerStoryByDistance(float totalTransitionTime)//進入劇情的引用
    {
        float fadeOutTime = totalTransitionTime * 0.7f;//關卡淡出
        float fadeInTime = totalTransitionTime * 0.3f;//劇情淡入

        SceneAudioManager.Instance.FadeOutSceneMusic(fadeOutTime);
        yield return new WaitForSeconds(fadeOutTime + 0.167f);
        SceneAudioManager.Instance.PlayStoryMusicWithFadeIn(fadeInTime);

        string sceneName = SceneManager.GetActiveScene().name;
        string storyID = "";

        switch (sceneName)
        {
            case "Stage0":
                storyID = "BossPreFight";
                break;
            case "Stage1":
                storyID = "BossPreFight";
                break;
            case "Stage2":
                storyID = "BossPreFight";
                break;
            case "Stage3":
                storyID = "BossPreFight";
                break;
        }

        StoryController storyCtrl = FindObjectOfType<StoryController>();
        storyCtrl.StartStory(storyID);
    }

    public int GetCurrentCase()
    {
        return Level;
    }
}
