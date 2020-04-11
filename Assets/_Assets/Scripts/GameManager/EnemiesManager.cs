using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GridCreator gridCreator;
    public List<GameObject> activeEnemies;
    public RoundManager roundManager;
    int currentEnemyTurn = 0;
    public bool allEnemiesTurnsDone = false;
    GameObject activeEnemy;
    EnemyStatus activeEnemyStatus;

    void Start()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            Node node = gridCreator.NodeFromWorldPoint(enemy.transform.position);
            node.walkable = false;
        }
    }
    public void NextRound()
    {
        allEnemiesTurnsDone = false;
        currentEnemyTurn = 0;
        foreach (GameObject enemy in activeEnemies)
        {
            enemy.GetComponent<EnemyStatus>().NextRound();
        }
    }
    void NextEnemyTurn()
    {
        currentEnemyTurn++;
        activeEnemy = activeEnemies[currentEnemyTurn];
        activeEnemyStatus = activeEnemy.GetComponent<EnemyStatus>();
        activeEnemyStatus.currentlyDoingTurn = true;
    }
    public void BeginEnemyPhase()
    {
        currentEnemyTurn = 0;
        activeEnemy = activeEnemies[currentEnemyTurn];
        activeEnemyStatus = activeEnemy.GetComponent<EnemyStatus>();
        activeEnemyStatus.currentlyDoingTurn = true;
    }

    void Update()
    {
        print("activeEnemyStatus.turnDone     " + activeEnemyStatus.turnDone);
        if (roundManager.playerPhaseDone && !roundManager.enemiesPhaseDone && activeEnemies.Count > 0)
        {
            if (activeEnemyStatus.turnDone)
            {
                print("active enemy turn done");

                if (currentEnemyTurn + 1 > activeEnemies.Count - 1)
                {
                    print("all enemies done");
                    roundManager.NextRound();
                    return;
                }
                else NextEnemyTurn();
            }
        }
    }
}
