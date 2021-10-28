﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Spawn> spawns;
    private int spawnsIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.GameState == GameManager.GameStates.InGame)
        {
            if (!GameManager.Instance.IsInWave)
            {
                GameManager.Instance.IsInWave = true;
                SpawnNextWave();
            }
        }
    }

    private void SpawnNextWave()
    {
        if (spawnsIndex >= spawns.Count)
            spawnsIndex = 0;
        spawns[spawnsIndex].enabled = true;
        spawnsIndex++;
    }
}
