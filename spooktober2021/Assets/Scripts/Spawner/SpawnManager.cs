using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Spawn> spawns;
    [SerializeField] private float timeBetweenWaves = 5;
    private int spawnsIndex = 0;

    private void Start()
    {
        StartCoroutine(WaitForNextWave(timeBetweenWaves));
    }

    private void Update()
    {
        
    }

    private IEnumerator WaitForNextWave(float time)
    {
        yield return new WaitForSeconds(time);
        if (spawnsIndex >= spawns.Count)
            spawnsIndex = 0;
        spawns[spawnsIndex].enabled = true;
        spawnsIndex++;
    }
}
