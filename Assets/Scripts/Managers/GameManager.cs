using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    enum GameState
    {
        Default,
        Paused,
        PlayerDied
    }

    private GameState _gameState;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (_gameState != GameState.Paused)
        {
            Debug.Log("Game Paused");
            _gameState = GameState.Paused;
            Time.timeScale = 0;
        }
        else
        {
            Debug.Log("Game unpaused");
            _gameState = GameState.Default;
            Time.timeScale = 1;
        }
    }

    public void PlayerDied()
    {
        if (_gameState != GameState.PlayerDied)
        {
            Debug.Log("Player died");
            _gameState = GameState.PlayerDied;
        }
    }

    // singleton pattern
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
}
