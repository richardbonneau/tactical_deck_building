using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public Transform obstaclesSpawnPoints;
    public Transform enemiesSpawnPoints;
    public Transform visualObstaclesParent;
    public Transform enemiesParent;
    public GameObject[] possibleRandomObstacles;
    public GameObject[] possibleRandomEnemies;
    public Vector2 size;
}
