using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireDeath : MonoBehaviour
{
    [SerializeField] private GameObject relatedVampire;
    [SerializeField] private Transform relatedVampireTransform;
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator.enabled = true;
        GameManager.Instance.GameState = GameManager.GameStates.InCinematic;
        GameManager.Instance.SetMainCameraPosition(relatedVampireTransform.position);
        float animTime = GameManager.Instance.GetAnimationLength(animator, "VampireDeathAnim");
        StartCoroutine(WaitBeforeWin(animTime));
    }

    private IEnumerator WaitBeforeWin(float time)
    {
        yield return new WaitForSeconds(time);

        GameManager.Instance.GameState = GameManager.GameStates.Win;
        Destroy(relatedVampire);
    }
}
