using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Pathfinder : MonoBehaviour
{
    public Camera camera;
    public GameObject mapSelector;
    public GameObject player;
    PlayerStatus playerstatus;
    NavMeshAgent playerAgent;
    GridCreator grid;
    

    void Awake(){
        grid = GetComponent<GridCreator>();
    }
    void Start()
    {
        playerstatus = GetComponent<PlayerStatus>();
        playerAgent = player.GetComponent<NavMeshAgent>();
        
    }

    void FindPath(Vector3 startPos,Vector3 endPos){
        print("find path method");
    
        // Nodes that we want to calculate the F cost of
        List<Node> openNodes = new List<Node>();
        // Nodes that have already been evaluated
        List<Node> closedNodes = new List<Node>();
    
        
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(endPos);
        bool test = true;
        openNodes.Add(startNode);
        while(openNodes.Count > 0){
            Node lowestCostNode = openNodes[0];
            foreach(Node node in openNodes){
                if(node.fCost <= lowestCostNode.fCost && node.hCost < lowestCostNode.hCost) {
                    lowestCostNode = node;
                    }
            }
            openNodes.Remove(lowestCostNode);
            closedNodes.Add(lowestCostNode);
            print("lowestCostNode == targetNode "+lowestCostNode.x +" "+targetNode.x);
            if(lowestCostNode == targetNode){
                RetracePath(startNode, targetNode);
                print("found path");
                return;
            }

            List<Node> neighbours = grid.GetNeighbours(lowestCostNode);
            foreach(Node neighbour in neighbours){
                if(!neighbour.walkable || closedNodes.Contains(neighbour)){
                    continue;
                }
                int newCostToNeighbour = lowestCostNode.gCost + GetDistance(lowestCostNode, neighbour);
                bool openNodesListContainsNeighbour = openNodes.Contains(neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openNodesListContainsNeighbour){
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = lowestCostNode;
                    if(!openNodesListContainsNeighbour){
                        openNodes.Add(neighbour);
                    }
                }
            }
            
test = false;
        }
    }

    void RetracePath(Node startNode, Node endNode){
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode){
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        // Send path wherever you need it
        print(path);
    }

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.x - nodeB.x);
		int dstZ = Mathf.Abs(nodeA.z - nodeB.z);

		if (dstX > dstZ)
			return 14*dstZ + 10* (dstX-dstZ);
		return 14*dstX + 10 * (dstZ-dstX);
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
            // print(snappedCoordinates);
            if (Input.GetMouseButtonDown(0)) {
                print("mouse btn down");
                FindPath(player.transform.position, snappedCoordinates);

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
