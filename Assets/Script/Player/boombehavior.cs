using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boombehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        A_Block aBlock = collision.gameObject.GetComponent<A_Block>();
        if (aBlock != null)
        {
            aBlock.TriggerBoom(); 
        }

        C_Block cBlock = collision.gameObject.GetComponent<C_Block>();
        if (cBlock != null)
        {
            cBlock.TriggerBoom();
        }
    }
}
