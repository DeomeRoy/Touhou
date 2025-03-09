using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMover : MonoBehaviour
{
    public Transform wall1_2, wall1_3, wall1_X;
    public float speed = 2.0f;
    Vector3 targetPosition;
    int Level;

    void Start()
    {
        Level = 1;
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
            StartCoroutine(TriggerBossMusic(totalTransitionTime));
        }
    }

    IEnumerator TriggerBossMusic(float totalTransitionTime)
    {
        float halfTime = totalTransitionTime / 2f;
        GlobalAudioManager.Instance.FadeOutMusic(halfTime);
        yield return new WaitForSeconds(halfTime);
        GlobalAudioManager.Instance.PlayBossMusicWithFadeIn(halfTime);
    }
}
