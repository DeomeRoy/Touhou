using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Block : MonoBehaviour
{
    public GameObject D_BallPrefab;  
    public int numBalls = 3;  
    public float delayBetweenBalls = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            GameObject spawner = new GameObject("D_BallSpawner");
            D_BallSpawner ballSpawnerScript = spawner.AddComponent<D_BallSpawner>();
            ballSpawnerScript.Initialize(D_BallPrefab, transform.position, numBalls, delayBetweenBalls);
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

            ballRb.velocity = forceDirection * ballSpeed * 0.9f;
            float angularForce = ballSpeed * -Mathf.Sign(collisionDirection.x) * 100.0f;
            ballRb.angularVelocity = angularForce;
            Destroy(gameObject);
        }
    }

    public void TriggerBoom()
    {
        GameObject spawner = new GameObject("D_BallSpawner");
        D_BallSpawner ballSpawnerScript = spawner.AddComponent<D_BallSpawner>();
        ballSpawnerScript.Initialize(D_BallPrefab, transform.position, numBalls, delayBetweenBalls);
        Destroy(gameObject);
    }
}
