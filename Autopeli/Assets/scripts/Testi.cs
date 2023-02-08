using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Testi : MonoBehaviour
{
    NavMeshAgent agentti;
    Transform pelaaja;
    Vector3 m‰‰r‰np‰‰;


    // Start is called before the first frame update
    void Start()
    {
        pelaaja = GameObject.Find("Cuubi").transform;
        agentti = GetComponent<NavMeshAgent>();
        m‰‰r‰np‰‰ = agentti.destination;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(pelaaja.position, m‰‰r‰np‰‰) > 1.0f)
        {
            m‰‰r‰np‰‰ = pelaaja.position;
            agentti.destination = m‰‰r‰np‰‰;
        }
    }
}
