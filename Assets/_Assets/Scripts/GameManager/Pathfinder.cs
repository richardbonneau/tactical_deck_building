using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Pathfinder : MonoBehaviour
{
    public Camera camera;
    public GameObject mapSelector;
    PlayerStatus playerStatus;

    GridCreator grid;
    List<GameObject> gridView = new List<GameObject>();
    List<GameObject> pathView = new List<GameObject>();

    public GameObject moveGridIndicator;
    public GameObject movePathIndicator;

    bool availableMovementsGridShown = false;
    Vector3 lastCalculatedMovePath = new Vector3(999,999,999);
    

    void Awake(){
        grid = GetComponent<GridCreator>();
        playerStatus = transform.parent.GetChild(1).GetComponent<PlayerStatus>();

    }


    List<Node> FindPath(Vector3 startPos,Vector3 endPos){
        // print("find path method");
    
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

            if(lowestCostNode == targetNode){
                List<Node> path = new List<Node>();
                Node currentNode = targetNode;
                while (currentNode != startNode){
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                path.Reverse();

                // playerStatus.playerNode = lowestCostNode;
                return path;
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
        print("return empty list");
        return new List<Node>();
    }



    // void RetracePath(Node startNode, Node endNode){
     



    //     playerStatus.player.transform.position = path[path.Count-1].worldPosition;
    //     foreach (GameObject sphere in pathView){
    //         Destroy(sphere);
    //     }
    //     foreach(Node node in path){
    //         GameObject p = Instantiate(pathIndicator, node.worldPosition, Quaternion.identity);
    //         pathView.Add(p);
    //     }
        
    // }

    IEnumerator MoveAgent(List<Node> path){
        while(true){
                
        yield return new WaitForSeconds(0.01f);

// print("in move agent "+path[0].worldPosition + " 1:"+(path[0].worldPosition.x != playerStatus.player.transform.position.x)+" 2:" +(path[0].worldPosition.z != playerStatus.player.transform.position.z));
        // print("path[0].worldPosition.x "+ path[0].worldPosition.x+ " != " + "playerStatus.player.transform.position.x "+ playerStatus.player.transform.position.x+ "&& "+ "path[0].worldPosition.z "+path[0].worldPosition.z+ "!= "+ "playerStatus.player.transform.position.z "+playerStatus.player.transform.position.z);
        if(path.Count == 0) print("path list empty");
        else if(path[0].worldPosition.x != playerStatus.player.transform.position.x && path[0].worldPosition.z != playerStatus.player.transform.position.z){
            print("movin");
            playerStatus.player.transform.position = path[0].worldPosition;

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
        if(!availableMovementsGridShown){
            int maxMove = 5;
            int playerPosX = Mathf.RoundToInt(playerStatus.playerNode.worldPosition.x);
            int playerPosZ = Mathf.RoundToInt(playerStatus.playerNode.worldPosition.z);
            for(int x= playerPosX-maxMove;x<= playerPosX+maxMove; x++){
                for(int z = playerPosZ-maxMove; z<=playerPosZ+maxMove;z++){
                    if(x == playerPosX && z == playerPosZ) continue;
                    List<Node> path = FindPath(playerStatus.playerNode.worldPosition, new Vector3(x,0,z));
                    // print(x+" "+z+" pathcount :"+path.Count);
                    if(path.Count > 0 && path[path.Count-1].gCost <= maxMove* 10) {
                        print("instantiate gameobject");
                        GameObject p = Instantiate(moveGridIndicator, new Vector3(x,0,z), Quaternion.identity);
                        gridView.Add(p);
                    }
                }
            }
            availableMovementsGridShown = true;
        }




        // Calculate MoveTo path

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit)) {
        Vector3 mouseSelectWorldPosition = new Vector3(Mathf.Round(hit.point.x),mapSelector.transform.position.y,Mathf.Round(hit.point.z));
        Transform objectHit = hit.transform;
        mapSelector.transform.position = mouseSelectWorldPosition;
        // print("lastCalculatedMovePath != mouseSelectWorldPosition "+lastCalculatedMovePath +" "+mouseSelectWorldPosition);
        if(lastCalculatedMovePath != mouseSelectWorldPosition){
            print("calculating");
            foreach (GameObject indicator in pathView){
                Destroy(indicator);
            }
            pathView = new List<GameObject>();
            List<Node> path = FindPath(playerStatus.playerNode.worldPosition, mouseSelectWorldPosition);
            print(path);
            
            foreach(GameObject gridSquare in gridView ){
                if(path.Count != 0 && gridSquare.transform.position == path[path.Count-1].worldPosition){
                    print("node found in grid");
                    // print("instantiating path objs "+path.Count);
                    foreach(Node node in path){
                        GameObject p = Instantiate(movePathIndicator, node.worldPosition, Quaternion.identity);
                        pathView.Add(p);
                    }
                }  
            }
         print("node not found");
                 lastCalculatedMovePath = mouseSelectWorldPosition;
                return;
        }
        // print(mouseSelectWorldPosition);
        if (Input.GetMouseButtonDown(0)) {
            print("mouse btn down");

            // playerstatus.currentCoordinates = mouseSelectWorldPosition;
            // I need an array of destinations that the agent will go through one by one
            }   
            
        } else mapSelector.transform.position = new Vector3(999,mapSelector.transform.position.y,999);
        
    }
}

