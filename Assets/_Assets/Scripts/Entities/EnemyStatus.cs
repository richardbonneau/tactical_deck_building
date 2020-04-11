﻿using System.Collections;
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
    public bool currentlyDoingAnAction = false;
    public bool turnDone = false;

    public string[,] actions = new string[2, 2] {
    {"move", "2"},
    {"move","1"}
    };
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
    public void NextAction()
    {
        currentAction++;
    }
    void TurnDone()
    {
        currentAction = 0;
        turnDone = true;
        currentlyDoingAnAction = false;
        currentlyDoingTurn = false;
    }
    void MoveAction()
    {
        int moveSpeed = int.Parse(actions[currentAction,1]);
        allowedMovement = moveSpeed;
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
            if (currentlyDoingTurn)
            {
                if (currentAction > actions.GetLength(0) - 1) TurnDone();
                else if (!currentlyDoingAnAction)
                {
                    if (actions[currentAction,0] == "move") MoveAction();

                }
            }

        }
    }
}
