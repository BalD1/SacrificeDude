﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Spells
{
    [Header("Lightning related")]

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float searchEnemyRadius = 10;
    [SerializeField] private GameObject particles;

    private GameObject enemySource;
    private Transform target;

    public int bounces = 2;

    private void Awake()
    {
        CallStart();
    }

    private void Start()
    {
        Shoot(base.firePoint.right);
    }

    private void FixedUpdate()
    {
    }


    public void Shoot(Vector2 direction)
    {
        this.body.AddForce(direction * stats.speed, ForceMode2D.Impulse);
        audioSource.PlayOneShot(GetSFXByName("electric"));
    }

    private void TravelToTarget()
    {
        if (target != null)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, target.position, 1);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.PlayOneShot(GetSFXByName("electric"));


        if (collision.CompareTag("Enemy") && collision.gameObject != enemySource)
        {
            enemySource = collision.gameObject;

            collision.GetComponentInParent<Enemy>().TakeDamages(stats.damages);
            if (bounces > 0)
            {
                bounces -= 1;

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(this.transform.position, searchEnemyRadius, enemyLayer);

                if (hitEnemies.Length > 1)  //hitEnnemies[0] will be the source
                {
                    target = hitEnemies[1].transform;
                    this.body.velocity = Vector2.zero;
                    Shoot((target.transform.position - this.transform.position).normalized);
                }
                else
                {
                    audioSource.GetComponent<DelayedDestroy>().enabled = true;
                    audioSource.transform.parent = null;
                    Destroy(this.gameObject);
                }
            }
            else
            {
                audioSource.GetComponent<DelayedDestroy>().enabled = true;
                audioSource.transform.parent = null;
                Destroy(this.gameObject);
            }
        }
        else if (!collision.CompareTag("Enemy"))
            if (!collision.CompareTag("EnemySpell"))
            {
                audioSource.GetComponent<DelayedDestroy>().enabled = true;
                audioSource.transform.parent = null;
                Destroy(this.gameObject);
            }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, searchEnemyRadius);
    }
}
