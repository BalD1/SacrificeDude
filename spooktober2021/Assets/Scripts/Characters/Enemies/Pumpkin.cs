using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : Enemy
{
    [Header("Pumpkin related")]
    [SerializeField] private PumpkinEventsHandler eventsHandler;

    private void Start()
    {
        CallStart();

        eventsHandler._OnAttackEvent += LaunchFireball;

        canAttack = true;
        ai.maxSpeed = stats.speed;
        ai.endReachedDistance = stopDistance;
        _Death += OnDeathEvent;
        EnemyState = EnemyStates.Moving;

        animator.SetTrigger("attack");
    }

    public void LaunchFireball()
    {
        Debug.Log("yo");
    }
}
