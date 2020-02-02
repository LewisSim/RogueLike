using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool traversable;
    public Vector3 location;
    public int gCost, hCost, gridX, gridY;

    public Node Parent;

    public Node(bool isTraversable, Vector3 curentLocation, int xPos, int yPos)
    {
        traversable = isTraversable;
        location = curentLocation;
        gridX = xPos;
        gridY = yPos;

    }


    public int fCost//total distance value of path
    {
        get
        {
            return gCost + hCost;
        }
    }

}

