using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    [SerializeField]
    private FloatSO ScoreSO;
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
            ScoreSO.Value += 100;
            Destroy(gameObject);
        }
    }
}
