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

    Base baseScript;
    Transform visualObstaclesParent;
    Transform obstaclesSpawnPoints;


    public void createLevel()
    {
        GameObject chosenBase = bases[Random.Range(0, bases.Length)];
        createdBase = Instantiate(chosenBase, new Vector3(0, 0, 0), Quaternion.identity);
        baseScript = createdBase.GetComponent<Base>();
        obstaclesSpawnPoints = baseScript.obstaclesSpawnPoints;
        visualObstaclesParent = baseScript.visualObstaclesParent;

        foreach (Transform obstacle in obstaclesSpawnPoints)
        {
            GameObject newObstacle = Instantiate(baseScript.possibleRandomObstacles[Random.Range(0, baseScript.possibleRandomObstacles.Length)], new Vector3(obstacle.position.x, 0, obstacle.position.z), Quaternion.identity);
            newObstacle.transform.SetParent(visualObstaclesParent);
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
