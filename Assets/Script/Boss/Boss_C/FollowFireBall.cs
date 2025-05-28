using System.Threading;
using UnityEngine;

public class FollowFireBall : MonoBehaviour
{
    public float Timer,angle,moveSpeed;
    public bool Followed, Search;
    public GameObject Player;
    Vector3 Direction;
    void Start()
    {
        Followed = false;
        Search = false;
        moveSpeed = 5f;
        angle = 0;
        Player = GameObject.FindGameObjectWithTag("Player");
        
    }
    void Update()
    {
        Timer += Time.deltaTime;
        if (!Search && Timer > 0.5f)
        {
            Search = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Aim();
        }

        if (Search && !Followed)
        {
            transform.position += Direction.normalized * moveSpeed * Time.deltaTime;
        }
    }
    void Aim()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        Direction = Player.transform.position - transform.position;
        angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
    }
}
