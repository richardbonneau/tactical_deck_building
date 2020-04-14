using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    GridCreator gridCreator;
    public Vector3 newRoomVector;
    public Vector3 newRoomPlayerSpawn;
    void Awake()
    {
        gridCreator = GameObject.FindWithTag("GridManager").GetComponent<GridCreator>();
    }
    void Start()
    {
        Node node = gridCreator.NodeFromWorldPoint(this.transform.position);
        if(node != null) {
            node.isDoor = true;
            node.newRoomVector = newRoomVector;
            node.newRoomPlayerSpawn = newRoomPlayerSpawn;
        }
      
    }

}
