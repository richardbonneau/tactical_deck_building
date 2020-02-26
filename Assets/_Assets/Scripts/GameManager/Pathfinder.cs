using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Pathfinder : MonoBehaviour
{
    public Camera mainCam;
    public GameObject mapSelector;
    PlayerStatus playerStatus;

    GridCreator grid;
    List<GameObject> gridView = new List<GameObject>();
    List<GameObject> pathView = new List<GameObject>();

    public GameObject moveGridIndicator;
    public GameObject movePathIndicator;

    bool availableMovementsGridShown = false;
    bool playerCanMoveToSelectedSpot = false;
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
        // print("return empty list");
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
        if(path.Count != 0 && path[0].worldPosition.x != playerStatus.player.transform.position.x && path[0].worldPosition.z != playerStatus.player.transform.position.z){
            // print("movin");
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
            DateTime before = DateTime.Now;


            grid.ResetAllNodeCosts();
            foreach(GameObject gridObj in gridView){
                Destroy(gridObj);
            }
            gridView = new List<GameObject>();
            // print("calculating move grid"+ playerStatus.playerNode.worldPosition);
            int maxMove = playerStatus.remainingMovements;
            int playerPosX = Mathf.RoundToInt(playerStatus.playerNode.worldPosition.x);
            int playerPosZ = Mathf.RoundToInt(playerStatus.playerNode.worldPosition.z);
            // print("x: "+(playerPosX-maxMove)+" to "+(playerPosX+maxMove));
            // print("z: "+(playerPosZ-maxMove)+" to "+(playerPosZ+maxMove));
            for(int x= playerPosX-maxMove;x<= playerPosX+maxMove; x++){
                for(int z = playerPosZ-maxMove; z<=playerPosZ+maxMove;z++){
                    if(x == playerPosX && z == playerPosZ) continue;
                    Node currentNode = grid.NodeFromWorldPoint(new Vector3(x,0,z));
                    // print(currentNode.worldPosition);
                    // print("findpath: "+playerStatus.playerNode.worldPosition+" "+(new Vector3(x,0,z)));
                    List<Node> path = FindPath(playerStatus.playerNode.worldPosition, currentNode.worldPosition);
                    // if(path.Count > 0)print("path first index:"+path[0].worldPosition);


                    if(path.Count > 0 && path[path.Count-1].gCost <= maxMove* 10) {
                        // print("instantiate");
                        // print("instantiate gameobject");
                        GameObject p = Instantiate(moveGridIndicator, new Vector3(x,0,z), Quaternion.identity);
                        gridView.Add(p);
                        // 
                    }
                }
            }
            DateTime after = DateTime.Now; 
            TimeSpan duration = after.Subtract(before);
            Debug.Log("Duration in milliseconds: " + duration.Milliseconds);
            availableMovementsGridShown = true;
        }

        // Calculate MoveTo path

        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit)) {
        Vector3 mouseSelectWorldPosition = new Vector3(Mathf.Round(hit.point.x),mapSelector.transform.position.y,Mathf.Round(hit.point.z));
        Transform objectHit = hit.transform;
        mapSelector.transform.position = mouseSelectWorldPosition;
        // print("lastCalculatedMovePath != mouseSelectWorldPosition "+lastCalculatedMovePath +" "+mouseSelectWorldPosition);
        if(lastCalculatedMovePath != mouseSelectWorldPosition){
            // print("calculating");
            playerCanMoveToSelectedSpot = false;
            foreach (GameObject indicator in pathView){
                Destroy(indicator);
            }
            pathView = new List<GameObject>();
            List<Node> path = FindPath(playerStatus.playerNode.worldPosition, mouseSelectWorldPosition);
            // print(path);
            
            foreach(GameObject gridSquare in gridView ){
                if(path.Count != 0 && gridSquare.transform.position == path[path.Count-1].worldPosition){
                    playerCanMoveToSelectedSpot = true;
                    // print("node found in grid");
                    // print("instantiating path objs "+path.Count);
                    foreach(Node node in path){
                        GameObject p = Instantiate(movePathIndicator, node.worldPosition, Quaternion.identity);
                        pathView.Add(p);
                    }
                }  
            }
                 lastCalculatedMovePath = mouseSelectWorldPosition;
                return;
        }
        if (Input.GetMouseButtonDown(0) && playerCanMoveToSelectedSpot) {

            playerStatus.player.transform.position = mouseSelectWorldPosition;
            playerStatus.playerNode.walkable = true;
            playerStatus.playerNode = grid.NodeFromWorldPoint(mouseSelectWorldPosition);
            playerStatus.playerNode.walkable = false;
            availableMovementsGridShown = false;
            }   
            
        } else mapSelector.transform.position = new Vector3(999,mapSelector.transform.position.y,999);
        
    }
}

