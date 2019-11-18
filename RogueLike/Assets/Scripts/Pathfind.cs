using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfind : MonoBehaviour
{

    Grid GridReference;//For referencing the grid class
    public Transform StartPosition;//Starting position to pathfind from
    public Transform TargetPosition;//Starting position to pathfind to
    public List<Node> FinalPath;

    private void Awake()
    {
        GridReference = GetComponent<Grid>();//Get a reference to the game manager
    }

    public void Update()
    {
        FindPath(StartPosition.position, TargetPosition.position);

        StartPosition.transform.position += StartPosition.forward * Time.deltaTime;
    }

    public void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);//gets the node closest to the starting position
        Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);//gets the node closest to the target position

        List<Node> OpenList = new List<Node>();//list of nodes
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);//add the starting node to the open list

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//set the current node to that object
                }
            }
            OpenList.Remove(CurrentNode);//remove that from the open list
            ClosedList.Add(CurrentNode);//and add it to the closed list

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);//calculate the final path
            }

            foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
            {
                if (!NeighborNode.bIsWall || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);//Get the F cost of that neighbor

                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.igCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);//Set the h cost
                    NeighborNode.ParentNode = CurrentNode;//Set the parent of the node for retracing steps

                    if (!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        OpenList.Add(NeighborNode);//Add it to the list
                    }
                }
            }
            
        }
    }

    

    public void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        FinalPath = new List<Node>();//list to hold the path sequentially 
        Node CurrentNode = a_EndNode;//node to store the current node being checked

        while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//Move onto its parent node
        }

        FinalPath.Reverse();//reverse the path

        GridReference.FinalPath = FinalPath;//set the final path

        LookTo();

    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);//y1-y2

        return ix + iy;//Return the sum
    }

    void LookTo()
    {
        Node NextNode = FinalPath.First();

        StartPosition.LookAt(NextNode.vPosition);
    }
}