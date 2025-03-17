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
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player"){
            // PlayerController player = other.GetComponent<PlayerController>();
            // player.LoseLife();
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
