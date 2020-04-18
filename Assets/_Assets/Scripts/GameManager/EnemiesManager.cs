using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GameObject inventory;
    public GridCreator gridCreator;
    public List<GameObject> activeEnemies = new List<GameObject>();
    public RoundManager roundManager;
    int currentEnemyTurn = 0;
    public bool allEnemiesTurnsDone = false;
    public UiManager uiManager;
    GameObject activeEnemy;
    EnemyStatus activeEnemyStatus;
    public GameObject lootableVisual;
    public List<GameObject> randomLootItems = new List<GameObject>();
    List<GameObject> lootables = new List<GameObject>();
    GameObject lootToRemove;

    void Start()
    {
        FindAllActiveEnemies();
    }
    public void FindAllActiveEnemies()
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
    IEnumerator WaitAndNextRound()
    {
        yield return new WaitForSeconds(1f);
        roundManager.NextRound();
    }
    public void BeginEnemyPhase()
    {
        if (activeEnemies.Count > 0)
        {
            currentEnemyTurn = 0;
            activeEnemy = activeEnemies[currentEnemyTurn];
            activeEnemyStatus = activeEnemy.GetComponent<EnemyStatus>();
            activeEnemyStatus.currentlyDoingTurn = true;
            activeEnemyStatus.HideEnemyIntent();
        }
        else
        {
            StartCoroutine(WaitAndNextRound());
        }
    }
    public void NewLootable(Vector3 position)
    {
        GameObject newLootable = Instantiate(lootableVisual, position, Quaternion.identity);
        lootables.Add(newLootable);
        print("WALKABLE?: " + gridCreator.NodeFromWorldPoint(position).walkable);
    }

    public void LootablePickedUp(Vector3 position)
    {
        print("PICKING UP LOOT" + lootables.Count);
        foreach (GameObject loot in lootables)
        {
            if (loot.transform.position == position)
            {
                lootToRemove = loot;
                break;
            }
        }

        lootables.Remove(lootToRemove);
        Destroy(lootToRemove);
        lootToRemove = null;
        GameObject randomItem = randomLootItems[Random.Range(0, randomLootItems.Count)];
        GameObject lootToAdd = Instantiate(randomItem, new Vector3(0, 0, 0), Quaternion.identity);
        lootToAdd.transform.SetParent(inventory.transform, false);
        uiManager.DisplayLootedMessage(lootToAdd.tag + " added to Inventory!");
        print("LOOT picked up" + lootables.Count);
    }

    public void ClearAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            Destroy(enemy);
        }
        activeEnemies.Clear();
    }

    void Update()
    {
        if (roundManager.playerPhaseDone && !roundManager.enemiesPhaseDone && activeEnemies.Count > 0)
        {
            if (activeEnemyStatus.turnDone)
            {
                if (currentEnemyTurn + 1 > activeEnemies.Count - 1)
                {
                    foreach (GameObject enemy in activeEnemies)
                    {
                        enemy.GetComponent<EnemyStatus>().ShowEnemyIntent();
                    }
                    roundManager.NextRound();
                    return;
                }
                else NextEnemyTurn();
            }
        }
        else if (activeEnemies.Count < 1) gridCreator.EnableDoorTriggers();
    }
}
