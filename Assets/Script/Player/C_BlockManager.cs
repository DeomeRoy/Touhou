using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_BlockManager : MonoBehaviour
{
    public GameObject D_BallPrefab; 

    public string blockTag = "C_Block";
    public string blockLayer = "C_Block";

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        foreach (Transform child in transform)
        {
            C_Block cBlock = child.GetComponent<C_Block>();
            if (cBlock == null)
            {
                cBlock = child.gameObject.AddComponent<C_Block>();
            }

            if (cBlock.D_BallPrefab == null)
            {
                cBlock.D_BallPrefab = D_BallPrefab;
            }

            child.gameObject.tag = blockTag;
            child.gameObject.layer = LayerMask.NameToLayer(blockLayer);
        }
    }
}
