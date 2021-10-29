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
    [SerializeField] protected bool isStatic = false;
    [SerializeField] protected bool flipOnlySprite = true;

    protected bool canAttack;

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
            if (!isStatic)
            {
                if (value == EnemyStates.Attacking)
                    ai.canMove = false;
                else
                    ai.canMove = true;
            }

            enemyState = value;
        }
    }

    public void SetCanMove(bool newCanMove)
    {
        ai.canMove = newCanMove;
    }

    public void SetEnemyTarget(GameObject newTarget)
    {
        if (!isStatic)
            this.AIdestinationSetter.target = newTarget.transform;
        this.target = newTarget.transform;
    }


    protected void HUDFollow()
    {
        HUD.transform.position = this.transform.position + HUDoffset;
    }

    protected void CallUpdate()
    {
        HUDFollow();
        CheckDistance();

        if (flipOnlySprite)
            FlipSpriteOnTargetPosition();
        else
            FlipObjectOnTargetPosition();
    }

    private void FlipSpriteOnTargetPosition()
    {
        this.sprite.flipX = target.transform.position.x > this.transform.position.x;
    }

    private void FlipObjectOnTargetPosition()
    {
        Vector2 scale = this.transform.localScale;
        if (target.transform.position.x > this.transform.position.x)
            scale.x = 1;
        else
            scale.x = -1;

        this.transform.localScale = scale;
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
        animator.enabled = false;
        Soul droppedSoul = Instantiate(soul, this.transform.position, Quaternion.identity).GetComponent<Soul>();
        droppedSoul.SetSoul(healAmount, soulScale);
        GameManager.Instance.CurrentEnemiesNumber--;
        Destroy(root);
    }

    public void TakeMeleeAttack(float damages, int strength, Transform attackerTransform)
    {
        base.TakeDamages(damages);
        if (!isStatic && knockbackResistance < 100)
        {
            ai.enabled = false;

            body.velocity = Vector2.zero;
            Vector2 direction = this.transform.position - attackerTransform.position;

            strength = Mathf.Clamp(strength - knockbackResistance, 0, strength);

            this.body.AddForce(direction.normalized * (strength - knockbackResistance), ForceMode2D.Impulse);
            StartCoroutine(Knockback(0.1f));
        }
    }

    private IEnumerator Knockback(float time)
    {
        yield return new WaitForSeconds(time);
        ai.enabled = true;
    }

}
