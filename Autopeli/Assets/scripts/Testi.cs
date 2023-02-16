using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Testi : MonoBehaviour
{
    NavMeshAgent agentti;
    Transform pelaaja;
    Vector3 määränpää;

    public Vector3 _centerOfMass;

    private Rigidbody carRb;

    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;

        pelaaja = GameObject.Find("PlayerCar").transform;
        agentti = GetComponent<NavMeshAgent>();
        määränpää = agentti.destination;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(pelaaja.position, määränpää) > 1.0f)
        {
            määränpää = pelaaja.position;
            agentti.destination = määränpää;
        }
    }
}
