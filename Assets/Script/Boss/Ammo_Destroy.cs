using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Destroy : MonoBehaviour{
    [HideInInspector]public float Clock = 0;
    [HideInInspector]public float Gap = 0;
    public GameObject BossHpBar;
    void Start(){
        BossHpBar = GameObject.Find("BossHPbar");
    }
    void Update(){
        Clock += Time.deltaTime;
        if(Clock - Gap > 0.05f){
            Gap += 0.05f;
            if(transform.position.x > 10f || transform.position.x < -10f || transform.position.y > 36f || transform.position.y < 21f){
                Destroy(gameObject);
            }
        }
        if(BossHpBar == null){
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.LoseLife();
            Destroy(gameObject);
        }
        if (collision.CompareTag("Ball"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("boom")){
            Destroy(gameObject);
        }
    }
    void OCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
