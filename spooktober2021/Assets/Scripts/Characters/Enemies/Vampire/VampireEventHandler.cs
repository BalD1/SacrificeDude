using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireEventHandler : MonoBehaviour
{
    [SerializeField] private Vampire attachedVampire;

    public void Teleportation()
    {
        attachedVampire.TeleportToCenter();
    }

    public void Attack()
    {
        attachedVampire.Attack();
    }

    public void StopAttack()
    {
        attachedVampire.StopAttack();
    }
}
