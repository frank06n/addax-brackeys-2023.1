using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{   
    public TMPro.TMP_Text trainingText;

    public void playGame() {
        if(PlayerPrefs.GetInt("hasPlayedTraining") == 1) {
            Debug.Log("Has played training and may start the game.");
            //SceneManager.LoadScene(LevelSceneNameGoesHere);
        } else {
            Debug.Log("Must play training before starting the game.");
            trainingText.text = "You must play training before starting the game.";
        }
    }

    public void playTraining() {
        Debug.Log("Training");
        PlayerPrefs.SetInt("hasPlayedTraining", 1);
        //SceneManager.LoadScene(TrainingSceneNameGoesHere);
    }

    public void quitGame(){
        Debug.Log("Quit");
        Application.Quit();
    }
}
