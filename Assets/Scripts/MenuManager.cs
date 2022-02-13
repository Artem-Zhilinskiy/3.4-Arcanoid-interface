using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static string _activeScene;
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
}
