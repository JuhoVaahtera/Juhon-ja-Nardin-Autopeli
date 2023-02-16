using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public PlayerHealth pHealth;
    public AIHealth aiHealth;
    public AIHealth aiHealth2;
    public AIHealth aiHealth3;
    public AIHealth aiHealth4;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        pHealth = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pHealth.health -= damage;
        }

        if (other.gameObject.CompareTag("AI"))
        {
            aiHealth.health -= damage;
        }

        if (other.gameObject.CompareTag("AI2"))
        {
            aiHealth2.health -= damage;
        }

        if (other.gameObject.CompareTag("AI3"))
        {
            aiHealth3.health -= damage;
        }

        if (other.gameObject.CompareTag("AI4"))
        {
            aiHealth4.health -= damage;
        }
    }
}
