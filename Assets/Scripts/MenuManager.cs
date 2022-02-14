using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static string _activeScene;
    public static bool _gameIsPaused = false;
    public GameObject _pauseMenuUI;
    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Back()
    {
        Debug.Log(_activeScene);
        SceneManager.LoadScene(_activeScene);
    }

    public void Preferences()
    {
        _activeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("PreferencesScene");
        Debug.Log(_activeScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _gameIsPaused = false;
    }

    public void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _gameIsPaused = true;
    }
}
