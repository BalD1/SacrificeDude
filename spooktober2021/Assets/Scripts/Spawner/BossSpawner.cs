using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [HideInInspector] public GameObject enemyToSpawn;

    private void Start()
    {
        GameManager.Instance.GameState = GameManager.GameStates.InCinematic;
        GameManager.Instance.SetMainCameraPosition(Vector2.zero);
        StartCoroutine(WaitForCinematic(3));
    }

    private IEnumerator WaitForCinematic(float time)
    {
        yield return new WaitForSeconds(time / 2);

        Enemy enemy = Instantiate(enemyToSpawn, this.transform.position, Quaternion.identity).GetComponentInChildren<Enemy>();
        enemy.SetEnemyTarget(GameManager.Instance.Player);
        enemy.enabled = false;
        enemy.SetCanMove(false);
        GameManager.Instance.CurrentEnemiesNumber++;

        yield return new WaitForSeconds(time);

        GameManager.Instance.IsInWave = true;

        GameManager.Instance.GameState = GameManager.GameStates.InGame;
        enemy.enabled = true;

        yield return new WaitForSeconds(1);

        enemy.SetCanMove(true);
    }
}
