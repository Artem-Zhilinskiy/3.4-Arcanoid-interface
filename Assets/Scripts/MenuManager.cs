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

    Coroutine _closeAnimationCoroutine = null;
    Coroutine _openAnimationCoroutine = null;

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

    /*
    public void Preferences()
    {
        _activeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("PreferencesScene");
        Debug.Log(_activeScene);
    }
    */

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
            if (_pauseMenuUI.activeSelf == false)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {

        ClosePanelAnimation();
    }

    public void Pause()
    {
        _pauseMenuUI.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        OpenPanelAnimation();
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
    private void ClosePanelAnimation ()
    {
        var _panel = PanelSearch();
        //Корутина уменьшения масштаба панели меню
        _closeAnimationCoroutine = StartCoroutine(CloseAnimationCoroutine(_panel));
        Debug.Log("ClosePanelAnimation сработал");
    }

    private void OpenPanelAnimation()
    {
        var _panel = PanelSearch();
        //Корутина увеличения масштаба панели меню
        _openAnimationCoroutine = StartCoroutine(OpenAnimationCoroutine(_panel));
        Debug.Log("OpenPanelAnimation сработал");
    }

    //метод поиска прикреплённой панели
    private GameObject PanelSearch()
    {
        var _panel = transform.Find("PauseMenu").gameObject;
        return _panel;
    }

    private IEnumerator CloseAnimationCoroutine(GameObject _panel)
    {
        while (_panel.transform.localScale.x >= 0.01)
        {
            _panel.transform.localScale = new Vector3(_panel.transform.localScale.x - 0.01f, _panel.transform.localScale.y - 0.01f, 1);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        _panel.SetActive(false);
        Time.timeScale = 1f;
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
}
