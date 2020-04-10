using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health = 10;
    public int allowedMovement = 10;
    public bool isDead = false;
    Animator animator;
    public EnemiesManager enemiesManager;
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
        if (!isDead && health <= 0)
        {
            print(enemiesManager.activeEnemies.Count);
            isDead = true;
            animator.SetBool("isDead", true);
            enemiesManager.activeEnemies.Remove(this.gameObject);
            print(enemiesManager.activeEnemies.Count);
        }

    }
}
