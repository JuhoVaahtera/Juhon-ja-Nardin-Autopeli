using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BulletScript : MonoBehaviour
{
    
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    public float speed;
    public GameObject hitEffect;
    AudioSource bulletHit;
    [SerializeField]
    private FloatSO ScoreSO;

    // Start is called before the first frame update
    void Start()
    {
        bulletHit = GetComponent<AudioSource>();


        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force * speed;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("rakennukset"))
        {
            bulletHit.Play();
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            
        }

        if (other.gameObject.CompareTag("AI"))
        {
            bulletHit.Play();
            Instantiate(hitEffect, transform.position, transform.rotation);
            ScoreSO.Value += 20;
            Destroy(gameObject);
            
        }

        if (other.gameObject.CompareTag("AI2"))
        {
            bulletHit.Play();
            Instantiate(hitEffect, transform.position, transform.rotation);
            ScoreSO.Value += 20;
            Destroy(gameObject);
            
        }

        if (other.gameObject.CompareTag("AI3"))
        {
            bulletHit.Play();
            Instantiate(hitEffect, transform.position, transform.rotation);
            ScoreSO.Value += 20;
            Destroy(gameObject);
            
        }

        if (other.gameObject.CompareTag("AI4"))
        {
            bulletHit.Play();
            Instantiate(hitEffect, transform.position, transform.rotation);
            ScoreSO.Value += 20;
            Destroy(gameObject);
            
        }

        if (other.gameObject.CompareTag("Player"))
        {
            bulletHit.Play();
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }


}
