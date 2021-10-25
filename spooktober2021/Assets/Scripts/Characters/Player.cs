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

    private Vector2 mousePos;

    private bool canShoot = true;

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

        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            FireSpell();
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
        canShoot = false;

        Spells spell = currentSelectedSpell.GetComponent<Spells>();

        TakeDamages(spell.GetStats().cost);

        StartCoroutine(SpellCooldown(spell.GetStats().cooldown));

        spell = Instantiate(currentSelectedSpell.gameObject, this.firePoint.position, this.firePoint.rotation).GetComponent<Spells>();
        spell.SetFirePoint(this.firePoint);
    }

    private IEnumerator SpellCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

    private void CameraFollow()
    {
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
    }
}
