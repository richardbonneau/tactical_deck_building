using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    GridCreator gridScript;
    public int allowedMovement = 5;
    public Node playerNode;
    public GameObject player;
    // if more than one vector is changed, its a diagonal movement, therefore it costs 2
    void Start()
    {
        gridScript = transform.parent.GetChild(0).GetComponent<GridCreator>();
        playerNode = gridScript.NodeFromWorldPoint(player.transform.position);
        playerNode.walkable = false;
    }
}
