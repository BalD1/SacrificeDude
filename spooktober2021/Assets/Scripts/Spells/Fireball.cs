using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spells
{
    private void Awake()
    {
        stats = spellInfos.SpellStats;
    }

    private void Start()
    {
        CallStart();
        Shoot();
    }


    public void Shoot()
    {
        this.body.AddForce(base.firePoint.up * stats.speed, ForceMode2D.Impulse);
    }
}
