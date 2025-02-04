﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public Vector3 gridSpawnPoint = new Vector3(-10, 0, -10);
    public int gridSizeX = 0;
    public int gridSizeZ = 0;
    public GameObject showNodePosition;
    public Node[,] grid;
    public List<Node> path;
    public EnemiesManager enemiesManager;
    public GameObject player;
    public List<GameObject> rooms = new List<GameObject>();
    int roomIndex = 0;
    List<GameObject> currentRoomDoorTriggers = new List<GameObject>();
    public static List<Vector2> obstaclesCoordinates = new List<Vector2>();
    public UiManager uiManager;
    public Menus menus;

    public void EnterNewRoom(Vector3 nextRoomGridStart, Vector3 newPlayerPositionInNewRoom, Vector2 roomSize)
    {
        print("Enter New Room " + roomSize);

        // PROTOTYPE-CODE:
        // if (doorTriggerNode.nextRoomIndex == 5)
        // {
        //     menus.WinState();
        // }
        // else
        // {
        gridSpawnPoint = nextRoomGridStart;
        gridSizeX = Mathf.RoundToInt(roomSize.x);
        gridSizeZ = Mathf.RoundToInt(roomSize.y);
        player.transform.position = newPlayerPositionInNewRoom;
        enemiesManager.FindAllActiveEnemies();

        // PROTOTYPE-CODE:
        // clear the list of active enemies
        // enemiesManager.ClearAllEnemies();
        // rooms[doorTriggerNode.nextRoomIndex].SetActive(true);
        // rooms[roomIndex].SetActive(false);
        // roomIndex = doorTriggerNode.nextRoomIndex;
        // // activate all new enemies
        // /// enemiesManager.FindAllActiveEnemies();

        CreateGrid();
        uiManager.CenterPlayer();
    }
    public void EnableDoorTriggers()
    {
        foreach (GameObject trigger in currentRoomDoorTriggers)
        {
            trigger.SetActive(true);
        }
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 endPos, bool isEnemy)
    {
        // Nodes that we want to calculate the F cost of
        List<Node> openNodes = new List<Node>();
        // Nodes that have already been evaluated
        List<Node> closedNodes = new List<Node>();

        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(endPos);
        openNodes.Add(startNode);
        while (targetNode != null && openNodes.Count > 0)
        {
            Node lowestCostNode = openNodes[0];
            foreach (Node node in openNodes)
            {
                if (node.fCost <= lowestCostNode.fCost && node.hCost < lowestCostNode.hCost)
                {
                    lowestCostNode = node;
                }
            }
            openNodes.Remove(lowestCostNode);
            closedNodes.Add(lowestCostNode);
            if (lowestCostNode == targetNode)
            {

                List<Node> path = new List<Node>();
                Node currentNode = targetNode;
                while (currentNode != startNode)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                return path;
            }

            List<Node> neighbours = GetNeighbours(lowestCostNode);
            foreach (Node neighbour in neighbours)
            {

                if (!neighbour.walkable || closedNodes.Contains(neighbour)) continue;
                else if (isEnemy && neighbour.lootable) continue;

                int newCostToNeighbour = lowestCostNode.gCost + GetDistance(lowestCostNode, neighbour);
                bool openNodesListContainsNeighbour = openNodes.Contains(neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openNodesListContainsNeighbour)
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = lowestCostNode;
                    if (!openNodesListContainsNeighbour)
                    {
                        openNodes.Add(neighbour);
                    }
                }
            }
        }

        return new List<Node>();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstZ = Mathf.Abs(nodeA.z - nodeB.z);

        if (dstX > dstZ)
            return 14 * dstZ + 10 * (dstX - dstZ);
        return 14 * dstX + 10 * (dstZ - dstX);
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeZ];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                bool walkable = true;
                Vector3 nodePosition = new Vector3(gridSpawnPoint.x + x, 0, gridSpawnPoint.z + z);

                //DEBUG: Creates a cube wherever a node is spawned.
                // Instantiate(showNodePosition, nodePosition, Quaternion.identity);

                grid[x, z] = new Node(x, z, new Vector3(gridSpawnPoint.x + x, 0, gridSpawnPoint.z + z), walkable);
            }
        }
        currentRoomDoorTriggers.Clear();
        GameObject[] doorTriggers = GameObject.FindGameObjectsWithTag("DoorTrigger");
        foreach (GameObject trigger in doorTriggers)
        {
            trigger.SetActive(false);
            currentRoomDoorTriggers.Add(trigger);
        }
    }


    public void ResetAllNodeCosts()
    {
        foreach (Node node in grid)
        {
            node.Reset();
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x - gridSpawnPoint.x);
        int z = Mathf.RoundToInt(worldPosition.z - gridSpawnPoint.z);
        if (x < 0 || z < 0 || x > grid.GetLength(0) - 1 || z > grid.GetLength(1) - 1)
        {
            return null;
        };
        return grid[x, z];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                int checkX = node.x + x;
                int checkZ = node.z + z;
                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbours.Add(grid[checkX, checkZ]);
                }
            }
        }
        return neighbours;
    }

    void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (1000 - .1f));
            }
        }
    }
}
