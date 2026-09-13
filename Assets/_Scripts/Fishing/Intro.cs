using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour
{
    public string sceneName = "GameScene";
    public InputActionReference click;
    bool loaded = false;

        // Enable the action when the script starts
    private void OnEnable()
    {
        if (click != null && click.action != null)
        {
            click.action.Enable();
        }
    }

    // Disable the action when the script is destroyed to avoid memory leaks
    private void OnDisable()
    {
        if (click != null && click.action != null)
        {
            click.action.Disable();
        }
    }

    void Update()
    {
        Debug.Log("frame " + Time.time);

        bool clicked = click.action.WasPressedThisFrame();

        if(clicked)
        {
            Debug.Log("clicked1 " + Time.time);

            if(!loaded)
            {
                Debug.Log("loaded2 " + Time.time);
                LoadGameScene();
                loaded = true;
            }
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("SCENE3 " + Time.time);
    }
}
