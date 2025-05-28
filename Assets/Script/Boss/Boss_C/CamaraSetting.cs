using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSetting : MonoBehaviour
{
    public Camera Main_Camera;
    public Camera Laser_Camera;

    void Start()
    {
        Main_Camera.depth = 0;
        Laser_Camera.depth = 1;
        Main_Camera.clearFlags = CameraClearFlags.Skybox;
        Laser_Camera.clearFlags = CameraClearFlags.Nothing;
        Debug.Log(Main_Camera.clearFlags);
        Debug.Log(Laser_Camera.clearFlags);
    }
}