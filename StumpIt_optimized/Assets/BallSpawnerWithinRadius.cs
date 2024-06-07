using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerWithinRadius : MonoBehaviour
{
    public GameObject ballPrefab;  
    public Transform stump;        
    public float innerRadius = 2f; 
    public float outerRadius = 5f;
    private GameObject instantiatedBall;

    void Start()
    {
    }
    public void Init()
    {
        SpawnBall();

    }
    void SpawnBall()
    {
        
        if (innerRadius >= outerRadius)
        {
            Debug.LogError("Inner radius must be less than outer radius");
            return;
        }

        Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(innerRadius, outerRadius);

        Vector3 spawnPosition = new Vector3(randomPoint.x, 0, randomPoint.y) + stump.position;

       instantiatedBall= Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }
    public void DestroyBall()
    {
        Destroy(instantiatedBall);
    }
}
