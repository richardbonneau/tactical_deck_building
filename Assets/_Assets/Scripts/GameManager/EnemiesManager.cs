using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GridCreator gridCreator;
    public List<GameObject> activeEnemies;


    void Start()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            Node node = gridCreator.NodeFromWorldPoint(enemy.transform.position);
            node.walkable = false;
        }
    }
}
