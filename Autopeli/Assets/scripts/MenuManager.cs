using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    public AudioSource audio;
    public void ChangeSceneByName(string name) {

        SceneManager.LoadScene(name);
    }

    public void playButton()
    {
        audio.Play();
    }
}
