using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraMover : MonoBehaviour{
    public Transform Level1_2,Level1_3,Level1_X;
    public float speed = 2.0f;
    Vector3 targetPosition;
    int Level;
    void Start(){
        Level = 1;
    }
    void Update(){
        switch(Level){
            case 2:
                targetPosition = new Vector3(Level1_2.position.x,Level1_2.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position,targetPosition, speed * Time.deltaTime);
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
}
