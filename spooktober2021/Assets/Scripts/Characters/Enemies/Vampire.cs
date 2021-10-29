using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{

    private void Start()
    {
        CallStart();

        if (target == null)
            target = GameManager.Instance.Player.transform;

        canAttack = true;
        ai.maxSpeed = stats.speed;
        ai.endReachedDistance = stopDistance;
        _Death += OnDeathEvent;
        EnemyState = EnemyStates.Moving;
    }

    private void Update()
    {
        CallUpdate();
        if (enemyState == EnemyStates.Attacking && canAttack)
            Attack();
    }


    private void Attack()
    {
        ai.enabled = false;
        canAttack = false;
        body.velocity = Vector2.zero;

        audioSource.PlayOneShot(GetSFXByName("attack"));

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
