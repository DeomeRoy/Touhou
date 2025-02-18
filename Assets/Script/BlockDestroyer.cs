using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour{
    void Start(){
        StartCoroutine(CheckChildrenCount());
    }
    IEnumerator CheckChildrenCount(){
        while (true){
            yield return new WaitForSeconds(1f*Time.deltaTime);
            if (transform.childCount == 0){
                Destroy(gameObject);
            }
        }
    }
}
