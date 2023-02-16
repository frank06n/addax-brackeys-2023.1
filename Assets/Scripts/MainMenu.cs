using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playGame() {
        SceneManager.LoadScene("SampleScene");
    }

    public void playTraining() {
        Debug.Log("Training");
        //SceneManager.LoadScene(TrainingSceneNameGoesHere);
    }

    public void quitGame(){
        Debug.Log("Quit");
        Application.Quit();
    }
}
