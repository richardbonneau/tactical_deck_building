using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour
{
    public Transform obstaclePositionsParent;
    

    public GameObject[] randomObstacles;

    void Start()
    {

        foreach(Transform obstacle in obstaclePositionsParent){
            Instantiate(randomObstacles[Random.Range(0,randomObstacles.Length)], new Vector3(obstacle.position.x,0,obstacle.position.z), Quaternion.identity);
        }
        
        
    }


    void Update()
    {
        
    }
}
