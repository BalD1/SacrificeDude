using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public GameObject Player => player;

    #region instances
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("GameManger Instance not found.");

            return instance;
        }
    }
    #endregion

    #region Game State
    public enum GameStates
    {
        MainMenu,
        InCinematic,
        InGame,
        Pause,
        Gameover,
    }

    [SerializeField] private GameStates gameState;
    public GameStates GameState
    {
        get => gameState;
        set
        {
            switch (value)
            {
                case GameStates.MainMenu:
                    if (GetActiveSceneName().Equals("MainScene"))
                        LoadScene("MainMenu");

                    break;

                case GameStates.InCinematic:
                    break;

                case GameStates.InGame:
                    if (GetActiveSceneName().Equals("MainMenu"))
                        LoadScene("MainScene");
                    break;

                case GameStates.Pause:
                    break;

                case GameStates.Gameover:
                    break;

                default:
                    Debug.Log(value + " not found in switch statement.");
                    break;
            }
            UIManager.Instance.WindowManager(value);
            gameState = value;
        }
    }

    #endregion

    private void Awake()
    {
        instance = this;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (SceneManager.GetActiveScene().Equals("MainScene"))
            gameState = GameStates.InGame;
        else if (SceneManager.GetActiveScene().Equals("MainMenu"))
            gameState = GameStates.MainMenu;
    }

    public string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
