using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    [SerializeField]
    private FloatSO ScoreSO;
    public GameObject deathEffect;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
            ScoreSO.Value += 50;
            Destroy(gameObject);
        }
    }
}
