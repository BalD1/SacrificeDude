using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] private float healAmount = 1;
    [SerializeField] private ParticleSystem particles;

    private void Start()
    {
        particles.gameObject.transform.localScale = this.transform.localScale;
    }

    public void SetSoul(float newHealAmount, Vector2 newScale)
    {
        this.healAmount = newHealAmount;
        this.transform.localScale = newScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.Heal(healAmount);
            player.SoulsCount += 1;
            Destroy(this.gameObject);
        }
    }
}
