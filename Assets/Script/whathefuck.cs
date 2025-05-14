using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whathefuck : MonoBehaviour
{
    public float rotationSpeed = 500f;
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
