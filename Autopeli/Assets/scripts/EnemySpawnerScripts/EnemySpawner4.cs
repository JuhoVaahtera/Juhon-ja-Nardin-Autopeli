using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner4 : MonoBehaviour
{

    [SerializeField]
    private GameObject EnemyPrefab;

    [SerializeField]
    private float EnemyInterval = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(EnemyInterval, EnemyPrefab));
    }

    // Update is called once per frame
    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-19f, -19f), 21), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
