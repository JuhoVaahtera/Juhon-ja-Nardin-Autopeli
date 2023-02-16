using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject()
    {
        Instantiate(Enemy, transform.position, transform.rotation);
        if(stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
