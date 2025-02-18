using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_BlockManager : MonoBehaviour
{
    public GameObject B_BlockPrefab; 

    public string blockTag = "A_Block";
    public string blockLayer = "A_Block";

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false; 

        foreach (Transform child in transform)
        {
            A_Block aBlock = child.GetComponent<A_Block>();
            if (aBlock == null)
            {
                aBlock = child.gameObject.AddComponent<A_Block>();
            }

            if (aBlock.B_BlockPrefab == null)
            {
                aBlock.B_BlockPrefab = B_BlockPrefab;
            }
            child.gameObject.tag = blockTag;
            child.gameObject.layer = LayerMask.NameToLayer(blockLayer);
        }
    }
}
