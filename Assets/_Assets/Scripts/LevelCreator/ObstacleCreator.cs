using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour
{
    public GridCreator gridCreator;
    public GameObject[] bases;
    public GameObject[] randomObstacles;

    List<GameObject> createdObstacles = new List<GameObject>();
    GameObject createdBase;

    Base baseScript;
    Transform obstaclesParent;
    Transform obstaclesPositionsParent;


    public void createLevel()
    {
        GameObject chosenBase = bases[Random.Range(0, bases.Length)];
        createdBase = Instantiate(chosenBase, new Vector3(0, 0, 0), Quaternion.identity);
        baseScript = createdBase.GetComponent<Base>();
        obstaclesPositionsParent = baseScript.obstaclesPositions;
        obstaclesParent = baseScript.obstacles;

        foreach (Transform obstacle in obstaclesPositionsParent)
        {
            GameObject newObstacle = Instantiate(randomObstacles[Random.Range(0, randomObstacles.Length)], new Vector3(obstacle.position.x, 0, obstacle.position.z), Quaternion.identity);
            newObstacle.transform.SetParent(obstaclesParent);
        }
        gridCreator.EnterNewRoom(new Vector3(18, 0, -10), new Vector3(10, 0, -5), 18, 10);
    }

    public void destroyLevel()
    {
        Destroy(createdBase);

    }
}
