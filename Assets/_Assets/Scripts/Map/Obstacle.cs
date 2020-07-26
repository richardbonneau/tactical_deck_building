using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GridCreator gridCreator;

    void Start()
    {
        gridCreator = GameObject.FindWithTag("GridManager").GetComponent<GridCreator>();
        Node node = gridCreator.NodeFromWorldPoint(this.transform.position);
        if (node != null) node.walkable = false;

    }


}
