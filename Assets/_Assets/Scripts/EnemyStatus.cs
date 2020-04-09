using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health = 10;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void GetHit()
    {
        animator.SetTrigger("getHit2");
        // int randomAttack = Random.Range(1, 4); playerAttackAnim.SetTrigger("Attack"); playerAttackAnim.SetInteger("randomAttack", randomAttack);
    }
    void Update()
    {
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
        }

    }
}
