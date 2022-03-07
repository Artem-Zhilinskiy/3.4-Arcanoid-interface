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
    public GameObject _preferencesMenuUI;

    Coroutine _closeAnimationCoroutine = null;
    Coroutine _openAnimationCoroutine = null;
    Coroutine _twoCoroutinesCoroutine = null;

    //Объявление делегата для события перезапуска уровня
    public delegate void RestartLevelDelegate();

    //Объявление события перезапуска уровня
    public static event RestartLevelDelegate RestartLevelEvent;
    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    /*
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
    */

    public void Preferences()
    {
        var _panel = PanelSearch();
        _gameIsPaused = true;
        _preferencesMenuUI.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        _preferencesMenuUI.SetActive(true);
        _twoCoroutinesCoroutine = StartCoroutine(TwoCoroutinesCoroutine(_panel, _preferencesMenuUI));
    }

    public void Back()
    {
        var _panel = PanelSearch();
        _panel.SetActive(true);
        _twoCoroutinesCoroutine = StartCoroutine(TwoCoroutinesCoroutine(_preferencesMenuUI, _panel));
    }


    private void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            if (_pauseMenuUI.activeSelf == false)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        _gameIsPaused = false;
        ClosePanelAnimation(_pauseMenuUI);
    }

    public void Pause()
    {
        _gameIsPaused = true;
        _pauseMenuUI.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        OpenPanelAnimation(_pauseMenuUI);
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

    //метод сворачивания панели
    private void ClosePanelAnimation (GameObject _panel)
    {
            _closeAnimationCoroutine = StartCoroutine(CloseAnimationCoroutine(_panel));
            Debug.Log("ClosePanelAnimation сработал");
    }

    private void OpenPanelAnimation(GameObject _panel)
    {
            //Корутина увеличения масштаба панели меню
            _openAnimationCoroutine = StartCoroutine(OpenAnimationCoroutine(_panel));
            Debug.Log("OpenPanelAnimation сработал");
    }

    //метод поиска прикреплённой панели
    /*
    private GameObject PanelSearch()
    {
        var _panel = transform.Find("PauseMenuPanel").gameObject;
        if (_panel != null)
        {
            return _panel;
        }
        else
        {
            _panel = transform.Find("MainMenuPanel").gameObject;
            return _panel;
        }
    }
    */

    private GameObject PanelSearch()
    {
        _activeScene = SceneManager.GetActiveScene().name;
        if (_activeScene == "GameScene")
        {
            var _panel = transform.Find("PauseMenuPanel").gameObject;
            return _panel;
        }
        if (_activeScene == "MenuScene")
        {
            var _panel = transform.Find("MainMenuPanel").gameObject;
            return _panel;
        }
        else return null;
    }

    private IEnumerator CloseAnimationCoroutine(GameObject _panel)
    {
        while (_panel.transform.localScale.x >= 0.01)
        {
            _panel.transform.localScale = new Vector3(_panel.transform.localScale.x - 0.01f, _panel.transform.localScale.y - 0.01f, 1);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        _panel.SetActive(false);
        if (_gameIsPaused == false)
        {
            Time.timeScale = 1f;
        }
        yield break;
    }

    private IEnumerator OpenAnimationCoroutine(GameObject _panel)
    {
        while (_panel.transform.localScale.x < 1)
        {
            _panel.transform.localScale = new Vector3(_panel.transform.localScale.x + 0.01f, _panel.transform.localScale.y + 0.01f, 1);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        yield break;
    }

    private IEnumerator TwoCoroutinesCoroutine(GameObject _panelToClose, GameObject _panelToOpen)
    {
        yield return CloseAnimationCoroutine(_panelToClose);
        yield return OpenAnimationCoroutine(_panelToOpen);
        yield break;
    }

    public void StopGame()
    {
        UnityEditor.EditorApplication.isPaused = true;
        Debug.Log("Игра остановлена");
    }
}
