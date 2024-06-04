using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset: MonoBehaviour {

    public void Restart(){
        Time.timeScale = 1;
        Ball.instance.ResetIsMoving();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit(){
        Debug.Log("quit game");
        Application.Quit();

        //if editor
        // UnityEditor.EditorApplication.isPlaying = false;
    }

    public void ReturnToMenu(string scene_name) {
        SceneManager.LoadScene(scene_name);
    }
}