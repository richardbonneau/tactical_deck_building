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
    public bool currentlyDoingAnAction = false;
    public bool turnDone = false;
    public GameObject player;
    public GridCreator gridCreator;
    Node enemyNode;

    public string[,] actions = new string[3, 2] {
    {"move", "2"},
    {"move","1"},
    {"meleeAttack", "10"}
    };
    int currentAction = 0;

    void Start()
    {
        enemyPathfinder = GetComponent<EnemyPathfinder>();
        animator = GetComponent<Animator>();
        enemyNode = gridCreator.NodeFromWorldPoint(player.transform.position);
        enemyNode.walkable = false;
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
    }
    void MoveAction()
    {
        int moveSpeed = int.Parse(actions[currentAction, 1]);
        allowedMovement = moveSpeed;
        currentlyDoingAnAction = true;
        enemyPathfinder.isAllowedToMove = true;
    }
    void EmptyAction()
    {

    }
    void MeleeAttackAction()
    {
        currentlyDoingAnAction = true;
        // List<GameObject> enemies = enemiesManager.activeEnemies;
        List<Node> neighboursNodes = gridCreator.GetNeighbours(gridCreator.NodeFromWorldPoint(this.transform.position));
        foreach (Node node in neighboursNodes)
        {
            if (node.worldPosition == player.transform.position)
            {
                animator.SetTrigger("Attack");
                int randomAnimation = Random.Range(1, 5);
                player.GetComponent<Animator>().SetTrigger("getHit" + randomAnimation);
                this.transform.LookAt(player.transform.position);
                player.GetComponent<PlayerStatus>().health = player.GetComponent<PlayerStatus>().health - int.Parse(actions[currentAction, 1]);

            }
        }
        currentlyDoingAnAction = false;
        NextAction();
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

                    if (actions[currentAction, 0] == "move") MoveAction();
                    else if (actions[currentAction, 0] == "meleeAttack") MeleeAttackAction();

                }
            }

        }
    }
}
