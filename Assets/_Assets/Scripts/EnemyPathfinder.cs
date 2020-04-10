﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{

    public Camera mainCam;
    public GameObject mapSelector;
    Node enemyNode;
    Animator animator;
    bool isCurrentlyMoving = false;
    public bool isAllowedToMove = false;
    public int moveSpeed = 1;
    public GridCreator grid;
    List<GameObject> gridView = new List<GameObject>();
    List<GameObject> pathView = new List<GameObject>();

    public ObjectPooler moveGridPool;

    public ObjectPooler movePathPool;

    bool availableMovementsGridShown = false;
    [System.NonSerialized] public bool enemyIsAllowedToMove = false;
    bool canMoveToSelectedSpot = false;
    Vector3 lastCalculatedMovePath = new Vector3(0, 20, 0);
    List<Node> destination;
    EnemyStatus enemyStatus;
    List<Node> path;

    void Awake()
    {
        enemyStatus = this.GetComponent<EnemyStatus>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {

        enemyNode = grid.NodeFromWorldPoint(this.transform.position);

    }

    void EntityCurrentlyMoving()
    {
        if (destination.Count == 0)
        {
            isCurrentlyMoving = false;
            animator.SetBool("isMoving", false);
        }
        else
        {
            EntityMoveToNextNode();
            if (destination[0].worldPosition == this.transform.position) destination.RemoveAt(0);
        }
    }
    void EntityMoveToNextNode()
    {
        transform.LookAt(destination[0].worldPosition);
        transform.position = Vector3.MoveTowards(this.transform.position, destination[0].worldPosition, moveSpeed * Time.deltaTime);
    }

    void removeMovementPath()
    {
        foreach (GameObject indicator in pathView)
        {
            indicator.SetActive(false);
        }
        pathView = new List<GameObject>();
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
    void Update()
    {
        {
            if (isCurrentlyMoving)
            {
                EntityCurrentlyMoving();
            }

            else if (isAllowedToMove)
            {
                if (!availableMovementsGridShown)
                {
                    removeMovementGrid();
                    int maxMove = enemyStatus.allowedMovement;
                    int enemyPosX = Mathf.RoundToInt(this.transform.position.x);
                    int enemyPosZ = Mathf.RoundToInt(this.transform.position.z);

                    for (int x = enemyPosX - maxMove; x <= enemyPosX + maxMove; x++)
                    {
                        for (int z = enemyPosZ - maxMove; z <= enemyPosZ + maxMove; z++)
                        {
                            if (x == enemyPosX && z == enemyPosZ) continue;
                            Node currentNode = grid.NodeFromWorldPoint(new Vector3(x, 0, z));

                            List<Node> path = null;
                            if (currentNode != null) path = grid.FindPath(this.transform.position, currentNode.worldPosition);

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
                        canMoveToSelectedSpot = false;
                        removeMovementPath();

                        path = grid.FindPath(this.transform.position, mouseSelectWorldPosition);


                        foreach (GameObject gridSquare in gridView)
                        {
                            if (path.Count != 0 && gridSquare.transform.position == path[path.Count - 1].worldPosition)
                            {
                                canMoveToSelectedSpot = true;

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
                    if (Input.GetMouseButtonDown(0) && canMoveToSelectedSpot)
                    {
                        removeMovementGrid();

                        isAllowedToMove = false;
                        destination = path;
                        isCurrentlyMoving = true;
                        animator.SetBool("isMoving", true);
                        enemyNode.walkable = true;
                        enemyNode = grid.NodeFromWorldPoint(mouseSelectWorldPosition);
                        enemyNode.walkable = false;
                        availableMovementsGridShown = false;
                    }
                }
                else mapSelector.transform.position = new Vector3(0, 20, 0);
            }
        }
    }
}
