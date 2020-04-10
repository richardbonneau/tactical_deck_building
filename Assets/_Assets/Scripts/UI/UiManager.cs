using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public EnemiesManager enemiesManager;
    public GameObject attackZoneObj;
    public GridCreator gridCreator;
    public RoundManager roundManager;
    public Pathfinder pathfinder;
    public GameObject player;
    Animator playerAnimator;
    public TextMeshProUGUI displayRounds;

    void Awake()
    {
        playerAnimator = player.GetComponentInChildren<Animator>();
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
                player.transform.GetChild(0).LookAt(found.transform.position);
                playerAnimator.SetTrigger("Attack");
            }

            // Instantiate(attackZoneObj, node.worldPosition, Quaternion.identity);
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


}
