using UnityEngine;
using System.Collections;
 
public class Ampuminen : MonoBehaviour {
 
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;
    AudioSource shootingSound;
    
    // Use this for initialization
    void Start () 
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        shootingSound = GetComponent<AudioSource>();
    }
   
    // Update is called once per frame
    void Update () 
    {
       mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

       Vector3 rotation = mousePos - transform.position;

       float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

       transform.rotation = Quaternion.Euler(0, 0, rotZ);

       if(!canFire)
       {
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0; 
            }
       }

       if(Input.GetMouseButton(0) && canFire)
       {
            shootingSound.Play();

            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
       }
    }
}
