using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Destroy : MonoBehaviour{
    [HideInInspector]public float Clock = 0;
    [HideInInspector]public float Gap = 0;
    void Update(){
        Clock += Time.deltaTime;
        if(Clock - Gap > 0.05f){
            Gap += 0.05f;
            if(transform.position.x > 10f || transform.position.x < -10f || transform.position.y > 8f || transform.position.y < -8f){
                Destroy(gameObject);
            }
        }
    }
}
