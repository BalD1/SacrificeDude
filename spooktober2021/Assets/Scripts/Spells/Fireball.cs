﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spells
{
    private void Awake()
    {
        CallStart();
    }

    private void Start()
    {
        Shoot();
    }


    public void Shoot()
    {
        this.body.AddForce(base.firePoint.up * stats.speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponentInParent<Enemy>().TakeDamages(stats.damages);
        }

        Destroy(this.gameObject);
    }
}