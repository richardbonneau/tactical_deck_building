using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour
{
    public Transform obstaclePositionsParent;
    
    // public GameObject[] bases;
    public GameObject[] randomObstacles;
    List<GameObject> createdObstacles = new List<GameObject>();

    void Start()
    {
   
    }

    public void createLevel(){
         foreach(Transform obstacle in obstaclePositionsParent){
            GameObject newObstacle = Instantiate(randomObstacles[Random.Range(0,randomObstacles.Length)], new Vector3(obstacle.position.x,0,obstacle.position.z), Quaternion.identity);
            createdObstacles.Add(newObstacle);
            
        }
    }

    public void destroyLevel(){
         foreach(GameObject obstacle in createdObstacles){
            Destroy(obstacle);
        }
        
    }
}
