using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GridCreator gridCreator;
    public List<GameObject> activeEnemies = new List<GameObject>();
    public RoundManager roundManager;
    int currentEnemyTurn = 0;
    public bool allEnemiesTurnsDone = false;
    GameObject activeEnemy;
    EnemyStatus activeEnemyStatus;


    void Start()
    {
        GameObject[] allEnemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemiesInScene)
        {
            activeEnemies.Add(enemy);
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
        if (roundManager.playerPhaseDone && !roundManager.enemiesPhaseDone && activeEnemies.Count > 0)
        {
            if (activeEnemyStatus.turnDone)
            {
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
