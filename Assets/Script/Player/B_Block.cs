using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Block : MonoBehaviour
{
    public float fallSpeed = 2.0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 1; 
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void Update()
    {
        rb.velocity = new Vector2(0, -fallSpeed); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) 
            {
                player.score += 1;
                Debug.Log("score" + player.score);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
