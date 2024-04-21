using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMEnu : MonoBehaviour
{
public void Exit () 
    {
        Application.Quit();
    
    }
    public void Play () 
    {

        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }
    public void MainMenu () 
    {

        SceneManager.LoadScene(0, LoadSceneMode.Single);


    }
    public void Restart () 
    {

        
        string currentSceneName = SceneManager.GetActiveScene().name;

        
        SceneManager.LoadScene(currentSceneName);

    }
}
