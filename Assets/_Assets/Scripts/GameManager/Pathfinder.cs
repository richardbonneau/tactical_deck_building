using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Pathfinder : MonoBehaviour
{
    public Camera mainCam;
    public GameObject mapSelector;
    PlayerStatus playerStatus;
    public GameObject player;
    int moveSpeed = 4;

    public int maxMove = 10;
    GridCreator grid;
    List<GameObject> gridView = new List<GameObject>();
    List<GameObject> pathView = new List<GameObject>();

    public ObjectPooler moveGridPool;

    public ObjectPooler movePathPool;

    bool availableMovementsGridShown = false;
    [System.NonSerialized] public bool playerIsAllowedToMove = false;
    bool playerCanMoveToSelectedSpot = false;
    Vector3 lastCalculatedMovePath = new Vector3(0, 20, 0);
    bool playerIsCurrentlyMoving = false;
    List<Node> destination;
    Animator animator;
    List<Node> path;
    [System.NonSerialized] public CardAbilities originCard;

    void Awake()
    {
        grid = GetComponent<GridCreator>();
        playerStatus = player.GetComponent<PlayerStatus>();
        animator = player.GetComponent<Animator>();
    }

    void EntityCurrentlyMoving()
    {
        if (destination.Count == 0)
        {
            playerIsCurrentlyMoving = false;
            animator.SetBool("isMoving", false);
            removeMovementPath();
            originCard.NextAction();
        }
        else
        {
            EntityMoveToNextNode();
            if (destination[0].worldPosition == player.transform.position) destination.RemoveAt(0);
        }
    }

    void EntityMoveToNextNode()
    {
        player.transform.LookAt(destination[0].worldPosition);
        player.transform.position = Vector3.MoveTowards(player.transform.position, destination[0].worldPosition, moveSpeed * Time.deltaTime);
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

                int playerPosX = Mathf.RoundToInt(player.transform.position.x);
                int playerPosZ = Mathf.RoundToInt(player.transform.position.z);

                for (int x = playerPosX - maxMove; x <= playerPosX + maxMove; x++)
                {
                    for (int z = playerPosZ - maxMove; z <= playerPosZ + maxMove; z++)
                    {
                        if (x == playerPosX && z == playerPosZ) continue;
                        Node currentNode = grid.NodeFromWorldPoint(new Vector3(x, 0, z));

                        List<Node> path = null;
                        if (currentNode != null) path = grid.FindPath(player.transform.position, currentNode.worldPosition);

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
                Vector3 mouseSelectWorldPosition = new Vector3(Mathf.Round(hit.point.x), 0, Mathf.Round(hit.point.z));
                Transform objectHit = hit.transform;
                mapSelector.transform.position = mouseSelectWorldPosition;

                if (lastCalculatedMovePath != mouseSelectWorldPosition)
                {
                    playerCanMoveToSelectedSpot = false;
                    removeMovementPath();

                    path = grid.FindPath(player.transform.position, mouseSelectWorldPosition);

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

                }
            }
            else mapSelector.transform.position = new Vector3(0, 20, 0);
        }
    }
}

