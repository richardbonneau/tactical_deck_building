using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    public GridCreator gridCreator;
    public GameObject[] bases;
    public PlayerStatus playerStatus;

    List<GameObject> createdObstacles = new List<GameObject>();
    GameObject createdBase;

    public void createLevel()
    {
        GameObject chosenBase = bases[Random.Range(0, bases.Length)];
        createdBase = Instantiate(chosenBase, new Vector3(0, 0, 0), Quaternion.identity);

        Base baseScript = createdBase.GetComponent<Base>();
        Transform obstaclesSpawnPoints = baseScript.obstaclesSpawnPoints;
        Transform visualObstaclesParent = baseScript.visualObstaclesParent;
        Transform enemiesSpawnPoints = baseScript.enemiesSpawnPoints;
        Transform enemiesParent = baseScript.enemiesParent;
        GameObject[] possibleRandomObstacles = baseScript.possibleRandomObstacles;
        GameObject[] possibleRandomEnemies = baseScript.possibleRandomEnemies;

        foreach (Transform obstacle in obstaclesSpawnPoints)
        {
            GameObject newObstacle = Instantiate(possibleRandomObstacles[Random.Range(0, possibleRandomObstacles.Length)], new Vector3(obstacle.position.x, 0, obstacle.position.z), Quaternion.identity);
            newObstacle.transform.SetParent(visualObstaclesParent);
        }

        foreach (Transform enemy in enemiesSpawnPoints)
        {
            GameObject newEnemy = Instantiate(possibleRandomEnemies[Random.Range(0, possibleRandomEnemies.Length)], new Vector3(enemy.position.x, 0, enemy.position.z), Quaternion.identity);
            newEnemy.transform.SetParent(enemiesParent);
        }


        Vector3 newRoomFirstNodeLocation = new Vector3(-11, 0, -9);
        gridCreator.EnterNewRoom(newRoomFirstNodeLocation, new Vector3(0, 0, 0), baseScript.size);
        playerStatus.playerNode = gridCreator.NodeFromWorldPoint(newRoomFirstNodeLocation);
    }

    public void destroyLevel()
    {
        Destroy(createdBase);

    }
}
