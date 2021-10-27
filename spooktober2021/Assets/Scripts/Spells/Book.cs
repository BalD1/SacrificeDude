using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Book : MonoBehaviour
{
    [SerializeField] private List<SpellsWithShopPage> spells;
    [System.Serializable]
    private struct SpellsWithShopPage
    {
        public Spells spell;
        public GameObject page;
        public TextMeshProUGUI pvCost;
        public TextMeshProUGUI damages;
        public TextMeshProUGUI soulCost;
    }
    private Player player;

    private void Start()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();

        foreach (SpellsWithShopPage individual in spells)
        {
            individual.pvCost.text += individual.spell.GetStats().pvCost;
            individual.damages.text += individual.spell.GetStats().damages;
            individual.soulCost.text += individual.spell.GetStats().soulCost;
        }
    }

    public void AddSpellToPlayer(string spellName)
    {
        foreach(SpellsWithShopPage individual in spells)
        {
            if (individual.spell.name.Equals(spellName))
            {
                if (player.TryBuySpell(individual.spell))
                {
                    Destroy(individual.page);
                }
                return;
            }    
        }
    }
}
