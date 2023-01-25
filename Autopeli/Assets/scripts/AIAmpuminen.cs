using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAmpuminen : MonoBehaviour
{

    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;
    AudioSource bulletHit;
    // Start is called before the first frame update
    void Start()
    {
        bulletHit = GetComponent<AudioSource>();
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bulletHit.Play();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("AI"))
        {
            bulletHit.Play();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("AI2"))
        {
            bulletHit.Play();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("AI3"))
        {
            bulletHit.Play();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("AI4"))
        {
            bulletHit.Play();
            Destroy(gameObject);
        }
    }

    }
