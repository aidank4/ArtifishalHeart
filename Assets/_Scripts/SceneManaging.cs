using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManaging : MonoBehaviour
{


    public void GoMenu()
    {
        Debug.Log("Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void GoQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void GoPlay()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("IntroScene");
    }
}
