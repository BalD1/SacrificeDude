using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : Enemy
{
    [Header("Pumpkin related")]
    [SerializeField] private PumpkinEventsHandler eventsHandler;
    [SerializeField] private EnemyFireball fireball;
    [SerializeField] private Transform firepoint;
    [SerializeField] private float delayBetweenAttacks = 2;
    [SerializeField] private float randomOffset = 0.5f;


    private void Start()
    {
        CallStart();

        if (target == null)
            target = GameManager.Instance.Player.transform;

        eventsHandler._OnAttackEvent += LaunchFireball;

        canAttack = true;
        _Death += OnDeathEvent;

        delayBetweenAttacks -= Random.Range(-randomOffset, randomOffset);

        StartCoroutine(AttackCooldown(delayBetweenAttacks));
    }

    private void Update()
    {
        CallUpdate();
    }

    public void LaunchFireball()
    {
        Vector2 offset = target.position - this.transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        firepoint.transform.rotation = rot;
        EnemyFireball launchedFireball = Instantiate(fireball, firepoint.position, firepoint.rotation).GetComponent<EnemyFireball>();
        launchedFireball.Shoot(this.firepoint, newDamages: this.stats.damages);
        audioSource.PlayOneShot(GetSFXByName("attack"));

        StartCoroutine(AttackCooldown(delayBetweenAttacks));
    }

    private IEnumerator AttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetTrigger("attack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInParent<Player>().TakeDamages(stats.damages / 2);
        }
    }
}
