using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;   // checar se o node é ´válido, no caso, pode "passar"
    public Vector3 WorldPosition;   // checa onde no grid ele está

    public int gCost;
    public int hCost;

    public int gridX;
    public int gridY;

    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        WorldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost { get{return gCost + hCost;} }

}
