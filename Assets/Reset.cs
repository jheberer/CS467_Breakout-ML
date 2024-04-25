using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset: MonoBehaviour {

    public void Restart(){
        Time.timeScale = 1;
        Ball.instance.ResetIsMoving();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit(){
        /*if build
        Application.Quit();*/

        //if editor
        UnityEditor.EditorApplication.isPlaying = false;
    }
}