using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HP_Bar_Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);
    public Camera cam;
    public RectTransform HPbar;
    public float BossHP, lessHP;
    void Start()
    {
        BossHP = 50;
        lessHP = 1;
    }

    void LateUpdate()
    {
        if (target != null && cam != null)
        {
            Vector3 worldPos = target.position + offset;
            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
            transform.position = screenPos;
        }
        HPbar.localScale = new Vector3(lessHP, 1f, 1f);
        if (lessHP == 0)
        {
            Destroy(gameObject);
        }
    }
    public void UpdateBossHP_A(float x)
    {
        lessHP = x / 50;
    }
    public void UpdateBossHP_BC(float x){
        lessHP = x/ 75;
    }
}
