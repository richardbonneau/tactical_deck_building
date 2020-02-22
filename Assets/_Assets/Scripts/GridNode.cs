using System.Collections;

using UnityEngine;



    public class GridNode
        {
          public float x;
          public float z;

          public int gCost;
          public int hCost;
          public int fCost;
          public GridNode parent;
            
          public GridNode(float _xCoordinates, float _zCoordinates){
              x = _xCoordinates;
              z = _zCoordinates;
          }
        }
