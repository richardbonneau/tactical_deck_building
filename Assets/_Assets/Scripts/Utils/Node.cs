using System.Collections;
using UnityEngine;

public class Node
{
    public int x;
    public int z;
    public Vector3 worldPosition;
    public bool walkable;
    public bool lootable = false;
    public bool isDoor = false;
    public Vector3 nextRoomVector;
    public Vector3 nextRoomPlayerSpawn;
    public int nextRoomIndex;

    // Distance from Starting Node
    public int gCost;
    // Distance from End Node
    public int hCost;
    // gCost + hCost
    public Node parent;

    // Put in place some kind of collider detection


    public Node(int _xCoordinates, int _zCoordinates, Vector3 _worldPosition, bool _walkable)
    {
        x = _xCoordinates;
        z = _zCoordinates;
        worldPosition = _worldPosition;
        walkable = _walkable;
    }
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public void Reset()
    {
        gCost = 0;
        hCost = 0;
    }
}
