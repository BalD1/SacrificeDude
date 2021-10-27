using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinEventsHandler : MonoBehaviour
{
    public delegate void OnAttackEvent();
    public OnAttackEvent _OnAttackEvent;

    public void LaunchFireball()
    {
        _OnAttackEvent();
    }
}
