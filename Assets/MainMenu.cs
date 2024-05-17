using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SinglePlayer()
    {
        Time.timeScale = 1;
        Ball.instance.ResetIsMoving();
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("game quit");
        Application.Quit();
    }

    public void TwoPlayer()
    {
        Time.timeScale = 1;
        Ball.instance.ResetIsMoving();
        SceneManager.LoadScene("2PModeScene");
    }

}
