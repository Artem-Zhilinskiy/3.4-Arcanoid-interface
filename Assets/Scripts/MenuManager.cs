using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static string _activeScene;
    public static bool _gameIsPaused = false;
    public GameObject _pauseMenuUI;

    //Объявление делегата для события перезапуска уровня
    public delegate void RestartLevelDelegate();

    //Объявление события перезапуска уровня
    public static event RestartLevelDelegate RestartLevelEvent;
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
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
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

    //Вызов события в методе
    public void RestartLevel()
    {
        if (RestartLevelEvent != null)
        {
            RestartLevelEvent();
            Resume();
        }
    }
}
