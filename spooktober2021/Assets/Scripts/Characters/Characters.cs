using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    [SerializeField] protected CharactersScriptable characterInfos;
    [SerializeField] protected CharactersScriptable.stats stats;

    [Header("Components")]
    [SerializeField] protected Rigidbody2D body;
    [SerializeField] protected SpriteRenderer sprite;

    protected Vector2 moveDirection;
    protected float moveX, moveY;

    #endregion

    protected void CallStart()
    {

    }

    protected void Movements()
    {
        moveDirection = new Vector2(moveX, moveY).normalized;
        body.MovePosition(body.position + moveDirection * stats.speed * Time.fixedDeltaTime);
    }
}
