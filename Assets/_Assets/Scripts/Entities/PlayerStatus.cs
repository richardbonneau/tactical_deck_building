using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public GridCreator gridCreator;
    public int allowedMovement = 5;
    public Node playerNode;
    public GameObject player;
    public int health = 15;
    public bool isDead = false;
    Animator animator;
    // if more than one vector is changed, its a diagonal movement, therefore it costs 2
    void Start()
    {
        animator = GetComponent<Animator>();
        playerNode = gridCreator.NodeFromWorldPoint(player.transform.position);
        playerNode.walkable = false;
    }
    void Update()
    {
        if (!isDead)
        {
            if (health <= 0)
            {
                isDead = true;
                animator.SetBool("isDead", true);
            }
        }
    }
}
