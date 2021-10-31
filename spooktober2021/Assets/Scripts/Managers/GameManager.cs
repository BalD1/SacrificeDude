using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorMainMenu;
    [SerializeField] private Texture2D cursorInGame;
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject player;

    [SerializeField] private Transform damagePopup;
    public Transform DamagePopup { get => damagePopup; }

    public GameObject Player => player;

    public GameObject vampireBoss;

    public bool lastWave;

    public delegate void HideHUDs();
    public HideHUDs hideHUDs;

    public delegate void ShowHUDs();
    public ShowHUDs showHUDs;

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
        Win,
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
            {
                if (lastWave)
                    GameState = GameStates.Win;

                UIManager.Instance.EndWave();
            }
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
                    Cursor.SetCursor(cursorMainMenu, Vector2.zero, CursorMode.Auto);
                    if (GetActiveSceneName().Equals("MainScene"))
                        LoadScene("MainMenu");

                    break;

                case GameStates.InCinematic:
                    Time.timeScale = 1;
                    break;

                case GameStates.InGame:
                    Time.timeScale = 1;
                    Cursor.SetCursor(cursorInGame, new Vector2(cursorInGame.width / 2, cursorInGame.height / 2), CursorMode.ForceSoftware);
                    if (GetActiveSceneName().Equals("MainMenu"))
                        LoadSceneAsync("MainScene");
                    showHUDs?.Invoke();
                    break;

                case GameStates.Pause:
                    Cursor.SetCursor(cursorMainMenu, Vector2.zero, CursorMode.Auto);
                    Time.timeScale = 0;
                    hideHUDs?.Invoke();
                    break;

                case GameStates.Gameover:
                    Cursor.SetCursor(cursorMainMenu, Vector2.zero, CursorMode.Auto);
                    Time.timeScale = 0;
                    hideHUDs?.Invoke();
                    break;

                case GameStates.Win:
                    Cursor.SetCursor(cursorMainMenu, Vector2.zero, CursorMode.Auto);
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
        Cursor.lockState = CursorLockMode.Confined;

        if (mainCam == null)
            mainCam = Camera.main;

        if (GetActiveSceneName().Equals("MainScene"))
        {
            GameState = GameStates.InGame;
            CurrentEnemiesNumber = 0;
        }
        else if (GetActiveSceneName().Equals("MainMenu"))
            GameState = GameStates.MainMenu;

    }

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
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
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsynch(sceneName));
    }
    private IEnumerator LoadAsynch(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            UIManager.Instance.UpdateProgressBar(progress);

            yield return null;
        }
    }
    public float GetAnimationLength(Animator animator, string searchedAnimation)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(searchedAnimation))
                return clip.length;
        }
        Debug.LogError(searchedAnimation + " not found in " + animator + ".");
        return 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameState == GameStates.InGame)
                GameState = GameStates.Pause;
            else if (GameState == GameStates.Pause)
            {
                if (UIManager.Instance.OptionsMenu.activeSelf)
                    UIManager.Instance.OptionsMenu.SetActive(false);
                else
                    GameState = GameStates.InGame;

            }
        }
    }

    public void SetMainCameraPosition(Vector2 pos)
    {
        Vector3 camPos = mainCam.transform.position;
        camPos.x = pos.x;
        camPos.y = pos.y;
        mainCam.transform.position = camPos;
    }
}
