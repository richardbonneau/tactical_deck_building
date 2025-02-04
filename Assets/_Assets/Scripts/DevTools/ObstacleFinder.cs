﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
public class ObstacleFinder : MonoBehaviour
{
    Node[,] grid;
    GridCreator gridScript;
    int currentObstacleFinderX = 0;
    int currentObstacleFinderZ = 0;
    bool obstacleScanDone = false;
    bool hitObstacleLastUpdate = false;
    public List<Vector2> obstacleCoordinatesList = new List<Vector2>();

    bool listSaved = false;

    void Awake()
    {
        gridScript = GameObject.FindWithTag("GameManager").transform.GetChild(0).GetComponent<GridCreator>();
    }
    void Start()
    {
        grid = gridScript.grid;
    }
    void FixedUpdate()
    {
        if (!obstacleScanDone)
        {
            if (!hitObstacleLastUpdate)
            {
                if (currentObstacleFinderZ < grid.GetLength(0))
                {
                    currentObstacleFinderZ++;
                    transform.position = grid[currentObstacleFinderX, currentObstacleFinderZ].worldPosition;
                }
                else if (currentObstacleFinderX < grid.GetLength(1) - 1)
                {
                    currentObstacleFinderX++;
                    currentObstacleFinderZ = 0;
                }
                else obstacleScanDone = true;
            }
            else
            {
                hitObstacleLastUpdate = false;
            }
        }
        else if (!listSaved)
        {
            obstacleCoordinatesList = obstacleCoordinatesList.Distinct().ToList();
            string path = "Assets/_Assets/DevTools/ObstaclesList.txt";
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine("public static List<Vector2> roomX = new List<Vector2>(){");
            foreach (Vector2 coord in obstacleCoordinatesList)
            {
                writer.WriteLine("new Vector2(" + coord.x + "f," + coord.y + "f)" + ",");
            }
            writer.WriteLine("};");
            writer.Close();
            listSaved = true;
        }
    }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Obstacle"))
        {
            print("obstacle found: " + col.transform.name);
            // Node node = gridScript.NodeFromWorldPoint(transform.position);
            // node.walkable = false;
            Vector2 newCoord = new Vector2(currentObstacleFinderX, currentObstacleFinderZ);
            // if(obstacleCoordinatesList.Count > 0 && obstacleCoordinatesList[obstacleCoordinatesList.Count-1] != newCoord)
            obstacleCoordinatesList.Add(newCoord);
            transform.position = new Vector3(0, 10, 0);
            hitObstacleLastUpdate = true;
        }
        if (col.transform.CompareTag("DoorTrigger"))
        {
            print("obstacle found: " + col.transform.name);
            // Node node = gridScript.NodeFromWorldPoint(transform.position);
            // node.walkable = false;
            Vector2 newCoord = new Vector2(currentObstacleFinderX, currentObstacleFinderZ);
            // if(obstacleCoordinatesList.Count > 0 && obstacleCoordinatesList[obstacleCoordinatesList.Count-1] != newCoord)
            obstacleCoordinatesList.Add(newCoord);
            transform.position = new Vector3(0, 10, 0);
            hitObstacleLastUpdate = true;
        }
    }
}
