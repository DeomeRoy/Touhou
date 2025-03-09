using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Block : MonoBehaviour
{
    public GameObject B_BlockPrefab;
    public GameObject E_BlockPrefab;
    public int OnCollisionTimes = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            if (OnCollisionTimes == 1)
            {
                bool dropEBlock = false;
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    PlayerController pc = playerObj.GetComponent<PlayerController>();
                   //玩家血量低於50
                   if (pc.life < 50)
                   {
                      //1/3掉Ecube
                      if (Random.value < (1f / 3f))
                      {
                        dropEBlock = true;
                      }
                   }
                }
                if (dropEBlock)
                {
                    Instantiate(E_BlockPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(B_BlockPrefab, transform.position, Quaternion.identity);
                }

                Rigidbody2D ballRb = collision.rigidbody;

                ContactPoint2D contact = collision.contacts[0];
                Vector2 collisionPoint = contact.point;
                Vector2 blockPosition = transform.position;
                Vector2 collisionDirection = (collisionPoint - blockPosition).normalized;

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
            else
            {
                OnCollisionTimes = 1;
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("A_Block_Broke");
            }
        }
    }

    public void TriggerBoom()
    {
        Instantiate(B_BlockPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
