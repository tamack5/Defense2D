using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour {

    // Only used on the canvas level
    [SerializeField]
    Text scoreField;

    //
    // ONLY used at the CANVAS level
    //


    public void GameOver(float score)
    {
        if (scoreField != null)
            scoreField.text = score.ToString();
        else
            scoreField.text = "???";

        // Make the game over canvas visible
        gameObject.SetActive(true);
    }

    //overload
    public void GameOver(int score)
    {
        GameOver(float.Parse(score.ToString()));
    }

    //
    //
    // Below functions: Only to be used in buttons
    //
    //

    // Reload the current scene (restart current level!)
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }


}
