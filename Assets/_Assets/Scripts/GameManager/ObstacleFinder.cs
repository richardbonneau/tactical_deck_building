using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFinder : MonoBehaviour
{
    Node[,] grid;
    GridCreator gridScript;
    public int currentObstacleFinderX = 0;
    public int currentObstacleFinderZ = 0;
    public GameObject test;
    bool obstacleScanDone = false;
    bool hitObstacleLastUpdate = false;

    void Awake(){
        gridScript = GameObject.FindWithTag("GameManager").transform.GetChild(0).GetComponent<GridCreator>();
        grid = gridScript.grid;
    }
    void FixedUpdate(){
   
       if(!obstacleScanDone && !hitObstacleLastUpdate){
           
           print("grid.GetLength(1) "+grid.GetLength(0)+"currentObstacleFinderZ "+currentObstacleFinderZ+" currentObstacleFinderX"+currentObstacleFinderX);
        if(currentObstacleFinderZ < grid.GetLength(0)){
            transform.position = grid[currentObstacleFinderX,currentObstacleFinderZ].worldPosition;
            // Instantiate(test, grid[currentObstacleFinderX,currentObstacleFinderZ].worldPosition, Quaternion.identity);
            currentObstacleFinderZ++;
            } else if(currentObstacleFinderX < grid.GetLength(1)-1){
                currentObstacleFinderX++;
                currentObstacleFinderZ=0;
            } else obstacleScanDone = true;
       } else hitObstacleLastUpdate = false;
       
    } 
  
    
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col){
        if(col.transform.CompareTag("Obstacle")){
            print("obstacle found: "+col.transform.name);
            Node node = gridScript.NodeFromWorldPoint(transform.position);
            node.walkable = false;
            transform.position = new Vector3(0,10,0);
            hitObstacleLastUpdate = true;
        }
    }
}
