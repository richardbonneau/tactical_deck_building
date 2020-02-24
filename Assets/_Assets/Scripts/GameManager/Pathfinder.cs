using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Pathfinder : MonoBehaviour
{
    public Camera camera;
    public GameObject mapSelector;
    PlayerStatus playerStatus;
    NavMeshAgent playerAgent;
    GridCreator grid;
    List<GameObject> pathView = new List<GameObject>();

    public GameObject pathIndicator;
    

    void Awake(){
        grid = GetComponent<GridCreator>();
        playerStatus = transform.parent.GetChild(1).GetComponent<PlayerStatus>();
        playerAgent = playerStatus.player.GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        
        
        
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
                playerStatus.playerNode = lowestCostNode;
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
         print("before coroutine");
        // StartCoroutine(MoveAgent(path));
        foreach (GameObject sphere in pathView){
            Destroy(sphere);
        }
        foreach(Node node in path){
            GameObject p = Instantiate(pathIndicator, node.worldPosition, Quaternion.identity);
            pathView.Add(p);
        }
    }

    IEnumerator MoveAgent(List<Node> path){
        while(true){
                
        yield return new WaitForSeconds(0.01f);

// print("in move agent "+path[0].worldPosition + " 1:"+(path[0].worldPosition.x != playerStatus.player.transform.position.x)+" 2:" +(path[0].worldPosition.z != playerStatus.player.transform.position.z));
        // print("path[0].worldPosition.x "+ path[0].worldPosition.x+ " != " + "playerStatus.player.transform.position.x "+ playerStatus.player.transform.position.x+ "&& "+ "path[0].worldPosition.z "+path[0].worldPosition.z+ "!= "+ "playerStatus.player.transform.position.z "+playerStatus.player.transform.position.z);
        if(path.Count == 0) print("path list empty");
        else if(path[0].worldPosition.x != playerStatus.player.transform.position.x && path[0].worldPosition.z != playerStatus.player.transform.position.z){
            print("movin");
            playerStatus.player.transform.position = path[0].worldPosition;
            // playerAgent.destination = path[0].worldPosition;
        } else if(path.Count != 0)path.RemoveAt(0);
       
        }
    }
        
    //     // grid.path = path;
    // }

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
                FindPath(playerStatus.playerNode.worldPosition, snappedCoordinates);

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
