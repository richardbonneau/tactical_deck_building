using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    Vector3 gridSpawnPoint = new Vector3(-10, 0, -10);
    public int gridSizeX = 20;
    public int gridSizeZ = 20;
    Node outOfBounds = new Node(0, 0, new Vector3(999, 999, 999), false);
    public Node[,] grid;
    public List<Node> path;

    public static List<Vector2> obstaclesCoordinates = new List<Vector2>(){
new Vector2(5f,5f),
new Vector2(5f,12f),
new Vector2(10f,17f),
new Vector2(17f,16f),
};


    void Awake()
    {
        CreateGrid();
    }


    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeZ];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                bool walkable = true;
                foreach (Vector2 obstacleCoord in obstaclesCoordinates)
                {
                    if (obstacleCoord.x == x && obstacleCoord.y == z)
                    {
                        walkable = false;
                        // obstaclesCoordinates.Remove(obstacleCoord);
                    }
                }

                // if(x == 10 && z == 10 || x == 11 && z == 10 || x == 12 && z ==10) walkable = false;
                grid[x, z] = new Node(x, z, new Vector3(gridSpawnPoint.x + x, 0, gridSpawnPoint.z + z), walkable);
            }
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
        // print("path "+path);
        // Gizmos.DrawWireCube(transform.position,new Vector3(gridSizeX.x,1,gridSizeZ.y));

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
