using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTest : MonoBehaviour{
    void Start(){
        StartCoroutine(LevelDoneCheck());
    }
    IEnumerator LevelDoneCheck(){
        Debug.Log("1");
        while (true){
            Debug.Log("2");
            yield return new WaitForSeconds(500f*Time.deltaTime);
            if (transform.childCount == 0 && GameObject.FindWithTag("B_Block") == null && GameObject.FindWithTag("D_Ball") == null){
                GameObject.Find("System").GetComponent<Leveloader>().LevelPush();
                Destroy(gameObject);
                yield break;
            }
        }
    }
}
