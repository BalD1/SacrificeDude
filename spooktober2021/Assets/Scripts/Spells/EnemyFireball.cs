using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : Spells
{
    private void Awake()
    {
        CallStart();
    }


    public void Shoot(Transform firepoint, float newDamages)
    {
        this.stats.damages = newDamages;
        this.body.AddForce(firepoint.right * stats.speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.PlayOneShot(GetSFXByName("hit"));
        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInParent<Player>().TakeDamages(stats.damages);
        }

        audioSource.GetComponent<DelayedDestroy>().enabled = true;
        audioSource.transform.parent = null;
        Destroy(this.gameObject);
    }
}
