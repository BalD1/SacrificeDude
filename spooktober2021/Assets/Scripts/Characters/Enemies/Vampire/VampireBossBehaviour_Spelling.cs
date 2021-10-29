using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBossBehaviour_Spelling : StateMachineBehaviour
{

    [SerializeField] private GameObject soul;
    [SerializeField] private Vector2 soulsSpawnPos;

    [SerializeField] private float attackTime;
    [SerializeField] private float minTimeBetweenSpells;
    [SerializeField] private float maxTimeBetweenSpells;
    [SerializeField] private GameObject spellToLaunch;
    private float attackTimer;
    private float spellTimer;
    private Coroutine coroutine;

    private Vampire vampire;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTimer = attackTime;
        spellTimer = Random.Range(minTimeBetweenSpells, maxTimeBetweenSpells);
        vampire = GameManager.Instance.vampireBoss.GetComponentInChildren<Vampire>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTimer -= Time.deltaTime;
        spellTimer -= Time.deltaTime;
        if (attackTimer <= 0)
            animator.SetBool("SpellAttack", false);
        else
        {
            if (spellTimer <= 0)
                LaunchSpell();
        }


    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(soul, soulsSpawnPos, Quaternion.identity);
        Instantiate(soul, soulsSpawnPos, Quaternion.identity);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void LaunchSpell()
    {
        foreach(Transform point in vampire.spellPoints)
        {
            EnemyFireball fireball = Instantiate(spellToLaunch, point.position, point.rotation).GetComponent<EnemyFireball>();
            fireball.Shoot(point, vampire.GetStats().damages);
        }
        spellTimer = Random.Range(minTimeBetweenSpells, maxTimeBetweenSpells);
    }
}
