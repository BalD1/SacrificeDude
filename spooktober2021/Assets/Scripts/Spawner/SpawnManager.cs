using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Spawn> spawns;
    [SerializeField] private AudioSource audioSource;
    private int spawnsIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.GameState == GameManager.GameStates.InGame)
        {
            if (!GameManager.Instance.IsInWave)
            {
                SpawnNextWave();
            }
        }
    }

    private void SpawnNextWave()
    {
        audioSource.Play();

        spawns[spawnsIndex].enabled = true;
        spawnsIndex++;

        if (spawnsIndex == spawns.Count)
            GameManager.Instance.lastWave = true;
    }
}
