using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            powerupEffect.Apply(other.gameObject);
        }

        if (other.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
            powerupEffect.Apply(other.gameObject);
        }

    }
}
