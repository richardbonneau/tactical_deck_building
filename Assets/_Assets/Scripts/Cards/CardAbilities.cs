using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbilities : MonoBehaviour
{

    Animator playerAnimator;
    GameObject player;
    Pathfinder pathfinder;
    EnemiesManager enemiesManager;
    GridCreator gridCreator;
    public bool isMoveCard = false;
    public List<CardAction> cardActions = new List<CardAction>();
    void Awake()
    {
        GameObject gridManager = GameObject.FindWithTag("GridManager");
        pathfinder = gridManager.GetComponent<Pathfinder>();
        gridCreator = gridManager.GetComponent<GridCreator>();
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        enemiesManager = GameObject.FindWithTag("EnemiesManager").GetComponent<EnemiesManager>();
    }
    void Start()
    {
        if (isMoveCard)
        {
            cardActions.Add(new CardAction(_actionType: "move", _value: 2));
        }
        else cardActions.Add(new CardAction(_actionType: "attack", _value: 5));
    }

    public void PlayCard()
    {
        foreach (CardAction action in cardActions)
        {
            if (action.actionType == "move") EnablePlayerMove(action.value);
            else if (action.actionType == "attack") EnablePlayerAttack(action.value);
        }
    }
    public void EnablePlayerMove(int maxMove)
    {
        pathfinder.maxMove = maxMove;
        if (pathfinder.playerIsAllowedToMove == false) pathfinder.playerIsAllowedToMove = true;
        else
        {
            pathfinder.removeMovementGrid();
            pathfinder.playerIsAllowedToMove = false;
        }
    }
    public void EnablePlayerAttack(int attackAmount)
    {
        List<GameObject> enemies = enemiesManager.activeEnemies;
        List<Node> neighboursNodes = gridCreator.GetNeighbours(gridCreator.NodeFromWorldPoint(player.transform.position));
        foreach (Node node in neighboursNodes)
        {
            GameObject found = enemies.Find(enemy => enemy.transform.position.x == node.worldPosition.x && enemy.transform.position.z == node.worldPosition.z);
            if (found != null)
            {
                playerAnimator.SetTrigger("Attack");
                int randomAnimation = Random.Range(1, 5);
                found.GetComponent<Animator>().SetTrigger("getHit" + randomAnimation);
                player.transform.LookAt(found.transform.position);

                found.GetComponent<EnemyStatus>().health = found.GetComponent<EnemyStatus>().health - 5;

                return;
            }
        }
    }
}
