using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Characters
{
    [Header("Player Related")]
    [SerializeField] private Camera cam;

    [SerializeField] private Transform firePoint;

    [SerializeField] private List<Spells> unlockedSpells;
    private Spells currentSelectedSpell;

    [SerializeField] private LayerMask enemyLayer;

    private Vector2 mousePos;

    [Header("Melee attack")]
    [SerializeField] private Transform meleePoint;
    [SerializeField] private float meleeAttackCooldown = 0.5f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float meleeDamages;
    [SerializeField] private int knockbackStrength = 10;
    private float meleeAttackTimer;

    private bool canFireSpell = true;
    private bool canMeleeAttack = true;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    private void Start()
    {
        CallStart();
        currentSelectedSpell = unlockedSpells[0];
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameManager.GameStates.InGame)
        {
            ProcessInputs();
            CameraFollow();

            if (meleeAttackTimer > 0)
            {
                meleeAttackTimer -= Time.deltaTime;

                if (meleeAttackTimer <= 0)
                    canMeleeAttack = true;
            }
        }

    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState == GameManager.GameStates.InGame)
        {
            Movements(moveDirection);
            RotateByMousePosition();
        }
    }

    private void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && canFireSpell)
        {
            FireSpell();
        }

        if (Input.GetMouseButtonDown(1) && canMeleeAttack)
        {
            MeleeAttack();
        }
    }

    private void RotateByMousePosition()
    {
        Vector2 lookDirection = mousePos - body.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        body.rotation = angle;  
    }

    private void FireSpell()
    {
        canFireSpell = false;
        PutMeleeAttackOnCooldown();

        Spells spell = currentSelectedSpell.GetComponent<Spells>();

        TakeDamages(spell.GetStats().cost);

        StartCoroutine(SpellCooldown(spell.GetStats().cooldown));

        spell = Instantiate(currentSelectedSpell.gameObject, this.firePoint.position, this.firePoint.rotation).GetComponent<Spells>();
        spell.SetFirePoint(this.firePoint);
    }

    private void MeleeAttack()
    {
        canFireSpell = false;
        PutMeleeAttackOnCooldown();
        StartCoroutine(SpellCooldown(meleeAttackCooldown));

        animator.SetTrigger("attack_melee");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponentInParent<Enemy>().TakeMeleeAttack(meleeDamages, knockbackStrength, this.transform);
        }
    }

    private void PutMeleeAttackOnCooldown()
    {
        canMeleeAttack = false;
        meleeAttackTimer = meleeAttackCooldown;
    }

    private IEnumerator SpellCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFireSpell = true;
    }

    private void CameraFollow()
    {
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
    }

    private void OnDrawGizmosSelected()
    {
        if (meleePoint == null)
            return;

        Gizmos.DrawWireSphere(meleePoint.position, attackRange); 
    }
}
