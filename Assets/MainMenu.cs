using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SinglePlayer(string scene_name)
    {
        Time.timeScale = 1;
        if (Ball.instance != null)
        {
            Ball.instance.ResetIsMoving();
        }
        SceneManager.LoadScene(scene_name);
    }

    public void QuitGame()
    {
        Debug.Log("game quit");
        Application.Quit();
    }

    public void TwoPlayer()
    {
        Time.timeScale = 1;
        if (Ball.instance != null)
        {
            Ball.instance.ResetIsMoving();
        }

        // if (AgentBall.agentInstance != null)
        // {
        //     AgentBall.agentInstance.ResetIsMoving();
        // }
        if (AgentBall.instance != null)
        {
            AgentBall.instance.ResetIsMoving();
        }
        SceneManager.LoadScene("2PModeScene");
    }

}
