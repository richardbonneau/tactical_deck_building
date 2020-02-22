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

    void Pathfinder(Vector3 startPos,Vector3 endPos){
    
        // Nodes that we want to calculate the F cost of
        List<Node> openNodes = new List<Node>();
        // Nodes that have already been evaluated
        List<Node> closedNodes = new List<Node>();
        bool pathFound = false;
        
        // Node playerNode = new Node(startPos.x,startPos.z);
        // Node targetNode = new Node(endPos.x,endPos.z);
        
        // List<Node> neighbours = GetNeighbours(playerNode);



        // openNodes.Add(playerNode);
        // while(!pathFound){
        //     Node lowestCostNode = playerNode;
        //     foreach(Node node in openNodes){
        //         if(lowestCostNode.fCost < node.fCost) {
        //             lowestCostNode = node;
        //             closedNodes.Remove(node);
        //             openNodes.Add(node);
        //             }
        //     }

        // }
        // foreach(Vector3 neighbour in neighbourList){
        // }
    }

	// int GetDistance(Node nodeA, Node nodeB) {
	// 	int dstX = Mathf.Abs(nodeA.x - nodeB.x);
	// 	int dstY = Mathf.Abs(nodeA.y - nodeB.y);

	// 	if (dstX > dstY)
	// 		return 14*dstY + 10* (dstX-dstY);
	// 	return 14*dstX + 10 * (dstY-dstX);
	// }

    // public List<Node> GetNeighbours(Node node){
    //     List<Node> neighbours = new List<Node>();

    //     for (int x = -1; x <= 1; x++) {
	// 	    for (int y = -1; y <= 1; y++) {
	// 		if (x == 0 && y == 0) continue;
    //         }
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)) {
            Vector3 snappedCoordinates =new Vector3(Mathf.Round(hit.point.x),mapSelector.transform.position.y,Mathf.Round(hit.point.z));
            Transform objectHit = hit.transform;
            mapSelector.transform.position = snappedCoordinates;
            // print(snappedCoordinates);
            if (Input.GetMouseButtonDown(0)) {
                Pathfinder(player.transform.position, snappedCoordinates);

                // playerAgent.destination=snappedCoordinates;
                // playerstatus.currentCoordinates = snappedCoordinates;
                // I need an array of destinations that the agent will go through one by one
            }
            
        } else mapSelector.transform.position = new Vector3(999,mapSelector.transform.position.y,999);
        
    }
}






// Off combat Movements 

    // void Update()
    // {
    //         RaycastHit hit;
    //         Ray ray = camera.ScreenPointToRay(Input.mousePosition);
    //         if(Physics.Raycast(ray, out hit)) {
    //         Vector3 snappedCoordinates =new Vector3(Mathf.Round(hit.point.x),mapSelector.transform.position.y,Mathf.Round(hit.point.z));
    //         Transform objectHit = hit.transform;
    //         mapSelector.transform.position = snappedCoordinates;
    //         print(snappedCoordinates);
    //         if (Input.GetMouseButtonDown(0)) {
    //             playerAgent.destination=snappedCoordinates;
    //             playerstatus.currentCoordinates = snappedCoordinates;
    //             // I need an array of destinations that the agent will go through one by one
    //         }
            
    //     } else mapSelector.transform.position = new Vector3(999,mapSelector.transform.position.y,999);
        
    // }
