using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public int counter;
    public GameObject[] powerup;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPowerUps", 0, 1f);
    }

    // Update is called once per frame
    public void SpawnPowerups()
    {
        if (--counter == 0) CancelInvoke("SpawnPowerUps");
        Instantiate(powerup[Random.Range(0, powerup.Length)], new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0,5)), Quaternion.identity);
    }
}
