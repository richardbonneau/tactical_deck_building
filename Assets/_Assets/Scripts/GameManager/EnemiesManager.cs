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
    public List<GameObject> randomLootAbilities = new List<GameObject>();
    public List<GameObject> randomLootCards = new List<GameObject>();
    List<GameObject> lootables = new List<GameObject>();
    GameObject lootToRemove;
    int lootPacer = 1;
    AudioSource audioSource;
    public AudioClip loot;

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = loot;
    }
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
            foreach (GameObject enemy in activeEnemies)
            {
                enemy.GetComponent<EnemyStatus>().HideEnemyIntent();
            }
            currentEnemyTurn = 0;
            activeEnemy = activeEnemies[currentEnemyTurn];
            activeEnemyStatus = activeEnemy.GetComponent<EnemyStatus>();
            activeEnemyStatus.currentlyDoingTurn = true;
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
        audioSource.Play();
        lootPacer++;
        if (lootPacer > 3) lootPacer = 1;
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
        GameObject randomItem;
        if (lootPacer == 3) randomItem = randomLootCards[Random.Range(0, randomLootCards.Count)];
        else randomItem = randomLootAbilities[Random.Range(0, randomLootAbilities.Count)];
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
                    print("active enemies: " + activeEnemies.Count);
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
