using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Powerups/ShootingbuffSpeed")]
public class ShootingSpeedBuff : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<BulletScript>().speed += amount;
    }
}
