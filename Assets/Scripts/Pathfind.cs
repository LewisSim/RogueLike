using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;
using System;

public class Pathfind : MonoBehaviour
{
    Grid grid;

    //public Transform seek, target;

    ReqManager requestManager;

    private void Awake()
    {
        requestManager = GetComponent<ReqManager>();
        grid = GetComponent<Grid>();
    }

    /*private void Update()
    {
        findPath(seek.position, target.position);
    }*/


    IEnumerator findPath(Vector3 startPos, Vector3 targetPos)//void
    {
        //Debug.Log("finding path");

        Vector3[] waypoints = new Vector3[0];//0
        bool pathSuccess = false;


        Node startNode = grid.nodeFromWorldPos(startPos);
        //Debug.Log("start node = " + startNode.gridX + " and " + startNode.gridY);//currently works

        Node targetNode = grid.nodeFromWorldPos(targetPos);


        if (startNode.traversable && targetNode.traversable)
        {


            List<Node> openNodes = new List<Node>();//nodes that can be searched
            HashSet<Node> closedNodes = new HashSet<Node>();



            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                Node currentNode = openNodes[0];//start
                for (int i = 0; i < openNodes.Count; i++)
                {
                    if (openNodes[i].fCost < currentNode.fCost || openNodes[i].fCost == currentNode.fCost && openNodes[i].hCost < currentNode.hCost)//checks distance value to each surrounding node
                    {
                        currentNode = openNodes[i];

                    }
                }

                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                if (currentNode == targetNode)//has reached the target
                {
                    //////retrace(startNode, targetNode);
                    pathSuccess = true;
                    break;
                    ////////return;//exit search
                }

                foreach (Node neigbour in grid.getBorderNodes(currentNode))/////////////////
                {
                    if (!neigbour.traversable || closedNodes.Contains(neigbour))//skips weaker nodes or ones that are intraversable
                    {


                        continue;
                    }
                    //Debug.Log("working");
                    int newMoveCost = currentNode.gCost + getDistance(currentNode, neigbour);
                    if (newMoveCost < neigbour.gCost || !openNodes.Contains(neigbour))
                    {
                        neigbour.gCost = newMoveCost;
                        neigbour.hCost = getDistance(neigbour, targetNode);
                        neigbour.Parent = currentNode;

                        if (!openNodes.Contains(neigbour))
                        {
                            openNodes.Add(neigbour);
                        }

                    }


                }

            }

        }
        yield return null;//wait 1 frame

        if (pathSuccess)
        {
            waypoints = retrace(startNode, targetNode);


        }
        requestManager.FinishedProcessing(waypoints, pathSuccess);


    }


    Vector3[] retrace(Node startNode, Node endNode)//void
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        if (currentNode == startNode)///////
        {
            path.Add(currentNode);
        }


        Vector3[] waypoints = SimplifyPath(path);
        //////path.Reverse();
        //waypoints.Reverse();
        Array.Reverse(waypoints);




        //grid.path = path;//for dwaring

        return waypoints;





    }




    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

            if (newDirection != oldDirection)
            {
                ////////waypoints.Add(path[i].location);

                waypoints.Add(path[i - 1].location);

                //Debug.Log("Next waypoint = " + path[i].location);
                //print("Next waypoint = " + path[i].location);

            }
            oldDirection = newDirection;
        }
        return waypoints.ToArray();
    }


    int getDistance(Node NodeA, Node NodeB)
    {
        int distX = Mathf.Abs(NodeA.gridX - NodeB.gridX);
        int distY = Mathf.Abs(NodeA.gridY - NodeB.gridY);


        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);

        }

    }

    public void StartFindingPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(findPath(startPos, targetPos));
    }

}
