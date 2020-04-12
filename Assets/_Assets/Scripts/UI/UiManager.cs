using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public GameObject deckUI;
    RectTransform rect;
    bool deckToggled = true;
    public GameObject debugCanvas;
    bool debugCanvasIsActive = false;
    public EnemiesManager enemiesManager;
    public GameObject singleEnemy;
    public GameObject attackZoneObj;
    public GridCreator gridCreator;
    public RoundManager roundManager;
    public CardsManager cardsManager;
    public Pathfinder pathfinder;
    public GameObject player;
    Animator playerAnimator;
    public TextMeshProUGUI displayRounds;

    void Awake()
    {
        rect = deckUI.GetComponent<RectTransform>();
        playerAnimator = player.GetComponentInChildren<Animator>();
    }
    void Start()
    {

    }
    void Update()
    {

        if (!roundManager.playerPhaseDone && !roundManager.enemiesPhaseDone)
        {

            debugCanvas.SetActive(true);
        }
        else
        {

            debugCanvas.SetActive(false);
        }


    }
    public void EnablePlayerMove()
    {
        if (pathfinder.playerIsAllowedToMove == false) pathfinder.playerIsAllowedToMove = true;
        else
        {
            pathfinder.removeMovementGrid();
            pathfinder.playerIsAllowedToMove = false;
        }
    }
    public void EnablePlayerAttack()
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
    public void ChangeRoundOnTheUI()
    {
        displayRounds.text = roundManager.currentRound.ToString();
    }
    public void NextRound()
    {
        roundManager.NextRound();
    }

    public void singleEnemyMoveToPlayer()
    {
        EnemyPathfinder enemyPathfinder = singleEnemy.GetComponent<EnemyPathfinder>();
        if (!enemyPathfinder.isAllowedToMove) enemyPathfinder.isAllowedToMove = true;
        else
        {
            enemyPathfinder.removeMovementGrid();
            enemyPathfinder.isAllowedToMove = false;
        }
    }
    public void EndTurn()
    {
        roundManager.playerPhaseDone = true;
        enemiesManager.BeginEnemyPhase();
    }


    public void ToggleDeck()
    {
        cardsManager.ToggleDeck();
    }


}
