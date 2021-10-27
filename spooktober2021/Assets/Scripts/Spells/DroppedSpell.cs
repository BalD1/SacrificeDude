using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedSpell : MonoBehaviour
{
    [SerializeField] private Spells spell;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().UnlockSpell(spell);
            Destroy(this.gameObject);
        }
    }
}
