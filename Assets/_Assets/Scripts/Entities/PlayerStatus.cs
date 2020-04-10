using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public GridCreator gridCreator;
    public int allowedMovement = 5;
    public Node playerNode;
    public GameObject player;
    // if more than one vector is changed, its a diagonal movement, therefore it costs 2
    void Start()
    {
        playerNode = gridCreator.NodeFromWorldPoint(player.transform.position);
        playerNode.walkable = false;
    }
}
