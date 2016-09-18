using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour {

    public void OnClicked()
    {
        SceneManager.LoadScene("MainGame");
    }
}
