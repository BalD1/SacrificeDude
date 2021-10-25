using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnPointData
    {
        public string enemyName;
        public GameObject enemy;
        public Transform point;
    }
    [SerializeField] private List<SpawnPointData> waveData;
    public List<SpawnPointData> WaveData
    {
        get => waveData;
    }

    private void OnEnable()
    {
        foreach(SpawnPointData data in waveData)
        {
            Enemy enemy = Instantiate(data.enemy, data.point.position, Quaternion.identity).GetComponentInChildren<Enemy>();
            enemy.SetEnemyTarget(GameManager.Instance.Player);
            GameManager.Instance.CurrentEnemiesNumber++;
        }
        this.enabled = false;
    }
}
