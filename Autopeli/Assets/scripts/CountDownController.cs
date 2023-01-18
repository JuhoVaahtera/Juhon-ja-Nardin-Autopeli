using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownController : MonoBehaviour
{
    public int countdownTime;
    public Text countdownDisplay;

    public float timeLeft = 3.0f;
    public Text startText; // used for showing countdown from 3, 2, 1 

     void Update()
    {
        timeLeft -= Time.deltaTime;
        startText.text = (timeLeft).ToString("0");
        if (timeLeft < 0)
        {
            //Do something useful or Load a new game scene depending on your use-case
        }
    }
    
    private void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {

        while(countdownTime > 0)
        {
           
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSecondsRealtime(1);

            countdownTime--;


        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSecondsRealtime(1);

        countdownDisplay.gameObject.SetActive(false);

    }
}
