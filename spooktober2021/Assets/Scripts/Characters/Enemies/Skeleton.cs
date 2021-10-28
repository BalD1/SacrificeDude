using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    [Header("Skeleton Related")]
    [SerializeField] private SkeletonEventHandler eventsHandler;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerLayer;

    private float attackAnimationTime;


    private void Start()
    {
        CallStart();

        if (target == null)
            target = GameManager.Instance.Player.transform;

        if (AIdestinationSetter.target == null)
            AIdestinationSetter.target = target;

        canAttack = true;
        ai.maxSpeed = stats.speed;
        ai.endReachedDistance = stopDistance;
        _Death += OnDeathEvent;
        EnemyState = EnemyStates.Moving;
        eventsHandler._OnAttackEvent += MeleeAttack;

        attackAnimationTime = GameManager.Instance.GetAnimationLength(animator, "Skeleton_Attack");
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
        animator.SetTrigger("attack");
        audioSource.PlayOneShot(GetSFXByName("startAttack"));

        StartCoroutine(WaitForAttack(attackAnimationTime));
    }

    private void MeleeAttack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        audioSource.PlayOneShot(GetSFXByName("attack"));

        if (hit != null)
        {
            Player player = hit.GetComponentInParent<Player>();
            player.TakeDamages(this.stats.damages);
        }
    }

    private IEnumerator WaitForAttack(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
        ai.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
