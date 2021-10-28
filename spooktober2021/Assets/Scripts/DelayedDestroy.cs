using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    [SerializeField] private float delay;

    private void Start()
    {
        Destroy(this.gameObject, delay);
    }
}
