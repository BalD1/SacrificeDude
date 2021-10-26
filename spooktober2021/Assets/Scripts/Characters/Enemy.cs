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
    [SerializeField] protected AIDestinationSetter AIdestinationSetter;
    [SerializeField] private int knockbackResistance = 0;

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

    public void SetEnemyTarget(GameObject newTarget)
    {
        this.AIdestinationSetter.target = newTarget.transform;
        this.target = newTarget.transform;
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
        GameManager.Instance.CurrentEnemiesNumber--;
        Destroy(root);
    }

    public void TakeMeleeAttack(float damages, int strength, Transform attackerTransform)
    {
        ai.enabled = false; 
        body.velocity = Vector2.zero;
        base.TakeDamages(damages);
        Vector2 direction = this.transform.position - attackerTransform.position;

        strength = Mathf.Clamp(strength - knockbackResistance, 0, strength);

        this.body.AddForce(direction.normalized * (strength - knockbackResistance),ForceMode2D.Impulse);
        StartCoroutine(Knockback(0.1f));
    }

    private IEnumerator Knockback(float time)
    {
        yield return new WaitForSeconds(time);
        ai.enabled = true;
    }

}
