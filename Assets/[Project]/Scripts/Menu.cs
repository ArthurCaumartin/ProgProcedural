using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private string _gameSceneName;
    public void StartGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
