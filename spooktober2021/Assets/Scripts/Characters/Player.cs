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

    private void Awake()
    {
        stats = characterInfos.CharacterStats;
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
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState == GameManager.GameStates.InGame)
        {
            Movements();
            RotateByMousePosition();
        }
    }

    private void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            FireSpell();
        }
    }

    private void RotateByMousePosition()
    {
        Vector2 lookDirection = mousePos - body.position;
        Debug.Log(mousePos);
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        body.rotation = angle;  
    }

    private void FireSpell()
    {
        Spells spell = Instantiate(currentSelectedSpell.gameObject, this.firePoint.position, this.firePoint.rotation).GetComponent<Spells>();
        spell.SetFirePoint(this.firePoint);
    }
}
