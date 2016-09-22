using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

    public void ClickStartMainGame()
    {
        SceneManager.LoadScene("Waves");
    }

    public void ClickStartProceduralGame()
    {
        SceneManager.LoadScene("Procedural");
    }
}
