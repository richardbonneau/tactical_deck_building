using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Pathfinder pathfinder;
    public GameObject gridManager;
    void Start()
    {
        pathfinder = gridManager.GetComponent<Pathfinder>();
    }
    public void EnablePlayerMove()
    {
        if (pathfinder.playerIsAllowedToMove == false) pathfinder.playerIsAllowedToMove = true;
        else
        {
            pathfinder.removeMovementGrid();
            pathfinder.playerIsAllowedToMove = false;
        }
    }
    public void EnablePlayerAttack()
    {

    }

}
