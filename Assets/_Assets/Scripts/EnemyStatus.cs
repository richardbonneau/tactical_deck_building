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
        int randomAnimation = Random.Range(1, 5);
        animator.SetTrigger("getHit" + randomAnimation);

    }
    void Update()
    {
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
        }

    }
}
