using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Material closedMaterial;
    [SerializeField] private GameObject redLight;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private CircleCollider2D trigger;
    private Material baseMateriel;

    public enum State
    {
        closed,
        open,
    }
    private State state;
    public State ShopState
    {
        get => state;
        set
        {
            state = value;

            bool enableComponents = true;
            switch (value)
            {
                case State.closed:
                    enableComponents = false;
                    shopWindow.SetActive(false);
                    sprite.material = closedMaterial;
                    break;

                case State.open:
                    enableComponents = true;
                    sprite.material = baseMateriel;
                    break;
            }

            trigger.enabled = enableComponents; 
            animator.enabled = enableComponents;
            trigger.enabled = enableComponents;
            redLight.SetActive(enableComponents);
        }
    }

    private void Start()
    {
        baseMateriel = sprite.material;
        ShopState = State.open;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameManager.GameStates.InCinematic)
            animator.SetTrigger("bossSpawn");

        if (!GameManager.Instance.IsInWave && state == State.closed)
            ShopState = State.open;
        else if (GameManager.Instance.IsInWave && state == State.open)
            ShopState = State.closed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.open)
        {
            if (collision.CompareTag("Player"))
            {
                shopWindow.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state == State.open)
        {
            if (collision.CompareTag("Player"))
            {
                shopWindow.SetActive(false);
            }
        }
    }
}
