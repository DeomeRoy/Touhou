using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTest : MonoBehaviour{
    void Start(){
        StartCoroutine(LevelDoneCheck());
    }
    IEnumerator LevelDoneCheck(){
        while (true){
            yield return new WaitForSeconds(125f*Time.deltaTime);
            if (transform.childCount == 0 && GameObject.FindWithTag("B_Block") == null && GameObject.FindWithTag("D_Ball") == null && GameObject.FindWithTag("E_Block") == null) {
                GameObject.Find("System").GetComponent<Leveloader>().LevelPush();
                Destroy(gameObject);
                yield break;
            }
        }
    }
}
