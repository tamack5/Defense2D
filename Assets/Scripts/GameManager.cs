using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Text scoreField;

    [SerializeField]
    GameOverMenu gameOverScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GameStart()
    {
        // Eh?
    }

    public void UpdateScore(float score)
    {
        if (scoreField != null)
        {
            scoreField.text = "Score: " + score;
        }
    }
    
    public void PlayerDied(float score)
    {
        gameOverScript.GameOver(score);
    }

}
