using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    [Header("Vampire related")]
    public List<Transform> spellPoints;

    private void Start()
    {
        CallStart();

        if (target == null)
            target = GameManager.Instance.Player.transform;

        GameManager.Instance.vampireBoss = this.root;

        canAttack = true;
        ai.maxSpeed = stats.speed;
        ai.endReachedDistance = stopDistance;
        _Death += OnDeathEvent;
        EnemyState = EnemyStates.Moving;
    }

    private void Update()
    {
        CallUpdate();
    }

    public void TeleportToCenter()
    {
        Vector2 pos = new Vector2(0, 1);
        this.transform.position = pos;
    }

    public void Attack()
    {
        enemyState = EnemyStates.Attacking;
        ai.enabled = false;
        canAttack = false;
        body.velocity = Vector2.zero;
    }

    public void StopAttack()
    {
        enemyState = EnemyStates.Moving;
        ai.enabled = true;
        canAttack = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<Player>().TakeDamages(stats.damages);
    }

    private IEnumerator WaitForAttack(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
        ai.enabled = true;
    }
}
