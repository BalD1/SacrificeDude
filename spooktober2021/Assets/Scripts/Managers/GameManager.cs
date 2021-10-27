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
    private bool isInWave = false;
    public bool IsInWave
    {
        get => isInWave;
        set
        {
            isInWave = value;
            if (isInWave)
                UIManager.Instance.StartWave();
            else
                UIManager.Instance.EndWave();
        }
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
                    Time.timeScale = 1;
                    if (GetActiveSceneName().Equals("MainScene"))
                        LoadScene("MainMenu");

                    break;

                case GameStates.InCinematic:
                    Time.timeScale = 1;
                    break;

                case GameStates.InGame:
                    Time.timeScale = 1;
                    if (GetActiveSceneName().Equals("MainMenu"))
                        LoadScene("MainScene");
                    break;

                case GameStates.Pause:
                    Time.timeScale = 0;
                    break;

                case GameStates.Gameover:
                    Time.timeScale = 0;
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

        if (GetActiveSceneName().Equals("MainScene"))
            gameState = GameStates.InGame;
        else if (GetActiveSceneName().Equals("MainMenu"))
            gameState = GameStates.MainMenu;

        CurrentEnemiesNumber = 0;
    }

    private int currentEnemiesNumber;
    public int CurrentEnemiesNumber
    {
        get => currentEnemiesNumber;
        set
        {
            currentEnemiesNumber = value;
            if (currentEnemiesNumber == 0)
                IsInWave = false;
        }
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
