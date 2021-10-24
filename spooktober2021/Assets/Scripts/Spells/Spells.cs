using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    [SerializeField] protected SpellsScriptable spellInfos;
    [SerializeField] protected SpellsScriptable.stats stats;

    [Header("Components")]
    [SerializeField] protected Rigidbody2D body;
    [SerializeField] protected SpriteRenderer sprite;

    protected Transform firePoint;

    #endregion

    protected void CallStart()
    {

    }

    public void SetFirePoint(Transform point)
    {
        firePoint = point;
    }
}
