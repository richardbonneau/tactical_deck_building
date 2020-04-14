﻿using System.Collections;
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
    public GameObject lootableVisual;
    List<GameObject> lootables = new List<GameObject>();
    GameObject lootToRemove;

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
    public void NewLootable(Vector3 position)
    {
        GameObject newLootable = Instantiate(lootableVisual, position, Quaternion.identity);
        lootables.Add(newLootable);
    }

    public void LootablePickedUp(Vector3 position)
    {
        print("PICKING UP LOOT" + lootables.Count);
        foreach (GameObject loot in lootables)
        {
            if (loot.transform.position == position) lootToRemove = loot;
            break;
        }

        lootables.Remove(lootToRemove);
        Destroy(lootToRemove);
        lootToRemove = null;
        print("LOOT picked up" + lootables.Count);
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
