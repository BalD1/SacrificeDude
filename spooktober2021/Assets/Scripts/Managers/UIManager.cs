using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
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
        spellImages = new List<Image>();
        foreach (Transform child in spellImagesContainer.transform)
        {
            spellImages.Add(child.GetComponent<Image>());
        }
    }


    public void WindowManager(GameManager.GameStates value)
    {
        switch (value)
        {
            case GameManager.GameStates.MainMenu:
                break;

            case GameManager.GameStates.InCinematic:
                break;

            case GameManager.GameStates.InGame:
                break;

            case GameManager.GameStates.Pause:
                break;

            case GameManager.GameStates.Gameover:
                break;

            default:
                Debug.Log(value + " not found in switch statement.");
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

}
