using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{   
    public AudioManager audioPlayer;

    void Start(){
        audioPlayer.Play("music_mainMenu");
    }

    public void playGame() {
        audioPlayer.Play("sfx_enterGame");
        //SceneManager.LoadScene(LevelSceneNameGoesHere);
        
    }

    /*public void playTraining() {
        audioPlayer.Play("sfx_enterGame");
        Debug.Log("Training");
        PlayerPrefs.SetInt("hasPlayedTraining", 1);
        //SceneManager.LoadScene(TrainingSceneNameGoesHere);
    }*/

    public void quitGame(){
        audioPlayer.Play("sfx_warning");
        Debug.Log("Quit");
        Application.Quit();
    }
}
