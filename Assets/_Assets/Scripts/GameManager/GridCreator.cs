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
        print(gridSpawnPoint+"  worldPosition.x "+worldPosition.x+" - gridSpawnPoint.x "+gridSpawnPoint.x);
        int x = Mathf.RoundToInt(worldPosition.x - gridSpawnPoint.x);
        print(gridSpawnPoint+"  worldPosition.z "+worldPosition.z+" - gridSpawnPoint.x "+gridSpawnPoint.z);
        int z = Mathf.RoundToInt(worldPosition.z - gridSpawnPoint.z);
        print("nodeworldpoint x  "+x +" z  "+ z);
        return grid[x,z];
    }
}
