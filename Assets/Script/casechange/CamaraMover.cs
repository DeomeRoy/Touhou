using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraMover : MonoBehaviour{
    public Transform Level1_2,Level1_3,Level1_X;
    public float speed = 2.0f;
    Vector3 targetPosition;
    int Level;
    bool CheakCamara;
    void Start(){
        Level = 1;

        if (GameManager.isContinue)
            GameSaveSystem.ApplySavedGameToScene();
    }

    void Update(){
        switch(Level){
            case 2:
                targetPosition = new Vector3(Level1_2.position.x,Level1_2.position.y, transform.position.z);
                if(transform.position.y != 10.02f){
                transform.position = Vector3.MoveTowards(transform.position,targetPosition, speed * Time.deltaTime);
                }
                break;
            case 3:
                targetPosition = new Vector3(Level1_3.position.x,Level1_3.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position,targetPosition, speed * Time.deltaTime);
                break;
            case 4:
                targetPosition = new Vector3(Level1_X.position.x,Level1_X.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position,targetPosition, speed * Time.deltaTime);
                break;
        }
    }
    public void LevelPush(){
        Level += 1;
    }

    public void ApplySaveData(int savedCase)
    {
        Level = savedCase;
        switch (savedCase)
        {
            case 2:
                transform.position = new Vector3(Level1_2.position.x, Level1_2.position.y, transform.position.z);
                break;
            case 3:
                transform.position = new Vector3(Level1_3.position.x, Level1_3.position.y, transform.position.z);
                break;
            case 4:
                transform.position = new Vector3(Level1_X.position.x, Level1_X.position.y, transform.position.z);
                break;
            default:
                break;
        }
    }

    public int GetCurrentCase()
    {
        return Level;
    }
}
