using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
}
