using System.Collections;
using UnityEngine;

public class D_BallSpawner : MonoBehaviour
{
    private GameObject ballPrefab;
    private Vector2 spawnPosition;
    private int ballsToSpawn;
    private float delayBetweenBalls;

    public void Initialize(GameObject prefab, Vector2 position, int count, float delay)
    {
        ballPrefab = prefab;
        spawnPosition = position;
        ballsToSpawn = count;
        delayBetweenBalls = delay;
        StartCoroutine(SpawnBalls());
    }

    private IEnumerator SpawnBalls()
    {
        for (int i = 0; i < ballsToSpawn; i++)
        {
            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(delayBetweenBalls);
        }
        Destroy(gameObject);
    }
}
