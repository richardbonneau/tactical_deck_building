using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{

    public GameObject player;
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
    Vector3 lastCalculatedMovePath = new Vector3(0, 20, 0);
    bool availableMovementsGridShown = false;
    [System.NonSerialized] public bool enemyIsAllowedToMove = false;
    bool canMoveToSelectedSpot = false;
    List<Node> destination;
    EnemyStatus enemyStatus;
    List<Node> path = new List<Node>();
    int movesMade = 0;

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
        if (movesMade >= enemyStatus.allowedMovement || destination.Count == 0)
        {
            isCurrentlyMoving = false;
            canMoveToSelectedSpot = false;
            animator.SetBool("isMoving", false);
            path = new List<Node>();
            movesMade = 0;

            enemyStatus.NextAction();
            enemyStatus.currentlyDoingAnAction = false;
        }
        else
        {
            EntityMoveToNextNode();
            if (destination[0].worldPosition == this.transform.position)
            {
                movesMade++;
                destination.RemoveAt(0);
            }
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
        if (!enemyStatus.isDead)
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
                if (!canMoveToSelectedSpot)
                {
                    // player position needs to be the closest neighbour node, and NOT the player node itself. that node is an OBSTACLE
                    // What neighbouring node do we want to go to?
                    List<Node> neighbouringPlayerNodes = grid.GetNeighbours(grid.NodeFromWorldPoint(player.transform.position));
                    int pathCount = 9999;
                    foreach (Node node in neighbouringPlayerNodes)
                    {
                        // Instantiate(player, node.worldPosition, Quaternion.identity);
                        List<Node> temporaryPath = grid.FindPath(this.transform.position, node.worldPosition);
                        if (temporaryPath.Count != 0 && temporaryPath.Count < pathCount)
                        {
                            path = temporaryPath;
                            pathCount = path.Count;
                        }
                    }
                    canMoveToSelectedSpot = true;
                    return;
                }

                else if (canMoveToSelectedSpot)
                {
                    removeMovementGrid();
                    isAllowedToMove = false;
                    destination = path;
                    isCurrentlyMoving = true;
                    animator.SetBool("isMoving", true);
                    enemyNode.walkable = true;
                    enemyNode = path[path.Count - 1];
                    enemyNode.walkable = false;
                }

            }
        }
    }
}
