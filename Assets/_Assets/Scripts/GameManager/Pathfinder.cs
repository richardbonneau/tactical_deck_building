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

    public ObjectPooler moveGridPool;

    public ObjectPooler movePathPool;

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
        return new List<Node>();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstZ = Mathf.Abs(nodeA.z - nodeB.z);

        if (dstX > dstZ)
            return 14 * dstZ + 10 * (dstX - dstZ);
        return 14 * dstX + 10 * (dstZ - dstX);
    }


    void EntityCurrentlyMoving()
    {
        if (destination.Count == 0)
        {
            playerIsCurrentlyMoving = false;
            animator.SetBool("isMoving", false);
            removeMovementPath();
        }
        else
        {
            EntityMoveToNextNode();
            if (destination[0].worldPosition == playerStatus.player.transform.position) destination.RemoveAt(0);
        }
    }
    void EntityMoveToNextNode()
    {
        playerStatus.player.transform.LookAt(destination[0].worldPosition);
        playerStatus.player.transform.position = Vector3.MoveTowards(playerStatus.player.transform.position, destination[0].worldPosition, moveSpeed * Time.deltaTime);
    }
    public void removeMovementGrid()
    {
        grid.ResetAllNodeCosts();
        foreach (GameObject gridObj in gridView)
        {
            gridObj.SetActive(false);
        }
        gridView = new List<GameObject>();
        availableMovementsGridShown = false;
    }
    void removeMovementPath()
    {
        foreach (GameObject indicator in pathView)
        {
            indicator.SetActive(false);
        }
        pathView = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsCurrentlyMoving)
        {
            EntityCurrentlyMoving();
        }

        else if (playerIsAllowedToMove)
        {
            if (!availableMovementsGridShown)
            {
                removeMovementGrid();
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
                            GameObject p = moveGridPool.GetPooledObject();
                            if (p != null)
                            {
                                p.transform.position = new Vector3(x, 0, z);
                                p.SetActive(true);
                            }
                            gridView.Add(p);
                        }

                    }
                }
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
                    removeMovementPath();

                    path = FindPath(playerStatus.playerNode.worldPosition, mouseSelectWorldPosition);


                    foreach (GameObject gridSquare in gridView)
                    {
                        if (path.Count != 0 && gridSquare.transform.position == path[path.Count - 1].worldPosition)
                        {
                            playerCanMoveToSelectedSpot = true;

                            foreach (Node node in path)
                            {
                                GameObject p = movePathPool.GetPooledObject();
                                if (p != null)
                                {
                                    p.transform.position = node.worldPosition;
                                    p.SetActive(true);
                                }
                                pathView.Add(p);
                            }
                        }
                    }
                    lastCalculatedMovePath = mouseSelectWorldPosition;
                    return;
                }
                if (Input.GetMouseButtonDown(0) && playerCanMoveToSelectedSpot)
                {
                    removeMovementGrid();

                    playerIsAllowedToMove = false;
                    destination = path;
                    playerIsCurrentlyMoving = true;
                    animator.SetBool("isMoving", true);
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

