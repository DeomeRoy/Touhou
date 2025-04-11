using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP : MonoBehaviour
{
    public void MP_Change(int value)
    {
        value = Mathf.Clamp(value, 0, 100);
        GetComponent<RectTransform>().localScale = new Vector3(value / 100f, 1f, 1f);
    }

    public void MP_Reset()
    {
        GetComponent<RectTransform>().localScale = new Vector3(0f, 1f, 1f);
    }
}
