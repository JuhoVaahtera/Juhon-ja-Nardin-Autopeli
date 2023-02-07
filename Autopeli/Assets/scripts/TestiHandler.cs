using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestiHandler : MonoBehaviour
{
    public int playerNumber = 1;

    //Components
    AutoTesti AutoTesti;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        AutoTesti = GetComponent<AutoTesti>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame and is frame dependent
    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        switch (playerNumber)
        {
            case 1:
                //Get input from Unity's input system.
                inputVector.x = Input.GetAxis("Horizontal_P1");
                inputVector.y = Input.GetAxis("Vertical_P1");
                break;

            case 2:
                //Get input from Unity's input system.
                inputVector.x = Input.GetAxis("Horizontal_P2");
                inputVector.y = Input.GetAxis("Vertical_P2");
                break;
        }

        //Send the input to the car controller.
        AutoTesti.SetInputVector(inputVector);
    }
}

