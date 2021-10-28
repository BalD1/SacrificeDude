using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Characters
{
    [Header("Player Related")]
    [SerializeField] private Camera cam;

    [SerializeField] private GameObject arm;
    [SerializeField] private Transform firePoint;

    [SerializeField] private List<Spells> unlockedSpells;
    private Spells currentSelectedSpell;
    private int currentSpellIndex;

    [SerializeField] private LayerMask enemyLayer;

    private Vector2 mousePos;

    private int soulsCount = 0;
    public int SoulsCount
    {
        get => soulsCount;
        set
        {
            soulsCount = value;
            UIManager.Instance.UpdateSoulsCount(soulsCount);
        }
    }

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
        SoulsCount = 0;

        if (unlockedSpells.Count > 0)
            EquipSpell(0);
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameManager.GameStates.InGame)
        {
            ProcessInputs();
            CameraFollow();
            ChangeSpellOnScroll();


            if (meleeAttackTimer > 0)
            {
                meleeAttackTimer -= Time.deltaTime;

                if (meleeAttackTimer <= 0)
                    canMeleeAttack = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.GameState == GameManager.GameStates.InGame)
                GameManager.Instance.GameState = GameManager.GameStates.Pause;
            else if (GameManager.Instance.GameState == GameManager.GameStates.Pause)
                GameManager.Instance.GameState = GameManager.GameStates.InGame;
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

    #region Movements / Inputs

    private void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && canFireSpell && !IsMouseOverUI() && currentSelectedSpell != null)
        {
            FireSpell();
        }

        if (Input.GetMouseButtonDown(1) && canMeleeAttack && !IsMouseOverUI())
        {
            MeleeAttack();
        }
    }

    private void RotateByMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        Vector2 selfPosByCam = cam.WorldToScreenPoint(this.transform.position);

        mousePosition.x -= selfPosByCam.x;
        mousePosition.y -= selfPosByCam.y;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (angle > -90f && angle < 90f)
        {
            this.transform.localScale = new Vector2(1, 1);
            arm.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            this.transform.localScale = new Vector2(-1, 1); 
            arm.transform.localScale = new Vector2(-1, 1);
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    #endregion

    #region Attacks

    private void FireSpell()
    {
        Spells spell = currentSelectedSpell.GetComponent<Spells>();
        if (spell.GetStats().pvCost >= this.stats.currentHP)
            return;

        canFireSpell = false;
        PutMeleeAttackOnCooldown();


        TakeDamages(spell.GetStats().pvCost);

        StartCoroutine(SpellCooldown(spell.GetStats().cooldown));

        spell = Instantiate(currentSelectedSpell.gameObject, this.firePoint.position, this.arm.transform.rotation).GetComponent<Spells>();
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

    #endregion

    #region Spells

    private void ChangeSpellOnScroll()
    {
        if (unlockedSpells.Count != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (currentSpellIndex >= unlockedSpells.Count - 1)
                    EquipSpell(0);
                else
                    EquipSpell(currentSpellIndex + 1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (currentSpellIndex <= 0)
                    EquipSpell(unlockedSpells.Count - 1);
                else
                    EquipSpell(currentSpellIndex - 1);
            }
        }
    }

    private void EquipSpell(int index)
    {
        currentSpellIndex = index;
        currentSelectedSpell = unlockedSpells[index];
        UIManager.Instance.UpdateEquipedSpell(index);
    }

    public void UnlockSpell(Spells newSpell)
    {
        unlockedSpells.Add(newSpell);
        UIManager.Instance.AddNewSpell(newSpell.GetStats().UIimage);
        if (unlockedSpells.Count == 1)
            EquipSpell(0);
    }

    public bool TryBuySpell(Spells spell)
    {
        int spellCost = spell.GetStats().soulCost;

        if (spellCost > this.soulsCount)
            return false;
        else
        {
            SoulsCount -= spellCost;
            UnlockSpell(spell);
            return true;
        }
    }

    #endregion

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
