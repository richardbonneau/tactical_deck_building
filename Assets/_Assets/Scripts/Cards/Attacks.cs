using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    public GridCreator gridCreator;
    public GameObject player;
    Animator playerAnimator;
    [System.NonSerialized] public CardAbilities originCard;
    public EnemiesManager enemiesManager;
    void Awake()
    {
        playerAnimator = player.GetComponent<Animator>();

    }
    public void FindPotentialTargets(int attackAmount)
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

                found.GetComponent<EnemyStatus>().health = found.GetComponent<EnemyStatus>().health - attackAmount;
                originCard.NextAction();
                return;

            }
        }

    }
    // public void PotentialTarget(GameObject target)
    // {

    // }
}
