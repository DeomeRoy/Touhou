using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Ball : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Dynamic; 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 targetPosition = player.transform.position; 
        moveDirection = (targetPosition - (Vector2)transform.position).normalized; 
    }

    void Update()
    {
        rb.velocity = moveDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.LoseLife(); 
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ball"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("boom"))
        {
            Destroy(gameObject);
        }
    }
}
