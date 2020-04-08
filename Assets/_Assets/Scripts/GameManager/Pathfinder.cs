using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Pathfinder : MonoBehaviour
{
    public Camera mainCam;
    public GameObject mapSelector;
    PlayerStatus playerStatus;
    public int moveSpeed = 1;

    GridCreator grid;
    List<GameObject> gridView = new List<GameObject>();
    List<GameObject> pathView = new List<GameObject>();

    public GameObject moveGridIndicator;
    public GameObject movePathIndicator;

    bool availableMovementsGridShown = false;
    [System.NonSerialized] public bool playerIsAllowedToMove = false;
    bool playerCanMoveToSelectedSpot = false;
    Vector3 lastCalculatedMovePath = new Vector3(999, 999, 999);
    bool playerIsCurrentlyMoving = false;
    List<Node> destination;
    Animator animator;
    List<Node> path;

    void Awake()
    {
        grid = GetComponent<GridCreator>();
        playerStatus = transform.parent.GetChild(1).GetComponent<PlayerStatus>();
        animator = playerStatus.player.GetComponentInChildren<Animator>();
    }


    List<Node> FindPath(Vector3 startPos, Vector3 endPos)
    {
        // print("find path method");

        // Nodes that we want to calculate the F cost of
        List<Node> openNodes = new List<Node>();
        // Nodes that have already been evaluated
        List<Node> closedNodes = new List<Node>();

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(endPos);
        openNodes.Add(startNode);
        while (targetNode != null && openNodes.Count > 0)
        {
            Node lowestCostNode = openNodes[0];
            foreach (Node node in openNodes)
            {

                if (node.fCost <= lowestCostNode.fCost && node.hCost < lowestCostNode.hCost)
                {
                    lowestCostNode = node;
                }
            }
            openNodes.Remove(lowestCostNode);
            closedNodes.Add(lowestCostNode);

            if (lowestCostNode == targetNode)
            {
                List<Node> path = new List<Node>();
                Node currentNode = targetNode;
                while (currentNode != startNode)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                return path;
            }

            List<Node> neighbours = grid.GetNeighbours(lowestCostNode);
            foreach (Node neighbour in neighbours)
            {
                if (!neighbour.walkable || closedNodes.Contains(neighbour))
                {
                    continue;
                }
                int newCostToNeighbour = lowestCostNode.gCost + GetDistance(lowestCostNode, neighbour);
                bool openNodesListContainsNeighbour = openNodes.Contains(neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openNodesListContainsNeighbour)
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = lowestCostNode;
                    if (!openNodesListContainsNeighbour)
                    {
                        openNodes.Add(neighbour);
                    }
                }
            }
        }
        // print("return empty list");
        return new List<Node>();
    }


    IEnumerator MoveAgent(List<Node> path)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (path.Count != 0 && path[0].worldPosition.x != playerStatus.player.transform.position.x && path[0].worldPosition.z != playerStatus.player.transform.position.z)
            {
                // print("movin");
                playerStatus.player.transform.position = path[0].worldPosition;

            }
            else if (path.Count != 0) path.RemoveAt(0);

        }
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstZ = Mathf.Abs(nodeA.z - nodeB.z);

        if (dstX > dstZ)
            return 14 * dstZ + 10 * (dstX - dstZ);
        return 14 * dstX + 10 * (dstZ - dstX);
    }




    // Update is called once per frame
    void Update()
    {
        if (playerIsCurrentlyMoving)
        {
            if (destination.Count == 0)
            {
                playerIsCurrentlyMoving = false;
            }
            else
            {
                if (destination[0].worldPosition == playerStatus.player.transform.position && destination.Count != 1) destination.RemoveAt(0);
                playerStatus.player.transform.position = Vector3.MoveTowards(playerStatus.player.transform.position, destination[0].worldPosition, moveSpeed * Time.deltaTime);

            }
        }

        if (playerIsAllowedToMove)
        {
            if (!availableMovementsGridShown)
            {
                DateTime before = DateTime.Now;


                grid.ResetAllNodeCosts();
                foreach (GameObject gridObj in gridView)
                {
                    Destroy(gridObj);
                }
                gridView = new List<GameObject>();

                int maxMove = playerStatus.remainingMovements;
                int playerPosX = Mathf.RoundToInt(playerStatus.playerNode.worldPosition.x);
                int playerPosZ = Mathf.RoundToInt(playerStatus.playerNode.worldPosition.z);

                for (int x = playerPosX - maxMove; x <= playerPosX + maxMove; x++)
                {
                    for (int z = playerPosZ - maxMove; z <= playerPosZ + maxMove; z++)
                    {
                        if (x == playerPosX && z == playerPosZ) continue;
                        Node currentNode = grid.NodeFromWorldPoint(new Vector3(x, 0, z));

                        List<Node> path = null;
                        if (currentNode != null) path = FindPath(playerStatus.playerNode.worldPosition, currentNode.worldPosition);

                        if (path != null && path.Count > 0 && path[path.Count - 1].gCost <= maxMove * 10)
                        {
                            GameObject p = Instantiate(moveGridIndicator, new Vector3(x, 0, z), Quaternion.identity);
                            gridView.Add(p);
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
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 mouseSelectWorldPosition = new Vector3(Mathf.Round(hit.point.x), mapSelector.transform.position.y, Mathf.Round(hit.point.z));
                Transform objectHit = hit.transform;
                mapSelector.transform.position = mouseSelectWorldPosition;
                if (lastCalculatedMovePath != mouseSelectWorldPosition)
                {

                    playerCanMoveToSelectedSpot = false;
                    foreach (GameObject indicator in pathView)
                    {
                        Destroy(indicator);
                    }
                    pathView = new List<GameObject>();

                    path = FindPath(playerStatus.playerNode.worldPosition, mouseSelectWorldPosition);


                    foreach (GameObject gridSquare in gridView)
                    {
                        if (path.Count != 0 && gridSquare.transform.position == path[path.Count - 1].worldPosition)
                        {
                            playerCanMoveToSelectedSpot = true;

                            foreach (Node node in path)
                            {
                                GameObject p = Instantiate(movePathIndicator, node.worldPosition, Quaternion.identity);
                                pathView.Add(p);
                            }
                        }
                    }
                    lastCalculatedMovePath = mouseSelectWorldPosition;
                    return;
                }
                if (Input.GetMouseButtonDown(0) && playerCanMoveToSelectedSpot)
                {
                    // playerStatus.player.transform.position = mouseSelectWorldPosition;
                    destination = path;
                    // animator.SetBool("Moving", true);
                    // animator.SetBool("Sprint", true);
                    // animator.SetFloat("Velocity Z", 1f);
                    // animator.SetFloat("Velocity X", 1f);
                    playerIsCurrentlyMoving = true;
                    playerStatus.playerNode.walkable = true;
                    playerStatus.playerNode = grid.NodeFromWorldPoint(mouseSelectWorldPosition);
                    playerStatus.playerNode.walkable = false;
                    availableMovementsGridShown = false;
                }

            }
            else mapSelector.transform.position = new Vector3(999, mapSelector.transform.position.y, 999);
        }


    }
}

