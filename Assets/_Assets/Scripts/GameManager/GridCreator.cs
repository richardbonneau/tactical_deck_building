using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    Vector3 gridSpawnPoint = new Vector3(-10,0,-10);
    public int gridSizeX = 20;
    public int gridSizeZ = 20;
    Node[,] grid;

    void Awake() {
        CreateGrid();
    }

    void CreateGrid(){
        grid = new Node[gridSizeX+1, gridSizeZ+1];

        for (int x= 0; x<=gridSizeX; x++){
            for(int z=0;z<=gridSizeZ; z++){
                // TODO: bool walkable = something something
                bool walkable = true;
                grid[x,z] = new Node(x,z,new Vector3(gridSpawnPoint.x + x,0,gridSpawnPoint.z + z),walkable);  
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition){
        int x = Mathf.RoundToInt(worldPosition.x - gridSpawnPoint.x);
        int z = Mathf.RoundToInt(worldPosition.z - gridSpawnPoint.z);
        return grid[x,z];
    }

    public List<Node> GetNeighbours(Node node){
    List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++) {
	        for (int z = -1; z <= 1; z++) {
		    if (x == 0 && z == 0) continue;
                int checkX = node.x + x;
                int checkZ = node.z + z;
                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ) {
                    neighbours.Add(grid[checkX,checkZ]);
				}
            }
        }
        return neighbours;
    }
}
