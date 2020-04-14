using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GridCreator gridCreator;
    void Awake()
    {
        gridCreator = GameObject.FindWithTag("GridManager").GetComponent<GridCreator>();
    }
    void Start()
    {
        Node node = gridCreator.NodeFromWorldPoint(this.transform.position);
        if(node != null) node.walkable = false;
      
    }


}
