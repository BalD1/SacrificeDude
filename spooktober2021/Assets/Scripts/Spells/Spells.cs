using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    [SerializeField] protected SpellsScriptable spellInfos;
    [SerializeField] protected SpellsScriptable.stats stats;

    [Header("Components")]
    [SerializeField] protected Rigidbody2D body;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected GameObject objectLight;

    [Header("Audio")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected List<SFX> sounds;
    [System.Serializable]
    protected struct SFX
    {
        public string name;
        public AudioClip clip;
    }

    protected Transform firePoint;

    #endregion

    protected void CallStart()
    {
        stats = spellInfos.SpellStats;
    }

    public void SetFirePoint(Transform point)
    {
        firePoint = point;
    }

    public SpellsScriptable.stats GetStats()
    {
        return spellInfos.SpellStats;
    }

    public void DebugStats()
    {
        spellInfos.PrintSpell();
    }
    protected AudioClip GetSFXByName(string searchedAudio)
    {
        foreach (SFX sfx in sounds)
        {
            if (sfx.name.Equals(searchedAudio))
                return sfx.clip;
        }

        Debug.LogError(searchedAudio + " not found in " + this.name + " character.");
        return null;
    }
}
