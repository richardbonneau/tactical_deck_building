using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public Vector3 gridSpawnPoint = new Vector3(0,0,0);
    public int gridSizeX;
    public int gridSizeY;

    Node[,] grid;

    void Awake() {
        CreateGrid();
        }
    void CreateGrid(){
        grid = new Node[gridSizeX+1, gridSizeY+1];
        int halfGridSizeX = gridSizeX / 2;
        int halfGridSizeY = gridSizeY / 2;
        print(halfGridSizeX+" "+halfGridSizeY);
        for (int x= 0; x<=gridSizeX; x++){
            for(int y=0;y<=gridSizeY; y++){
                grid[x,y] = new Node(x-halfGridSizeX,y-halfGridSizeY,new Vector3(x-halfGridSizeX,0,y-halfGridSizeY),true);
                print(grid[x,y].x);
            }
        }
    }
}
