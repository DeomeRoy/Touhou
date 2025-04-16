using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BossB_NATK_C1 : MonoBehaviour{
    public float radius = 5f;     // 圓的半徑
    public float speed = 1f;      // 旋轉速度
    private float angle = 90f;     // 角度
    private Vector2 startPosition;
    void Start(){
        startPosition = new Vector3(transform.position.x,transform.position.y-4f);  // 記錄起始位置
    }
    void Update(){
        angle -= speed * Time.deltaTime;  

        if (angle < -90f) angle = -90f;

        // 使用餘弦 (X) 和正弦 (Y) 計算物件的位置
        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

        // 更新物件位置
        transform.position = new Vector2(startPosition.x + x, startPosition.y + y);
        if(angle == -90){
            Destroy(gameObject);
        }
    }
}
