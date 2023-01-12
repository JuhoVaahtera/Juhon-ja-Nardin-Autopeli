using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GAMEOVERSCREEN : MonoBehaviour
{
    // Start is called before the first frame update
    public void RestartButton()
    {
        SceneManager.LoadScene("Menu");
    }

    //void Die(){

    //if (health.CurrentValue  <=0) 
    //{
        //Destroy(GameObject.FindGameObjectWithTag ("Auto"));
 
        //SceneManager.LoadScene("GAME OVER SCREEN");
    //}

   // }
}
