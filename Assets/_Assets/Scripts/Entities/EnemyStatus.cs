﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health = 10;
    public int allowedMovement = 10;
    public bool isDead = false;
    Animator animator;
    EnemiesManager enemiesManager;
    EnemyPathfinder enemyPathfinder;
    public bool currentlyDoingTurn = false;
    public bool currentlyDoingAnAction = false;
    public bool turnDone = false;
    GameObject player;
    GridCreator gridCreator;
    public Node enemyNode;
    public int enemyType = 1;

    public GameObject moveIntent;
    public GameObject moveAndAttackIntent;
    public GameObject attackIntent;


    public string[,] strongMoveAndAttack = new string[2, 2] {
        {"move", "3"},
        {"meleeAttack", "4"}
    };
    public string[,] weakMoveAndAttack = new string[2, 2] {
        {"move", "3"},
        {"meleeAttack", "2"}
    };
    public string[,] oneMoveAndBigAttack = new string[2, 2] {
        {"move", "1"},
        {"meleeAttack", "5"}
    };
    public string[,] bigMove = new string[1, 2]{
        {"move","5"}
    };
    public string[,] normalMove = new string[1, 2]{
        {"move","4"}
    };
    public string[,] normalAttack = new string[1, 2]{
        {"meleeAttack","2"}
    };
    public string[,] bigAttack = new string[1, 2]{
        {"meleeAttack","5"}
    };

    List<string[,]> listOfActions = new List<string[,]>();

    int currentAction = 0;
    int actionType = 0;


    void Awake()
    {
        enemiesManager = GameObject.FindWithTag("EnemiesManager").GetComponent<EnemiesManager>();
        gridCreator = GameObject.FindWithTag("GridManager").GetComponent<GridCreator>();
        player = GameObject.FindWithTag("Player");

        // Enemy Type 1 : Robot
        if (enemyType == 1)
        {
            listOfActions.Add(weakMoveAndAttack);
            listOfActions.Add(weakMoveAndAttack);
            listOfActions.Add(weakMoveAndAttack);
            listOfActions.Add(normalAttack);
            listOfActions.Add(normalMove);


        }
        // Enemy Type 2 : Human
        else if (enemyType == 2)
        {
            health = 16;
            listOfActions.Add(strongMoveAndAttack);
            listOfActions.Add(strongMoveAndAttack);
            listOfActions.Add(oneMoveAndBigAttack);
            listOfActions.Add(oneMoveAndBigAttack);
            listOfActions.Add(bigAttack);
            listOfActions.Add(bigMove);
            // actionType = Random
            // select model
        }
        actionType = Random.Range(0, listOfActions.Count);
        ShowEnemyIntent();

    }

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyPathfinder = this.GetComponent<EnemyPathfinder>();
        enemyNode = enemyPathfinder.enemyNode;
        print("action " + this.gameObject.name);

        print(listOfActions[actionType][0, 0]);
        if (listOfActions[actionType].GetLength(0) > 1) print(listOfActions[actionType][1, 0]);
    }
    void Update()
    {
        if (!isDead)
        {
            if (health <= 0) EnemyDied();
            if (currentlyDoingTurn)
            {
                if (currentAction > listOfActions[actionType].GetLength(0) - 1) TurnDone();
                else if (!currentlyDoingAnAction)
                {
                    if (listOfActions[actionType][currentAction, 0] == "move") MoveAction();
                    else if (listOfActions[actionType][currentAction, 0] == "meleeAttack") MeleeAttackAction();
                }
            }
        }
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
    public void NextRound()
    {
        turnDone = false;
    }
    void TurnDone()
    {

        currentAction = 0;
        turnDone = true;
        currentlyDoingAnAction = false;
        currentlyDoingTurn = false;
        actionType = Random.Range(0, listOfActions.Count);

    }
    void MoveAction()
    {
        int moveSpeed = int.Parse(listOfActions[actionType][currentAction, 1]);
        allowedMovement = moveSpeed;
        currentlyDoingAnAction = true;
        enemyPathfinder.isAllowedToMove = true;
    }

    void MeleeAttackAction()
    {
        currentlyDoingAnAction = true;
        List<Node> neighboursNodes = gridCreator.GetNeighbours(gridCreator.NodeFromWorldPoint(this.transform.position));
        foreach (Node node in neighboursNodes)
        {
            if (node.worldPosition == player.transform.position)
            {
                animator.SetTrigger("Attack");
                int randomAnimation = Random.Range(1, 5);
                player.GetComponent<Animator>().SetTrigger("getHit" + randomAnimation);
                this.transform.LookAt(player.transform.position);
                player.GetComponent<PlayerStatus>().GetHit(int.Parse(listOfActions[actionType][currentAction, 1]));

            }
        }
        currentlyDoingAnAction = false;
        NextAction();
    }
    void EnemyDied()
    {
        HideEnemyIntent();
        isDead = true;
        animator.SetBool("isDead", true);
        enemiesManager.activeEnemies.Remove(this.gameObject);
        enemyNode.walkable = true;
        enemyNode.lootable = true;
        enemiesManager.NewLootable(this.transform.position);
    }
    public void ShowEnemyIntent()
    {
        if (listOfActions[actionType].GetLength(0) > 1)
        {
            moveAndAttackIntent.SetActive(true);
        }
        else if (listOfActions[actionType][0, 0] == "move")
        {
            moveIntent.SetActive(true);
        }
        else if (listOfActions[actionType][0, 0] == "meleeAttack")
        {
            attackIntent.SetActive(true);
        }
    }
    public void HideEnemyIntent()
    {
        moveAndAttackIntent.SetActive(false);
        moveIntent.SetActive(false);
        attackIntent.SetActive(false);
    }

}
