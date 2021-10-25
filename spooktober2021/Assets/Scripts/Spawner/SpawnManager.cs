using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Spawn> spawns;
    [SerializeField] private float timeBetweenWaves = 5;
    private int spawnsIndex = 0;

    private enum State
    {
        Spawning,
        Idle,
    }
    private State state;

    private void Start()
    {
        StartCoroutine(WaitForNextWave(timeBetweenWaves));
        state = State.Spawning;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentEnemiesNumber <= 0 && state == State.Idle)
        {
            state = State.Spawning;
            StartCoroutine(WaitForNextWave(timeBetweenWaves));
        }
    }

    private IEnumerator WaitForNextWave(float time)
    {
        yield return new WaitForSeconds(time);
        if (spawnsIndex >= spawns.Count)
            spawnsIndex = 0;
        spawns[spawnsIndex].enabled = true;
        spawnsIndex++;

        state = State.Idle;
    }
}
