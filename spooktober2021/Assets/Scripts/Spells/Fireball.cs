using System.Collections;
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
        this.body.AddForce(base.firePoint.right * stats.speed, ForceMode2D.Impulse);
        audioSource.PlayOneShot(GetSFXByName("launch"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.PlayOneShot(GetSFXByName("hit"));
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponentInParent<Enemy>().TakeDamages(stats.damages);
        }

        if (!collision.CompareTag("EnemySpell"))
        {
            audioSource.GetComponent<DelayedDestroy>().enabled = true;
            audioSource.transform.parent = null;
            Destroy(this.gameObject);
        }
    }
}
