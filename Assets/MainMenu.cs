using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SinglePlayer(string scene_name)
    {
        Time.timeScale = 1;
        Ball.instance.ResetIsMoving();
        SceneManager.LoadScene(scene_name);
    }

    public void QuitGame()
    {
        Debug.Log("game quit");
        Application.Quit();
    }
}