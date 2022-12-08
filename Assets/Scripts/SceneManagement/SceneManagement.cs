using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private static SceneManagement _instance;

    public static SceneManagement Instance { get { return _instance; } }



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Playground");
    }
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void LoadYouWin()
    {
        SceneManager.LoadScene("YouWin");
    }
    public void LoadHowTo()
    {
        SceneManager.LoadScene("HowTo");
    }
    public void LoadYouLose()
    {
        SceneManager.LoadScene("YouLose");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}