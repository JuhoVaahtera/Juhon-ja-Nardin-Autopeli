using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    [SerializeField]
    private FloatSO ScoreSO;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ScoreSO.Value += 15;
            Destroy(gameObject);
            powerupEffect.Apply(other.gameObject);
        }

    }
}
