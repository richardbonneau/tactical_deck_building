using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GridMouseManager : MonoBehaviour
{
    public Camera camera;
    public GameObject mapSelector;
    public GameObject player;
    PlayerStatus playerstatus;
    NavMeshAgent playerAgent;
    // Start is called before the first frame update
    void Start()
    {
        playerstatus = GetComponent<PlayerStatus>();
        playerAgent = player.GetComponent<NavMeshAgent>();
    }

    void Pathfinder(){
        // Nodes that we want to calculate the F cost of
        List<Vector3> openNodes = new <Vector3>();
        // Nodes that have already been evaluated
        List<Vector3> closedNodes = new <Vector3>();
        bool pathFound = false;

        openNodes.add(player.transform.position);

        while(!pathFound){
            Vector3 lowestCostNode;
            foreach(Vector3 node in openNodes){
                
            }
        }
        // foreach(Vector3 neighbour in neighbourList){

        // }
    }
    int GetDistance(Vector3 nodeA, Vector3 NodeB){
        int changingCoordinates = 0;
        if(nodeA.x == nodeB.x) changingCoordinates++;
        if(nodeA.z == nodeB.z) changingCoordinates++;
        if(changingCoordinates == 1) return 10;
        else if(changingCoordinates == 2) return 14
    }

    // Update is called once per frame
    void Update()
    {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)) {
            Vector3 snappedCoordinates =new Vector3(Mathf.Round(hit.point.x),mapSelector.transform.position.y,Mathf.Round(hit.point.z));
            Transform objectHit = hit.transform;
            mapSelector.transform.position = snappedCoordinates;
            print(snappedCoordinates);
            if (Input.GetMouseButtonDown(0)) {
                playerAgent.destination=snappedCoordinates;
                playerstatus.currentCoordinates = snappedCoordinates;
                // I need an array of destinations that the agent will go through one by one
            }
            
        } else mapSelector.transform.position = new Vector3(999,mapSelector.transform.position.y,999);
        
    }
}
