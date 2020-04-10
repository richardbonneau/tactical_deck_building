using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health = 10;
    public int attack = 2;
    public int allowedMovement = 10;
    public bool isDead = false;
    Animator animator;
    public EnemiesManager enemiesManager;
    EnemyPathfinder enemyPathfinder;
    public bool currentlyDoingTurn = false;
    bool currentlyDoingAnAction = false;
    public bool turnDone = false;
    List<string> actions = new List<string>() { "move", "attack" };
    int currentAction = 0;

    void Start()
    {
        enemyPathfinder = GetComponent<EnemyPathfinder>();
        animator = GetComponent<Animator>();
    }
    public void GetHit()
    {
        int randomAnimation = Random.Range(1, 5);
        animator.SetTrigger("getHit" + randomAnimation);
    }
    void NextAction()
    {
        currentAction++;
        // if action array is empty, end this enemys turn
        // otherwise get the next action into currentAction var
    }
    void MoveAction()
    {
        currentlyDoingAnAction = true;
        enemyPathfinder.isAllowedToMove = true;
    }
    void Update()
    {
        if (!isDead)
        {
            if (health <= 0)
            {
                isDead = true;
                animator.SetBool("isDead", true);
                enemiesManager.activeEnemies.Remove(this.gameObject);
            }
            else if (currentlyDoingTurn && !currentlyDoingAnAction)
            {
                if (actions[currentAction] == "move") MoveAction();

            }
        }
    }
}
