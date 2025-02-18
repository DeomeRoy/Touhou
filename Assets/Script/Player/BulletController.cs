using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController player;
    public float speed = 0.75f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        Vector2 newPosition = rb.position + new Vector2(0, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("A_Block"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("B_Block"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("C_Block"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("D_Ball"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Fake_Wall"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Player"), true);
    }

    void Update()
    {
        Vector2 newPosition = rb.position + new Vector2(0, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            Debug.Log(collision.gameObject.name); 

            if (collision.gameObject.CompareTag("Ball"))
            {
                Vector2 newPosition = rb.position + new Vector2(0, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
                //rb.velocity = new Vector2(0, speed);

                Rigidbody2D ballRb = collision.rigidbody;

                ContactPoint2D contact = collision.contacts[0];
                Vector2 collisionPoint = contact.point;
                Vector2 bulletPosition = transform.position;
                Vector2 collisionDirection = (collisionPoint - bulletPosition).normalized;

                float angle = Mathf.Atan2(collisionDirection.y, collisionDirection.x) * Mathf.Rad2Deg;
                float horizontalFactor = Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad)); 
                float verticalFactor = Mathf.Abs(Mathf.Cos(angle * Mathf.Deg2Rad));
                float horizontalStrength = Mathf.Lerp(1.0f, 3.0f, horizontalFactor); 
                float verticalStrength = Mathf.Lerp(3.0f, 1.0f, verticalFactor); 
            
                Vector2 forceDirection = new Vector2(collisionDirection.x * horizontalStrength, verticalStrength).normalized;

                float ballSpeed = ballRb.velocity.magnitude;
                float minSpeed = 3.0f;

                if (ballSpeed < minSpeed)
                {
                    ballSpeed = minSpeed; 
                }

                ballRb.velocity = forceDirection * ballSpeed * 1.5f;
                float angularForce = ballSpeed * -Mathf.Sign(collisionDirection.x) * 100.0f;
                ballRb.angularVelocity = angularForce;
            }

            if (collision.gameObject.CompareTag("Wall"))
            {
                player.bullet_exists = false;
                Destroy(gameObject);
            }
    }
}
