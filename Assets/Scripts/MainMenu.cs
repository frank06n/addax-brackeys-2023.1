using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{   
    public TMPro.TMP_Text trainingText;
    public AudioManager audioPlayer;

    void Start(){
        audioPlayer.Play("music_mainMenu");
    }

    public void playGame() {
        audioPlayer.Play("sfx_enterGame");
        if(PlayerPrefs.GetInt("hasPlayedTraining") == 1) {
            Debug.Log("Has played training and may start the game.");
            //SceneManager.LoadScene(LevelSceneNameGoesHere);
        } else {
            Debug.Log("Must play training before starting the game.");
            trainingText.text = "You must play training before starting the game.";
        }
    }

    public void playTraining() {
        audioPlayer.Play("sfx_enterGame");
        Debug.Log("Training");
        PlayerPrefs.SetInt("hasPlayedTraining", 1);
        //SceneManager.LoadScene(TrainingSceneNameGoesHere);
    }

    public void quitGame(){
        audioPlayer.Play("sfx_warning");
        Debug.Log("Quit");
        Application.Quit();
    }
}
