using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{

    private void Start()
    {
        CallStart();

        ai.maxSpeed = stats.speed;
        ai.endReachedDistance = stopDistance;
        _Death += OnDeathEvent;
        EnemyState = EnemyStates.Moving;
    }

    private void Update()
    {
        CallUpdate();
        if (enemyState == EnemyStates.Attacking)
            Attack();
    }

    private void FixedUpdate()
    {
        if (enemyState == EnemyStates.Moving)
            EnemyMovements();
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<Player>().TakeDamages(stats.damages);
    }
}
