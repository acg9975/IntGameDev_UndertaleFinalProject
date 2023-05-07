using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.LoadScene("SampleScene 1");
            //Change to whatever the first level is in the modern version
        }
        else if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().name == "DeathScene") 
        {
            //load the last scene the player was at
            Vector3 pos = new Vector3(PlayerPrefs.GetFloat("lastOverworldX"), PlayerPrefs.GetFloat("lastOverworldY"), 0);
            SceneTransition.ChangeScene(PlayerPrefs.GetString("lastScene"), pos);
        }
    }
}
