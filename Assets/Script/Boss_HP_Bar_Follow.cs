using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HP_Bar_Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);
    public Camera cam;
    public RectTransform HPbar;
    public int BossHP;
    void Start(){
        BossHP = 10;
    }

    void LateUpdate()
    {
        if (target != null && cam != null)
        {
            Vector3 worldPos = target.position + offset;
            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
            transform.position = screenPos;
        }
    }
    public void UpdateBossHP(int x){
        float lessHP = x/BossHP;
        HPbar.localScale = new Vector3(lessHP,1f,1f);
    }
}
