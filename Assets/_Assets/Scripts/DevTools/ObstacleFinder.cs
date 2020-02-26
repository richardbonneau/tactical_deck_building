using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ObstacleFinder : MonoBehaviour
{
    Node[,] grid;
    GridCreator gridScript;
    int currentObstacleFinderX = 0;
    int currentObstacleFinderZ = 0;
    bool obstacleScanDone = false;
    bool hitObstacleLastUpdate = false;
    public List<Vector2> obstacleCoordinatesList = new List<Vector2>();

    void Awake(){
        gridScript = GameObject.FindWithTag("GameManager").transform.GetChild(0).GetComponent<GridCreator>();
        
    }
    void Start(){
        grid = gridScript.grid;
        print("grid:"+gridScript);
    }
    void FixedUpdate(){
   
       if(!obstacleScanDone) {
       if(!hitObstacleLastUpdate){
           
        //    print("grid.GetLength(1) "+grid.GetLength(0)+"currentObstacleFinderZ "+currentObstacleFinderZ+" currentObstacleFinderX"+currentObstacleFinderX);
        if(currentObstacleFinderZ < grid.GetLength(0)){
            transform.position = grid[currentObstacleFinderX,currentObstacleFinderZ].worldPosition;
            // Instantiate(test, grid[currentObstacleFinderX,currentObstacleFinderZ].worldPosition, Quaternion.identity);
            currentObstacleFinderZ++;
            } else if(currentObstacleFinderX < grid.GetLength(1)-1){
                currentObstacleFinderX++;
                currentObstacleFinderZ=0;
            } else obstacleScanDone = true;
        } else {
            print("obstacle else");
            
            hitObstacleLastUpdate = false;
            }
       
    } else{
        print("scan done else");
obstacleCoordinatesList = obstacleCoordinatesList.Distinct().ToList();
    }
    }
  
    
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col){
        if(col.transform.CompareTag("Obstacle")){
            print("obstacle found: "+col.transform.name);
            // Node node = gridScript.NodeFromWorldPoint(transform.position);
            // node.walkable = false;
            Vector2 newCoord = new Vector2(currentObstacleFinderX,currentObstacleFinderZ);
            // if(obstacleCoordinatesList.Count > 0 && obstacleCoordinatesList[obstacleCoordinatesList.Count-1] != newCoord)
            obstacleCoordinatesList.Add(newCoord);
            transform.position = new Vector3(0,10,0);
            hitObstacleLastUpdate = true;
        }
    }
}
