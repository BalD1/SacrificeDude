using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Characters
{
    [Header("Enemy Related")]

    [SerializeField] protected GameObject root;
    [SerializeField] protected Transform target;
    [SerializeField] private GameObject HUD;
    [SerializeField] private Vector3 HUDoffset;
    [SerializeField] protected float stopDistance;
    [SerializeField] protected AIPath ai;

    [Header("Dropped Soul")]
    [SerializeField] protected GameObject soul;
    [SerializeField] protected Vector2 soulScale;
    [SerializeField] protected float healAmount;

    public enum EnemyStates
    {
        Moving,
        Attacking
    }
    protected EnemyStates enemyState;
    public EnemyStates EnemyState
    {
        get => enemyState;
        set
        {
            if (value == EnemyStates.Attacking)
                ai.canMove = false;
            else
                ai.canMove = true;

            enemyState = value;
        }
    }

    public void SetEnemyState(EnemyStates newState)
    {
        EnemyState = newState;
    }


    protected void EnemyMovements()
    {
        HUD.transform.position = this.transform.position + HUDoffset;
    }

    protected void CallUpdate()
    {
        if (EnemyState == EnemyStates.Moving)
        {
            LookAtTarget(target);
        }
        CheckDistance();
    }

    protected void CheckDistance()
    {
        if (Vector2.Distance(this.transform.position, target.transform.position) > stopDistance)
            EnemyState = EnemyStates.Moving;
        else
            EnemyState = EnemyStates.Attacking;
    }

    protected void OnDeathEvent()
    {
        Soul droppedSoul = Instantiate(soul, this.transform.position, Quaternion.identity).GetComponent<Soul>();
        droppedSoul.SetSoul(healAmount, soulScale);
        Destroy(root);
    }

}
