using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [Header("Кнопки главного меню")]
    [SerializeField]
    private Button _newGameButtonMain;
    [SerializeField]
    private Button _preferencesButtonMain;
    [SerializeField]
    private Button _exitButtonMain;

    [Header("Кнопки меню настроек")]
    [SerializeField]
    private Button _backButton;

    [Header("Кнопки меню-паузы")]
    [SerializeField]
    private Button _resumeButton;
    [SerializeField]
    private Button _restartButton;
    [SerializeField]
    private Button _preferencesButton;
    [SerializeField]
    private Button _exitButton;

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

    //Открытие меню настроек
    public void Preferences()
    {
        var _panel = PanelSearch();
        _gameIsPaused = true;
        _preferencesMenuUI.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        _preferencesMenuUI.SetActive(true);
        _twoCoroutinesCoroutine = StartCoroutine(TwoCoroutinesCoroutine(_panel, _preferencesMenuUI));
    }

    //Кнопка "назад" меню настроек
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

    //Кнопка "продолжить" меню-паузы
    public void Resume()
    {
        _gameIsPaused = false;
        ClosePanelAnimation(_pauseMenuUI);
    }

    //Открытие меню-паузы на кнопку Escape
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
    private void ClosePanelAnimation(GameObject _panel)
    {
        _closeAnimationCoroutine = StartCoroutine(CloseAnimationCoroutine(_panel));
        //Debug.Log("ClosePanelAnimation сработал");
    }

    private void OpenPanelAnimation(GameObject _panel)
    {
        //Корутина увеличения масштаба панели меню
        _openAnimationCoroutine = StartCoroutine(OpenAnimationCoroutine(_panel));
        //Debug.Log("OpenPanelAnimation сработал");
    }

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
        AllButtonsTurnOn();
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

    #region "включение и отключение кнопок"
    //Метод отключения кнопок главного меню
    private void UnButtonMainMenu()
    {
        _newGameButtonMain.interactable = false;
        _preferencesButtonMain.interactable = false;
        _exitButtonMain.interactable = false;
    }

    //Метод включения кнопок главного меню
    private void ButtonMainMenu()
    {
        _newGameButtonMain.interactable = true;
        _preferencesButtonMain.interactable = true;
        _exitButtonMain.interactable = true;
    }

    //Метод отключения кнопок меню настроек
    private void UnButtonPreferencesMenu()
    {
        _backButton.interactable = false;
    }

    //Метод включения кнопки меню настроек
    private void ButtonPreferencesMenu()
    {
        _backButton.interactable = true;
    }

    //Метод отключения кнопок меню паузы
    private void UnButtonPauseMenu()
    {
        _resumeButton.interactable = false;
        _restartButton.interactable = false;
        _preferencesButton.interactable = false;
        _exitButton.interactable = false;
    }

    //Метод включения кнопок меню
    private void ButtonPauseMenu()
    {
        _resumeButton.interactable = true;
        _restartButton.interactable = true;
        _preferencesButton.interactable = true;
        _exitButton.interactable = true;
    }

    //Методы включения и отключения всех кнопок
    private void AllButtonsTurnOn()
    {
        ButtonCheckTurnOn(_newGameButtonMain);
        ButtonCheckTurnOn(_preferencesButtonMain);
        ButtonCheckTurnOn(_exitButtonMain);
        ButtonCheckTurnOn(_backButton);
        ButtonCheckTurnOn(_resumeButton);
        ButtonCheckTurnOn(_preferencesButton);
        ButtonCheckTurnOn(_restartButton);
        ButtonCheckTurnOn(_exitButton);
    }

    public void AllButtonsTurnOff()
    {
        ButtonCheckTurnOff(_newGameButtonMain);
        ButtonCheckTurnOff(_preferencesButtonMain);
        ButtonCheckTurnOff(_exitButtonMain);
        ButtonCheckTurnOff(_backButton);
        ButtonCheckTurnOff(_resumeButton);
        ButtonCheckTurnOff(_preferencesButton);
        ButtonCheckTurnOff(_restartButton);
        ButtonCheckTurnOff(_exitButton);
    }

    //Метод проверки, существует ли кнопка
    private void ButtonCheckTurnOn(Button _button)
    {
        if (_button != null)
        {
            _button.interactable = true;
        }
    }
    private void ButtonCheckTurnOff(Button _button)
    {
        if (_button != null)
        {
            _button.interactable = false;
        }
    }
    #endregion
}
