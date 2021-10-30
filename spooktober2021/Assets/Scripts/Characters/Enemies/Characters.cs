using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Characters : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    [SerializeField] protected CharactersScriptable characterInfos;
    [SerializeField] protected CharactersScriptable.stats stats;
    [SerializeField] protected bool isPlayer;

    [Header("Components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D body;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Image hpBar;
    [SerializeField] private Material hitMaterial;
    [SerializeField] protected bool destroyOnDeath = true;
    private Material baseMaterial;

    [Header("Audio")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected List<SFX> sounds;
    [System.Serializable]
    protected struct SFX
    {
        public string name;
        public AudioClip clip;
    }

    protected Vector2 moveDirection;
    protected float moveX, moveY;

    protected bool isInvincible;

    protected delegate void DeathDelegate();
    protected DeathDelegate _Death;

    #endregion

    protected void CallStart()
    {
        stats = characterInfos.CharacterStats;
        baseMaterial = this.sprite.material;
    }

    protected void Movements(Vector2 direction)
    {
        body.MovePosition(body.position + direction * stats.speed * Time.fixedDeltaTime);
    }
    protected void MoveToTarget(Vector2 direction)
    {
        body.MovePosition(body.position + (direction * stats.speed * Time.fixedDeltaTime));
    }
    protected void LookAtTarget(Transform target)
    {
        Vector2 offset = target.position - this.transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg + 90f;
        body.rotation = angle;
    }

    public void TakeDamages(float amount)
    {
        if (amount == 0 || isInvincible)
            return;
        isInvincible = true;

        if (amount < 0)
            amount *= -1;
        this.stats.currentHP -= amount;


        if (this.stats.currentHP <= 0)
            Death();
        else
        {
            audioSource.PlayOneShot(GetSFXByName("hit"));
            this.sprite.material = hitMaterial;
            StartCoroutine(Hit(stats.invincibleTime));
        }

        UpdateHPBar();
    }

    public void Heal(float amount)
    {
        if (amount == 0)
            return;

        if (amount < 0)
            amount *= -1;
        this.stats.currentHP = Mathf.Clamp(this.stats.currentHP + amount, 0, this.stats.maxHP);

        UpdateHPBar();
    }

    protected void UpdateHPBar()
    {
        this.hpBar.fillAmount = this.stats.currentHP / this.stats.maxHP;
    }

    protected void Death()
    {
        if (_Death != null)
        {
            if (!isPlayer)
            {
                audioSource.PlayOneShot(GetSFXByName("death"));
                audioSource.GetComponent<DelayedDestroy>().enabled = true;
                audioSource.transform.parent = null;

                if (destroyOnDeath)
                    Destroy(this.gameObject);
            }

            _Death();
        }
        else
            Debug.Log("Death was not set");
    }

    protected AudioClip GetSFXByName(string searchedAudio)
    {
        foreach(SFX sfx in sounds)
        {
            if (sfx.name.Equals(searchedAudio))
                return sfx.clip;
        }

        Debug.LogError(searchedAudio + " not found in " + this.name + " character.");
        return null;
    }

    public CharactersScriptable.stats GetStats()
    {
        return this.stats;
    }

    private IEnumerator Hit(float time)
    {
        yield return new WaitForSeconds(time);
        isInvincible = false;
        this.sprite.material = baseMaterial;
    }
}
