using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreScript : MonoBehaviour
{
    [SerializeField]
    private FloatSO ScoreSO;
    public static int scoreValue = 0;
    Text score;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text> ();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 5f)
        {
            ScoreSO.Value += 5;

            timer = 0;
        }


        score.text = "Score: " + ScoreSO.Value;
    }
}
