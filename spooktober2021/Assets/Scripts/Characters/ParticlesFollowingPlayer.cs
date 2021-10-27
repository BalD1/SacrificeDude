using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFollowingPlayer : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    void Update()
    {
        this.transform.position = player.transform.position;
    }
}
