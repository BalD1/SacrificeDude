using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    public GameObject OptionsMenu { get => optionsMenu; }
    [SerializeField] private GameObject HUD;
    public GameObject GetHUD { get => HUD; }
    [SerializeField] private GameObject blackBars;
    public GameObject BlackBars { get => blackBars; }
    public Animator BlackBarsAnimator { get => blackBars.GetComponent<Animator>(); }
    [SerializeField] private GameObject shopMenu;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject gameoverScreen;

    [SerializeField] private Image progressBar;

    [Header("Player related")]
    [SerializeField] private TextMeshProUGUI playerSoulsCount;
    [SerializeField] private Color unselectedSpellTransparence;
    [SerializeField] private GameObject spellImagesContainer;
    [SerializeField] private GameObject nextWaveText;
    public List<Image> spellImages;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("UIManager Instance not found.");

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;

        if(spellImagesContainer != null)
        {
            spellImages = new List<Image>();
            foreach (Transform child in spellImagesContainer.transform)
            {
                spellImages.Add(child.GetComponent<Image>());
            }
        }
    }


    public void WindowManager(GameManager.GameStates value)
    {
        switch (value)
        {
            case GameManager.GameStates.MainMenu:
                break;

            case GameManager.GameStates.InCinematic:
                BlackBarsAnimator.SetTrigger("appear"); 

                nextWaveText.SetActive(false);

                if (shopMenu.activeSelf)
                    shopMenu.SetActive(false);
                break;

            case GameManager.GameStates.InGame:
                if (pauseMenu != null)
                    pauseMenu.SetActive(false);

                if (blackBars != null)
                    BlackBarsAnimator.SetTrigger("disappear");

                break;

            case GameManager.GameStates.Pause:
                pauseMenu.SetActive(true);
                nextWaveText.SetActive(false);
                break;

            case GameManager.GameStates.Gameover:
                gameoverScreen.SetActive(true);
                HUD.SetActive(false);
                nextWaveText.SetActive(false);
                break;

            case GameManager.GameStates.Win:
                winScreen.SetActive(true);
                HUD.SetActive(false);
                break;

            default:
                Debug.Log(value + " not found in switch statement.");
                break;
        }
    }

    public void OnButtonClick(string button)
    {
        switch (button)
        {
            case "play":
                GameManager.Instance.GameState = GameManager.GameStates.InGame;
                break;

            case "options":
                break;

            case "quit":
                GameManager.Instance.QuitGame();
                break;

            case "continue":
                GameManager.Instance.GameState = GameManager.GameStates.InGame;
                break;

            case "mainmenu":
                GameManager.Instance.GameState = GameManager.GameStates.MainMenu;
                break;

            default:
                Debug.LogError(button + " not found in switch statement.");
                break;
        }
    }

    public void UpdateSoulsCount(int value)
    {
        playerSoulsCount.text = " x " + value;
    }

    public void AddNewSpell(Sprite image)
    {
        foreach(Image spellImage in spellImages)
        {
            if (spellImage.color.a == 0)
            {
                spellImage.sprite = image;
                spellImage.color = unselectedSpellTransparence;
                return;
            }
        }
    }

    public void UpdateEquipedSpell(int index)
    {
        for(int i = 0; i < spellImages.Count; i++)
        {
            Color imageColor = spellImages[i].color;
            if (i == index)
                imageColor.a = 1;
            else if (imageColor.a != 0)
                imageColor = unselectedSpellTransparence;

            spellImages[i].color = imageColor;
        }
    }

    public void StartWave()
    {
        nextWaveText.SetActive(false);
    }
    public void EndWave()
    {
        nextWaveText.SetActive(true);
    }

    public void UpdateProgressBar(float val)
    {
        progressBar.fillAmount = val;
    }

}
