using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BossB_NATK_D : MonoBehaviour{
    public GameObject Player;
    public float Speed,rotationSpeed;
    public float SkillTime;
    private Vector3 lockedDirection;
    public void Start(){
        Speed = 5f;
        rotationSpeed = 200f;
    }
     public void Update(){
        Player = GameObject.FindGameObjectWithTag("Player");
        SkillTime += Time.deltaTime;
        if(SkillTime > 1.5f && SkillTime < 2.5f){
            Vector2 direction = (Player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle+90);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else{
            lockedDirection = transform.up.normalized;
            transform.position += lockedDirection * -Speed * Time.deltaTime;
        }
    }
}
