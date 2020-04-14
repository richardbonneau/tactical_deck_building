using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    GridCreator gridCreator;
    public Vector3 nextRoomVector;
    public Vector3 nextRoomPlayerSpawn;
    public int nextRoomIndex;
    void Awake()
    {
        gridCreator = GameObject.FindWithTag("GridManager").GetComponent<GridCreator>();
    }
    void Start()
    {
        Node node = gridCreator.NodeFromWorldPoint(this.transform.position);
        if (node != null)
        {
            node.isDoor = true;
            node.nextRoomVector = nextRoomVector;
            node.nextRoomPlayerSpawn = nextRoomPlayerSpawn;
            node.nextRoomIndex = nextRoomIndex;
        }

    }

}
