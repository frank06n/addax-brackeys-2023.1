using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject gameplayUI;

    void Awake(){
        pauseMenuUI.SetActive(false);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                resume();
            } else {
                pause();
            }
        }
    }

    public void pause(){
        pauseMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void resume(){
        pauseMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void exit(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
