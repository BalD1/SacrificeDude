using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    [Header("Bat related")]
    [SerializeField] private float dashSpeed = 2;

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
        ai.enabled = false;
        body.velocity = Vector2.zero;
        Vector2 direction = GameManager.Instance.Player.transform.position - this.transform.position;
        body.AddForce((direction.normalized * 2) * 10, ForceMode2D.Impulse);
        StartCoroutine(WaitForAttack(0.3f));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<Player>().TakeDamages(stats.damages);
    }

    private IEnumerator WaitForAttack(float time)
    {
        yield return new WaitForSeconds(time);
        ai.enabled = true;
    }
}
