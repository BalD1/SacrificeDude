using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEventHandler : MonoBehaviour
{
    public delegate void OnAttackEvent();
    public OnAttackEvent _OnAttackEvent;

    public void MeleeAttack()
    {
        _OnAttackEvent();
    }
}
